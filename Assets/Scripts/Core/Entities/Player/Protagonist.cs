//Made by Galactspace Studios

using System;
using UnityEngine;
using Core.Controllers;
using Scriptable.Entities;
using System.Threading.Tasks;
using System.Collections.Generic;
using Core.Interactables;

namespace Core.Entities
{
    public class Protagonist : Entity
    {
        public event Action Interact;
        public event Action FreezeInteract;
        public event Action ClimbInteract;
        public event Action WalkInteract;

        private CapsuleCollider _collider;

        [HideInInspector] public Stair CurrentStair;

        private float _curMoveSize;
        public float CurrentMoveSize
        {
            get => _curMoveSize;
            set
            {
                _curMoveSize = value;
                EAnimator.SetFloat("MoveSize", value);
            }
        }

        private StealthKill _currentKillSpace;
        public StealthKill CurrentKillSpace
        {
            get => _currentKillSpace;
            set
            {
                _currentKillSpace = value;
                
                if (!value)
                {
                    InputMan.GameInputs.MeleeAttackChannel.Unlink(KillEnemy);
                    return;
                }

                InputMan.GameInputs.MeleeAttackChannel.Link(KillEnemy);
            }
        }

        private Vector2 _tMoveVector;
        private float _acelerationTimer;

        public void RegisterJump(Func<bool> function)
        {
            Debug.Log("jump registered");
            OnJump += function;
        }
        public void UnregisterJump(Func<bool> function)
        {
            Debug.Log("jump unregistered");
            OnJump -= function;
        }
        private event Func<bool> OnJump = delegate { return true; };

        public void Set_Anim_StairUpper()
        {

        }
        private void Set_Anim_Hideout(bool value)
        {
            if (value)
            {
                EAnimator.Play("Hideout");

                return;
            }
            EAnimator.Play("Idle");
        }
        private void Set_Anim_IsCrouching(bool value) => EAnimator.SetBool("IsCrouching", value);

        public void Set_Anim_ClimbLeft() => EAnimator.SetTrigger("ClimbLeft");
        public void Set_Anim_ClimbRight() => EAnimator.SetTrigger("ClimbRight");
        public void Set_Anim_FinishClimbing() => EAnimator.SetTrigger("FinishClimb");
        public void Set_Anim_IsClimbing(bool value) => EAnimator.SetBool("IsClimbing", value);

        public void Set_Anim_DashBack() => EAnimator.SetTrigger("DashBack");
        public void Set_Anim_DashFront() => EAnimator.SetTrigger("DashFront");

        public void Set_Anim_Jump() => EAnimator.SetTrigger("Jump");

        public Vector3 HeadPosition => transform.position + (Vector3.up * (_collider.height));
        public Vector3 ArmPosition
        {
            get => _armPosition.position;
            set => transform.position = value - _armPosition.localPosition;
        }

        public Vector3 VerticalStairDistance => new Vector3(0, stairAffect.VerticalDistance, 0);
        public Vector3 HorizontalStairDistance => new Vector3(stairAffect.HorizontalDistance, 0, 0);

        public Vector3 UpStair => transform.localToWorldMatrix.MultiplyPoint(stairAffect.CenterOffset + VerticalStairDistance);
        public Vector3 DownStair => transform.localToWorldMatrix.MultiplyPoint(stairAffect.CenterOffset - VerticalStairDistance);
        public Vector3 LeftStair => transform.localToWorldMatrix.MultiplyPoint(stairAffect.CenterOffset + HorizontalStairDistance);
        public Vector3 RightStair => transform.localToWorldMatrix.MultiplyPoint(stairAffect.CenterOffset - HorizontalStairDistance);

        [SerializeField] private StairAffectionSo stairAffect;

        [Space]
        [SerializeField] private Transform _armPosition;

        [Space]
        [SerializeField] private LayerMask _groundMask;
        [SerializeField] private Vector3 _groundOffset;
        [SerializeField] private Vector3 _groundSize;

        private void OnEnable()
        {
            SetMoveStage(MoveStage.Walk);
            InputMan.GameInputs.InteractChannel.Link(OnInteract);
        }
        private void OnDisable()
        {
            SetMoveStage(MoveStage.None);
            InputMan.GameInputs.InteractChannel.Unlink(OnInteract);
        }
        
        protected override void Awake()
        {
            Player = this;
            _collider = GetComponent<CapsuleCollider>();

            base.Awake();
        }
        private void Update()
        {
            MoveVector = Vector2.Lerp(MoveVector, _tMoveVector, Time.deltaTime * 10);

            if (EMoveStage == MoveStage.Walk ||
                EMoveStage == MoveStage.Runner) CurrentMoveSize = 
                    Mathf.Lerp(CurrentMoveSize, Mathf.Abs(MoveVector.x) + AcelerationRate, Time.deltaTime * 10);

            if ((EMoveStage == MoveStage.Runner || IsRunning) && IsCrouching && SlidingCompleted)
            {
                SlidingCompleted = false;
                IsCrouching = false;
                Set_Anim_IsCrouching(false);
            }

            if (_tMoveVector.x != 0 && !IsCrouching && IsRunning) AcelerationRate = Mathf.Lerp(AcelerationRate, Mathf.Abs(MoveVector.x), Time.deltaTime * _config.AcelerationRate);
            else if (!SlidingCompleted && IsCrouching) AcelerationRate = Mathf.Lerp(AcelerationRate, 0, Time.deltaTime * _config.CrouchDeacelerationRate);
            else AcelerationRate = Mathf.Lerp(AcelerationRate, 0, Time.deltaTime * _config.DeacelerationRate);
        }

        private Action OnInteractChoose() => EMoveStage switch
        {
            MoveStage.Walk => Interact + WalkInteract,
            MoveStage.None => Interact + FreezeInteract,
            _ => Interact
        };
        private void OnInteract() => OnInteractChoose()?.Invoke();

        protected override void OnDeath()
        {
            base.OnDeath();
            SceneManager.Reload(true);
        }

        public void SetMoveStageByIndex(int index) => SetMoveStage((MoveStage)index);

        private void OnWalkCrouch()
        {
            if (AcelerationRate > 0.3f) AcelerationRate = 1;
            else SlidingCompleted = true;

            IsCrouching = !IsCrouching;
            if (!IsCrouching) SlidingCompleted = false;

            Set_Anim_IsCrouching(IsCrouching);
        }
        private void OnWalkMovement(Vector2 value)
        {
            _tMoveVector = new Vector3(value.x, 0, 0);
        }

        protected override void EnableWalk(bool v)
        {
            if (v)
            {
                MoveVector = Vector3.zero;
                InputMan.GameInputs.MovementChannel.Link(OnWalkMovement);
                InputMan.GameInputs.CrouchChannel.Link(OnWalkCrouch);
                InputMan.GameInputs.RunChannel.Link(OnRun);
                InputMan.GameInputs.JumpChannel.Link(Jump);
                InputMan.GameInputs.DashChannel.Link(OnDash);
                return;
            }

            MoveVector = Vector3.zero;
            InputMan.GameInputs.MovementChannel.Unlink(OnWalkMovement);
            InputMan.GameInputs.CrouchChannel.Unlink(OnWalkCrouch);
            InputMan.GameInputs.RunChannel.Unlink(OnRun);
            InputMan.GameInputs.JumpChannel.Unlink(Jump);
            InputMan.GameInputs.DashChannel.Unlink(OnDash);
        }
        protected override void EnableRunner(bool v)
        {
            if (v)
            {
                MoveVector = Vector3.zero;
                InputMan.GameInputs.CrouchChannel.Link(OnWalkCrouch);
                InputMan.GameInputs.JumpChannel.Link(Jump);
                return;
            }

            MoveVector = Vector3.zero;
            InputMan.GameInputs.CrouchChannel.Unlink(OnWalkCrouch);
            InputMan.GameInputs.JumpChannel.Unlink(Jump);
        }
        protected override void EnableClimbing(bool v)
        {
            if (v)
            {
                MoveVector = Vector3.zero;
                ERigidbody.useGravity = false;
                InputMan.GameInputs.MovementChannel.Link(OnClimbMovement);
                InputMan.GameInputs.JumpChannel.Link(StairJump);

                Set_Anim_IsClimbing(true);
                return;
            }

            MoveVector = Vector3.zero;
            ERigidbody.useGravity = true;
            InputMan.GameInputs.MovementChannel.Unlink(OnClimbMovement);
            InputMan.GameInputs.JumpChannel.Unlink(StairJump);

            Set_Anim_IsClimbing(false);
        }

        private void OnClimbMovement(Vector2 value)
        {
            _tMoveVector = new Vector3(value.x, value.y, 0);
        }

        public void AddForce(Vector3 force)
        {
            _forces.Add(force);
        }

        private void OnRun(bool isRunning)
        {
            IsRunning = isRunning;
        }

        public void Jump()
        {
            if (Physics.OverlapBox(transform.position + _groundOffset, _groundSize / 2, Quaternion.identity, _groundMask.value).Length <= 0) return;

            if (OnJump.Invoke())
            {
                Set_Anim_Jump();
                ERigidbody.velocity = new Vector3(ERigidbody.velocity.x, _config.JumpSpeed);
                Routinef.Invoke(IsGroundCheck, 0.01f, this);
            }
        }
        private void IsGroundCheck()
        {
            Routinef.LoopWhile(() =>
            {
                IsGrounded = false;

            }, () =>
            Physics.OverlapBox(transform.position + _groundOffset, _groundSize / 2, Quaternion.identity, _groundMask.value).Length <= 0,
            Time.fixedDeltaTime, this, () =>
            {
                IsGrounded = true;
                StairImpulsing = false;
            });
        }

        public void StairJump()
        {
            if (CurrentStair == null) return;
            CurrentStair.JumpInside();
        }
        public void FinishTopStairClimb()
        {
            EAnimator.ResetTrigger("FinishClimb");
            CurrentStair.SetActive(false, true);
            CanMove = true;
        }

        protected override void Dash()
        {
            DashDirection = MoveVector.normalized;
            IsDashing = true;
            StopedDash = false;

            if ((Euler.y == RightAngle && DashDirection.x > 0) ||
                (Euler.y == RightAngle + 180 && DashDirection.x < 0))
            {
                Set_Anim_DashFront();
                return;
            }

            Set_Anim_DashBack();
        }
        public void OnDash(bool value)
        {
            IsDashing = value;
            if (!IsDashing && !StopedDash) StopedDash = true;
        }
        public void DisablePerformingDash()
        {
            SetVelocity(Vector3.zero);
            IsDashing = false;
        }

        public void KillEnemy()
        {
            if (CurrentKillSpace == null || IsAttacking) return;
            EAnimator.SetTrigger("StealthKill");
            CurrentKillSpace.StartedKill();

            Vector3 pos = CurrentKillSpace.KillPosition;
            Lerp(new Vector3(pos.x, transform.position.y, pos.z));
            
            IsAttacking = true;
            CanMove = false;
        }
        public void FinishStealthKill()
        {
            if (CurrentKillSpace == null) return;
            CurrentKillSpace.KillEnemy();
            CurrentKillSpace = null;
            IsAttacking = false;
            CanMove = true;
        }
        
        public void Test()
        {
            Debug.LogWarning("Entered!");
        }

        private void OnDrawGizmosSelected()
        {
            Gizmosf.DrawCubeWithBorder(transform.position + _groundOffset, _groundSize, new Color(0, 1, 0, 0.1f), new Color(0, 1, 0, 1));

            Gizmosf.DrawSphere(UpStair, 0.1f, Color.green);
            Gizmosf.DrawSphere(DownStair, 0.1f, Color.green);
            Gizmosf.DrawSphere(LeftStair, 0.1f, Color.green);
            Gizmosf.DrawSphere(RightStair, 0.1f, Color.green);
        }
    }
}
