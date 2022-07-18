//Made by Galactspace Studios

using System;
using UnityEngine;
using Core.Controllers;
using Scriptable.Entities;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class Protagonist : Entity
    {
        public event Action Interact;
        public event Action FreezeInteract;
        public event Action ClimbInteract;
        public event Action WalkInteract;

        private CapsuleCollider _collider;

        private float _curMoveSize;
        public float CurrentMoveSize
        {
            get => _curMoveSize;
            set
            {
                _curMoveSize = value;
                _entityAnimator.SetFloat("MoveSize", value);
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

        private event Func<bool> OnJump = delegate { return true; };
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

        private void Set_Anim_IsCrouching(bool value) => _entityAnimator.SetBool("IsCrouching", value);
        private void Set_Anim_Hideout(bool value)
        {
            if (value)
            {
                _entityAnimator.Play("Hideout");

                return;
            }
            _entityAnimator.Play("Idle");
        }
        public void Set_Anim_StairUpper()
        {

        }

        public void Set_Anim_IsClimbing(bool value) => _entityAnimator.SetBool("IsClimbing", value);
        public void Set_Anim_FinishClimbing() => _entityAnimator.SetTrigger("FinishClimb");
        public void Set_Anim_ClimbLeft() => _entityAnimator.SetTrigger("ClimbLeft");
        public void Set_Anim_ClimbRight() => _entityAnimator.SetTrigger("ClimbRight");

        public void Set_Anim_DashBack() => _entityAnimator.SetTrigger("DashBack");
        public void Set_Anim_DashFront() => _entityAnimator.SetTrigger("DashFront");

        public void Set_Anim_Jump() => _entityAnimator.SetTrigger("Jump");

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

        protected override void EnablePhysics(bool value)
        {
            base.EnablePhysics(value);
            _collider.enabled = value;
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

            if (EntityMoveStage == MoveStage.Walking ||
                EntityMoveStage == MoveStage.Runner) CurrentMoveSize = Mathf.Lerp(CurrentMoveSize, Mathf.Abs(MoveVector.x) + _acelerationRate, Time.deltaTime * 10);

            if ((EntityMoveStage == MoveStage.Runner || IsRunning) && IsCrouching && SlidingCompleted)
            {
                SlidingCompleted = false;
                IsCrouching = false;
                Set_Anim_IsCrouching(false);
            }

            if (_tMoveVector.x != 0 && !IsCrouching && IsRunning) _acelerationRate = Mathf.Lerp(_acelerationRate, Mathf.Abs(MoveVector.x), Time.deltaTime * configuration.AcelerationRate);
            else if (!SlidingCompleted && IsCrouching) _acelerationRate = Mathf.Lerp(_acelerationRate, 0, Time.deltaTime * configuration.CrouchDeacelerationRate);
            else _acelerationRate = Mathf.Lerp(_acelerationRate, 0, Time.deltaTime * configuration.DeacelerationRate);
        }

        private void OnEnable()
        {
            SetMoveStage(MoveStage.Walking);
            InputMan.GameInputs.InteractChannel.Link(OnInteract);
        }

        private void OnDisable()
        {
            SetMoveStage(MoveStage.Freezing);
            InputMan.GameInputs.InteractChannel.Unlink(OnInteract);
        }

        private void OnInteract() => OnInteractChoose()?.Invoke();

        private Action OnInteractChoose() => EntityMoveStage switch
        {
            MoveStage.MonkeyClimbing => Interact + ClimbInteract,
            MoveStage.Walking => Interact + WalkInteract,
            MoveStage.Freezing => Interact + FreezeInteract,
            _ => Interact
        };

        [ContextMenu("Die")]
        public override void Death()
        {
            base.Death();
            SceneManager.Reload(true);
        }

        public void SetMoveStageByIndex(int index) => SetMoveStage((MoveStage)index);

        private void OnWalkMovement(Vector2 value)
        {
            _tMoveVector = new Vector3(value.x, 0, 0);
        }

        private void OnWalkCrouch()
        {
            if (AcelerationRate > 0.3f) _acelerationRate = 1;
            else SlidingCompleted = true;

            IsCrouching = !IsCrouching;
            if (!IsCrouching) SlidingCompleted = false;

            Set_Anim_IsCrouching(IsCrouching);
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

        protected override void EnableWalkJumpCouch(bool v)
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

        private void OnRun(bool isRunning)
        {
            IsRunning = isRunning;
        }

        protected override void EnableClimbing(bool v)
        {
            if (v)
            {
                MoveVector = Vector3.zero;
                _entityRigidbody.useGravity = false;
                InputMan.GameInputs.MovementChannel.Link(OnClimbMovement);

                Set_Anim_IsClimbing(true);
                return;
            }

            MoveVector = Vector3.zero;
            _entityRigidbody.useGravity = true;
            InputMan.GameInputs.MovementChannel.Unlink(OnClimbMovement);

            Set_Anim_IsClimbing(false);
        }

        private void OnClimbMovement(Vector2 value)
        {
            _tMoveVector = new Vector3(value.x, value.y, 0);
        }

        public override void LerpTo(Vector3 target, Action onCompleted = null)
        {
            float currentTime = 0;
            if (_currentLerp != null) StopCoroutine(_currentLerp);

            EnablePhysics(false);
            CanMove = false;

            _currentLerp = Routinef.LoopUntil(() =>
            {
                currentTime += Time.fixedDeltaTime;
                transform.position = Vector3.Lerp(transform.position, target, Time.fixedDeltaTime * 5);

            }, () => (target - transform.position).magnitude < 0.1f, Time.fixedDeltaTime, this, () =>
            {
                EnablePhysics(true);
                onCompleted?.Invoke();
                CanMove = true;
            });
        }

        public override void LerpTo(Vector3 target, Vector3 euler, Action onCompleted = null)
        {
            float currentTime = 0;
            if (_currentLerp != null) StopCoroutine(_currentLerp);

            SetEuler(euler);
            CanMove = false;

            EnablePhysics(false);
            _currentLerp = Routinef.LoopUntil(() =>
            {
                currentTime += Time.fixedDeltaTime;
                transform.position = Vector3.Lerp(transform.position, target, stairAffect.VerticalAffection.Evaluate(currentTime));

            }, () => (target - transform.position).magnitude < 0.1f, Time.fixedDeltaTime, this, () =>
            {
                EnablePhysics(true);
                onCompleted?.Invoke();
                CanMove = true;
            });
        }

        public void LerpTo(Vector3 target, Vector3 euler, float speed, Action onCompleted = null)
        {
            float currentTime = 0;
            if (_currentLerp != null) StopCoroutine(_currentLerp);

            SetEuler(euler);
            CanMove = false;

            EnablePhysics(false);
            _currentLerp = Routinef.LoopUntil(() =>
            {
                currentTime += Time.fixedDeltaTime;
                transform.position = Vector3.Lerp(transform.position, target, speed <= 0 ? stairAffect.VerticalAffection.Evaluate(currentTime) : speed);

            }, () => (target - transform.position).magnitude < 0.1f, Time.fixedDeltaTime, this, () =>
            {
                EnablePhysics(true);
                onCompleted?.Invoke();
                CanMove = true;
            });
        }

        public void LerpTo(Vector3 target, Vector3 euler, float speed, float threshold, Action onCompleted = null)
        {
            float currentTime = 0;
            if (_currentLerp != null) StopCoroutine(_currentLerp);

            CanMove = false;
            SetEuler(euler);

            EnablePhysics(false);
            _currentLerp = Routinef.LoopUntil(() =>
            {
                currentTime += Time.fixedDeltaTime;
                transform.position = Vector3.Lerp(transform.position, target, speed <= 0 ? stairAffect.VerticalAffection.Evaluate(currentTime) : speed);

            }, () => (target - transform.position).magnitude < threshold, Time.fixedDeltaTime, this, () =>
            {
                EnablePhysics(true);
                onCompleted?.Invoke();
                CanMove = true;
            });
        }

        public void LerpAxis(SnapAxis axis, float positionInAxis, float speed = 5, Vector3? euler = null)
        {
            if (_currentLerp != null) StopCoroutine(_currentLerp);

            CanMove = false;

            euler ??= transform.localEulerAngles;
            SetEuler(euler.Value);

            _currentLerp = Routinef.LoopUntil(() =>
            {
                Vector3 target = transform.position;

                switch (axis)
                {
                    case SnapAxis.X:
                        target.x = positionInAxis; break;

                    case SnapAxis.Y:
                        target.y = positionInAxis; break;

                    case SnapAxis.Z:
                        target.z = positionInAxis; break;
                }

                transform.position = Vector3.Lerp(transform.position, target, speed);
            }, () =>
            {
                bool completed = false;

                switch (axis)
                {
                    case SnapAxis.X:
                        completed = Mathf.Abs(positionInAxis - transform.position.x) < 0.05f;
                        break;

                    case SnapAxis.Y:
                        completed = Mathf.Abs(positionInAxis - transform.position.y) < 0.05f;
                        break;

                    case SnapAxis.Z:
                        Debug.Log($"{(positionInAxis - transform.position.z) < 0.05f} {positionInAxis} {transform.position.z}");
                        completed = Mathf.Abs(positionInAxis - transform.position.z) < 0.05f;
                        break;
                }

                return completed;
            }, Time.fixedDeltaTime, this, () => CanMove = true);
        }

        public void LinearLerpTo(Vector3 target, float speed, Action onComplete = null)
        {
            Vector3 sPos = transform.position;
            Vector3 norm = (target - transform.position).normalized;

            EnablePhysics(false);

            _currentLerp = Routinef.LoopWhile((t) =>
            {
                transform.position = sPos + (norm * speed) * t;

            }, () => (target - transform.position).magnitude > 1f, Time.fixedDeltaTime, this, () =>
            {
                transform.position = target;
                EnablePhysics(true);
                onComplete?.Invoke();
            });
        }

        public void LinearLerpTo(Vector3 target, float speed, Vector3 euler, Action onComplete = null)
        {
            Vector3 sPos = transform.position;
            Vector3 norm = (target - transform.position).normalized;

            EnablePhysics(false);
            SetEuler(euler);

            _currentLerp = Routinef.LoopWhile((t) =>
            {
                transform.position = sPos + (norm * speed) * t;

            }, () => (target - transform.position).magnitude > 1f, Time.fixedDeltaTime, this, () =>
            {
                transform.position = target;
                EnablePhysics(true);
                onComplete?.Invoke();
            });
        }

        public void LinearLerpTo(Vector3 target, float speed, Vector3 euler, bool resetEulerOnComplete, Action onComplete = null)
        {
            Vector3 sPos = transform.position;
            Vector3 norm = (target - transform.position).normalized;

            Vector3 startEuler = transform.eulerAngles;

            EnablePhysics(false);
            SetEuler(euler);

            _currentLerp = Routinef.LoopWhile((t) =>
            {
                transform.position = sPos + (norm * speed) * t;

            }, () => (target - transform.position).magnitude > 1f, Time.fixedDeltaTime, this, () =>
            {
                transform.position = target;
                EnablePhysics(true);
                if (resetEulerOnComplete) SetEuler(startEuler);
                onComplete?.Invoke();
            });
        }

        public void LerpArmTo(Vector3 target, Action onCompleted = null, Action<bool> cooldown = null)
        {
            float currentTime = 0;
            if (_currentLerp != null) StopCoroutine(_currentLerp);

            cooldown?.Invoke(false);
            EnablePhysics(false);
            _currentLerp = Routinef.LoopUntil(() =>
            {

                currentTime += Time.fixedDeltaTime;
                ArmPosition = Vector3.Lerp(ArmPosition, target, stairAffect.VerticalAffection.Evaluate(currentTime));

            }, () => (target - ArmPosition).magnitude < 0.1f, Time.fixedDeltaTime, this, () =>
            {
                onCompleted?.Invoke();
                cooldown?.Invoke(true);
            });
        }

        public void LerpArmTo(Vector3 target, Vector3 euler, Action onCompleted = null, Action<bool> cooldown = null)
        {
            float currentTime = 0;
            if (_currentLerp != null) StopCoroutine(_currentLerp);

            SetEuler(euler);

            cooldown?.Invoke(false);
            EnablePhysics(false);
            _currentLerp = Routinef.LoopUntil(() =>
            {

                currentTime += Time.fixedDeltaTime;
                ArmPosition = Vector3.Lerp(ArmPosition, target, stairAffect.VerticalAffection.Evaluate(currentTime));

            }, () => (target - ArmPosition).magnitude < 0.1f, Time.fixedDeltaTime, this, () =>
            {
                onCompleted?.Invoke();
                cooldown?.Invoke(true);
            });
        }

        public void JumpToStair(Vector3 start, Vector3 end)
        {
            base.JumpTo(start, end, stairAffect.VerticalAffection);
        }

        public void JumpToStairFromHere(Vector3 end)
        {
            base.JumpFromHere(end, stairAffect.VerticalAffection);
        }

        public void JumpToStair(Vector3 start, Vector3 end, Vector3 euler)
        {
            base.JumpTo(start, end, stairAffect.VerticalAffection);
            SetEuler(euler);
        }

        public void JumpToStairFromHere(Vector3 end, Vector3 euler)
        {
            base.JumpFromHere(end, stairAffect.VerticalAffection);
            SetEuler(euler);
        }

        public void JumpToStairFromArm(Vector3 end, Action onCompleted = null, Action<bool> cooldown = null)
        {
            float curTime = 0;

            Vector3 start = ArmPosition;

            bool isRight = (end - start).x < 0;

            Vector3 startEuler = _curEuler;
            Vector3 endEuler = new Vector3(_curEuler.x, _curEuler.y, isRight ? stairAffect.HorizontalRotation : -stairAffect.HorizontalRotation);

            if (isRight) Set_Anim_ClimbRight();
            else Set_Anim_ClimbLeft();

            cooldown?.Invoke(false);
            _currentLerp = Routinef.LoopUntil(() =>
            {
                float evaluated = stairAffect.VerticalAffection.Evaluate(curTime);
                float eulerEvaluated = stairAffect.HorizontalAffection.Evaluate(curTime);
                curTime += Time.fixedDeltaTime * stairAffect.LerpMultiplier;

                ArmPosition = (start * (1 - evaluated)) + (end * evaluated);
                transform.eulerAngles = (startEuler * (1 - eulerEvaluated)) + (endEuler * eulerEvaluated);

            }, () => curTime > 0.95f, Time.fixedDeltaTime, this, () =>
            {
                curTime = 1;
                ArmPosition = end;
                transform.eulerAngles = startEuler;
                cooldown?.Invoke(true);
                onCompleted?.Invoke();

            });
        }

        public void JumpToStairStart(Vector3 end, Vector3 euler)
        {
            SetEuler(euler);

            float curTime = 0;

            Vector3 start = ArmPosition;

            bool isRight = (end - start).x < 0;

            _currentLerp = Routinef.LoopUntil(() =>
            {
                float evaluated = stairAffect.VerticalAffection.Evaluate(curTime);
                float eulerEvaluated = stairAffect.HorizontalAffection.Evaluate(curTime);
                curTime += Time.fixedDeltaTime * stairAffect.LerpMultiplier;

                ArmPosition = (start * (1 - evaluated)) + (end * evaluated);

            }, () => curTime > 0.95f, Time.fixedDeltaTime, this, () =>
            {
                curTime = 1;
                ArmPosition = end;

            });
        }

        public void Jump()
        {
            if (Physics.OverlapBox(transform.position + _groundOffset, _groundSize / 2, Quaternion.identity, _groundMask.value).Length <= 0) return;

            if (OnJump.Invoke())
            {
                Set_Anim_Jump();
                _entityRigidbody.velocity = new Vector3(_entityRigidbody.velocity.x, configuration.JumpSpeed);
                Routinef.Invoke(IsGroundCheck, 0.01f, this);
            }
        }

        public void StairJump()
        {
            if (CurrentStair == null) return;
            CurrentStair.JumpInside();
        }

        private void IsGroundCheck()
        {
            Routinef.LoopWhile(() =>
            {
                IsGrounded = false;

            }, () => Physics.OverlapBox(transform.position + _groundOffset, _groundSize / 2, Quaternion.identity, _groundMask.value).Length <= 0, Time.fixedDeltaTime, this, () => IsGrounded = true);
        }

        public void TeleportToObj(Transform obj) => transform.position = obj.position;

        public void SetRotateAngle(int angle) => RightAngle = angle;

        public void FinishTopStairClimb()
        {
            CurrentStair.SetActive(false, true);
            CanMove = true;
        }

        public void CompleteDes() => SlidingCompleted = true;

        public void OnDash(bool value)
        {
            IsDashing = value;
            if (!IsDashing && !HasStopPerformingDash) HasStopPerformingDash = true;
        }

        protected override void Dash()
        {
            DashDirection = MoveVector.normalized;
            IsPerformingDash = true;
            HasStopPerformingDash = false;

            if ((_curEuler.y == RightAngle && DashDirection.x > 0) ||
                (_curEuler.y == RightAngle + 180 && DashDirection.x < 0))
            {
                Set_Anim_DashFront();
                return;
            }

            Set_Anim_DashBack();
        }

        public void DisablePerformingDash()
        {
            SetVelocity(Vector3.zero);
            IsPerformingDash = false;
        }

        public void KillEnemy()
        {
            if (CurrentKillSpace == null || IsAttacking) return;
            _entityAnimator.SetTrigger("StealthKill");
            CurrentKillSpace.StartedKill();

            Vector3 pos = CurrentKillSpace.KillPosition;
            LerpTo(new Vector3(pos.x, transform.position.y, pos.z));
            
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
        
        private void OnDrawGizmosSelected()
        {
            if (!_drawGizmos) return;
            Gizmosf.DrawCubeWithBorder(transform.position + _groundOffset, _groundSize, new Color(0, 1, 0, 0.1f), new Color(0, 1, 0, 1));
        }

        private void OnDrawGizmos()
        {
            if (!_drawGizmos) return;

            Gizmosf.DrawSphere(UpStair, 0.1f, Color.green);
            Gizmosf.DrawSphere(DownStair, 0.1f, Color.green);
            Gizmosf.DrawSphere(LeftStair, 0.1f, Color.green);
            Gizmosf.DrawSphere(RightStair, 0.1f, Color.green);
        }
    }
}
