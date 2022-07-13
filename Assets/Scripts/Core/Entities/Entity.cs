//Made by Galactspace Studios

using System;
using UnityEngine;
using Core.Controllers;
using Scriptable.Entities;
using System.Threading.Tasks;
using Core.Interactables;

namespace Core.Entities
{
    public enum MoveStage {
        Freezing,
        Walking,
        MonkeyClimbing,
        Climbing,
        Runner
    }

    public class Entity : MonoBehaviour
    {
        protected Animator _entityAnimator;
        protected Rigidbody _entityRigidbody;

        public Rigidbody ERigidbody => _entityRigidbody;

        protected Vector3 _freezePosition;

        private Vector3 _moveVector;
        public Vector3 MoveVector
        {
            get => _moveVector;
            set => _moveVector = value;
        }

        //protected Vector3 _targetEuler;
        //public Vector3 TargetEuler => _targetEuler;

        protected Vector3 _lastLerp;
        public Vector3 LastLerp => _lastLerp;

        protected float _acelerationRate;
        public float AcelerationRate { get => _acelerationRate; protected set => _acelerationRate = value; }

        protected Coroutine _currentLerp;
        protected Coroutine _eulerLerpCoroutine;

        public virtual float RightAngle
        {
            get => _rightAngle; 
            set
            {
                _rightAngle = value;
                if ((value % 90) == 0)
                    _entityRigidbody.constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY;
                if (value == 0 || (value % 180) == 0)
                    _entityRigidbody.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY;
            }
        }

        [HideInInspector] public Stair CurrentStair;

        [HideInInspector] public MoveStage EntityMoveStage = MoveStage.Walking;
        public bool IsFreezingMode => EntityMoveStage == MoveStage.Freezing;
        public bool IsWalkingMode => EntityMoveStage == MoveStage.Walking;
        public bool IsClimbingMode => EntityMoveStage == MoveStage.MonkeyClimbing;

        public virtual int MaxLife => configuration.MaxLife;
        public virtual int MaxStamina => configuration.MaxStamina;

        public virtual bool IsAlive { get; set; } = true;
        public virtual bool IsWalking { get; set; }
        public virtual bool IsCrouching { get; set; }
        public virtual bool IsDashing { get; set; }
        public virtual bool IsGrounded { get; set; } = true;
        public virtual bool CanMove { get; set; } = true;
        public virtual bool IsRunning { get; set; }
        public virtual bool SlidingCompleted { get; set; }
        public virtual bool IsAttacking { get; set; }

        public virtual bool IsPerformingDash { get; set; }
        public virtual bool HasStopPerformingDash { get; set; }

        public virtual float CurrentLife { get; set; }
        public virtual float CurrentStamina { get; set; }

        public Vector3 DashDirection { get; set; }

        protected Action OnDeath;
        protected Action<float> OnDamage;
        protected Action<float> OnStaminaUse;

        [SerializeField] protected bool _drawGizmos;

        [Space]
        [SerializeField] protected EntityConfigurationSo configuration;

        [Space]
        [SerializeField] private float _rightAngle;

        protected Vector3 _curEuler;

        protected virtual bool IsLifeZero => CurrentLife <= 0;
        protected virtual bool IsStaminaZero => CurrentStamina == 0;

        protected virtual float GetDamagedLife(float damage) => Mathf.Clamp(CurrentLife - damage, 0, MaxLife);
        protected virtual float GetUsedStamina(float used) => Mathf.Clamp(CurrentStamina - used, 0, MaxStamina);

        public virtual void Set_Anim_IsWalking(bool value) => _entityAnimator.SetBool("IsWalking", value);

        public float WalkSpeed => configuration.WalkSpeed;
        public float RunSpeed => configuration.RunSpeed;
        public float CrouchSpeed => configuration.CrouchSpeed;
        public float JumpSpeed => configuration.JumpSpeed;

        protected virtual void EnablePhysics(bool value)
        {
            _entityRigidbody.useGravity = value;
        }

        protected void SetVelocity(Vector3 value)
        {
            _entityRigidbody.velocity = Vector3.zero;
            _entityRigidbody.velocity = value;
        }

        protected virtual void Awake()
        {
            IsAlive = true;

            CurrentLife = MaxLife;
            CurrentStamina = MaxStamina;

            _entityAnimator = GetComponent<Animator>();
            _entityRigidbody = GetComponent<Rigidbody>();

            RightAngle = _rightAngle;

            _freezePosition = transform.position;
        }

        protected virtual void FixedUpdate()
        {
            Collider[] cols = Physics.OverlapSphere(transform.position, 0.2f, LayerMask.GetMask("Movable"));
            if (cols.Length > 0)
            {
                ERigidbody.position += cols[0].GetComponentInParent<Movable>().Delta;
            }
            Movement()?.Invoke();
        }

        public virtual void SetEuler(Vector3 euler, float lerpSpeed = 10, float threshold = 0.1f)
        {
            _curEuler = euler;

            if (_eulerLerpCoroutine != null) StopCoroutine(_eulerLerpCoroutine);

            _eulerLerpCoroutine = Routinef.LoopWhile(() => transform.eulerAngles = Vector3.Lerp(transform.eulerAngles, euler, Time.fixedDeltaTime * lerpSpeed),
                () => (transform.eulerAngles - euler).magnitude > threshold, Time.fixedDeltaTime, this, () => transform.eulerAngles = euler);
        }

        public virtual void Damage(float damage)
        {
            if (!IsAlive) return;

            CurrentLife = GetDamagedLife(damage);
            OnDamage?.Invoke(damage);

            if (IsLifeZero) Death();
        }

        public virtual void UseStamina(float used)
        {
            if (!IsAlive) return;

            CurrentStamina = GetUsedStamina(used);
            OnStaminaUse?.Invoke(used);
        }

        public virtual void Death()
        {
            if (!IsAlive) return;

            IsAlive = false;
            OnDeath?.Invoke();
        }

        protected virtual Action Movement() => EntityMoveStage switch
        {
            MoveStage.Freezing => FreezeMove,
            //MoveStage.MonkeyClimbing => ClimbMove,
            MoveStage.Walking => WalkMove,
            MoveStage.Runner => WalkMove,
            MoveStage.Climbing => ClimbMove,
            _ => throw new NotImplementedException()
        };

        protected virtual void WalkMove()
        {
            if (!IsAlive || !CanMove)
            {
                SetVelocity(Vector3.zero);
                return;
            }

            float tSpeed;
            float walkSpeed = (configuration.WalkSpeed * (1 - AcelerationRate)) + (configuration.RunSpeed * AcelerationRate);
            float airSpeed = walkSpeed * configuration.AirSpeedModifier;
            float crouchSpeed = (configuration.CrouchSpeed * (1 - AcelerationRate)) + (configuration.RunSpeed * AcelerationRate);
            float slideSpeed = walkSpeed * configuration.SlideSpeedModifier;

            if (IsCrouching && !SlidingCompleted) tSpeed = slideSpeed;
            else if (IsCrouching) tSpeed = crouchSpeed;
            else if (!IsGrounded) tSpeed = airSpeed;
            else tSpeed = walkSpeed;

            if (MoveVector.magnitude <= 0.01f)
            {
                StopMovement();
                return;
            }

            if (HasStopPerformingDash && IsDashing && !IsPerformingDash)
            {
                Dash();
                return;
            }

            if (IsPerformingDash)
            {
                Vector3 sv = DashDirection * configuration.DashSpeed;
                Vector3 nv = transform.forward.Abs() * sv.x;

                Vector3 fv = nv;
                if (fv.x == 0) fv.x = _entityRigidbody.velocity.x;
                if (fv.z == 0) fv.z = _entityRigidbody.velocity.z;
                fv.y = _entityRigidbody.velocity.y;

                SetVelocity(fv);

                return;
            }

            SetTargetEuler();
            Set_Anim_IsWalking(true);
            IsWalking = true;

            Vector3 speededVec = MoveVector * tSpeed;
            Vector3 newVelocity = transform.forward.Abs() * speededVec.x;

            Vector3 fVel = newVelocity;
            if (fVel.x == 0) fVel.x = _entityRigidbody.velocity.x;
            if (fVel.z == 0) fVel.z = _entityRigidbody.velocity.z;
            fVel.y = _entityRigidbody.velocity.y;

            SetVelocity(fVel);
        }

        protected virtual void ClimbMove()
        {
            if (!IsAlive || !CanMove)
            {
                SetVelocity(Vector3.zero);
                return;
            }

            if (MoveVector.magnitude <= 0.01f)
            {
                StopMovement(Vector3.zero);
                return;
            }

            IsWalking = true;
            Set_Anim_IsWalking(true);

            Vector3 fVel = MoveVector * configuration.ClimbSpeed;
            if (fVel.z == 0) fVel.z = _entityRigidbody.velocity.z;

            SetVelocity(fVel);
        }

        protected virtual void FreezeMove()
        {
            _entityRigidbody.position = Vector3.Lerp(_entityRigidbody.position, _freezePosition, Time.fixedDeltaTime * 3);
        }

        //protected virtual void ClimbMove()
        //{
        //    if (!IsAlive || !CanMove || CurrentStair == null) return;

        //    if (MathF.Abs(MoveVector.y) < 0.5f)
        //    {
        //        StopMovement();
        //        return;
        //    }
        //}

        //protected virtual void OnClimbUp()
        //{
        //    if (!IsAlive || !CanMove || CurrentStair == null) return;
        //    CurrentStair.JumpTo(StairController.Direction.Up);
        //}

        //protected virtual void OnClimbDown()
        //{
        //    if (!IsAlive || !CanMove || CurrentStair == null) return;
        //    CurrentStair.JumpTo(StairController.Direction.Down);
        //}

        //protected virtual void OnClimbLeft()
        //{
        //    if (!IsAlive || !CanMove || CurrentStair == null) return;
        //    CurrentStair.JumpTo(StairController.Direction.Left);
        //}

        //protected virtual void OnClimbRight()
        //{
        //    if (!IsAlive || !CanMove || CurrentStair == null) return;
        //    CurrentStair.JumpTo(StairController.Direction.Right);
        //}

        protected virtual void StopMovement(Vector3? value = null)
        {
            value ??= new Vector3(0, EntityMoveStage == MoveStage.MonkeyClimbing ? 0 : _entityRigidbody.velocity.y, 0);

            SetVelocity(value.Value);
            Set_Anim_IsWalking(false);
            IsWalking = false;
        }

        protected virtual void SetTargetEuler()
        {
            float y = MoveVector.x < 0 || (MoveVector.x == 0 && _curEuler.y == (RightAngle + 180)) ? (RightAngle + 180) : RightAngle;
            SetEuler(new Vector3(0, y, 0), 40);
        }

        public void SetMoveStage(MoveStage value)
        {
            if (EntityMoveStage == value) return;
            DisableCurrent(EntityMoveStage);
            EntityMoveStage = value;

            switch (value)
            {
                case MoveStage.Freezing:
                    _freezePosition = transform.position;
                    break;

                case MoveStage.Walking:
                    EnablePhysics(true);
                    EnableWalk(true);
                    break;

                case MoveStage.Runner:
                    EnablePhysics(true);
                    EnableWalkJumpCouch(true);
                    break;

                case MoveStage.MonkeyClimbing:
                    EnableClimbing(true);
                    break;

                case MoveStage.Climbing:
                    EnableClimbing(true);
                    EnablePhysics(false);
                    break;

                default:
                    throw new NotImplementedException();
            }

            void DisableCurrent(MoveStage v)
            {
                switch (v)
                {
                    case MoveStage.Freezing:
                        
                        break;

                    case MoveStage.Walking:
                        EnableWalk(false);
                        break;

                    case MoveStage.MonkeyClimbing:
                        EnableClimbing(false);
                        break;

                    case MoveStage.Runner:
                        EnableWalkJumpCouch(false);
                        break;

                    case MoveStage.Climbing:
                        EnableClimbing(false);
                        break;

                    default:
                        throw new NotImplementedException();
                }
            }
        }

        public void SwitchMoveStage(MoveStage a, MoveStage b)
        {
            if (EntityMoveStage == a)
            {
                SetMoveStage(b);
            }
            else if (EntityMoveStage == b)
            {
                SetMoveStage(a);
            }
        }

        public virtual void LerpTo(Vector3 target, Action onCompleted = null)
        {
            float currentTime = 0;
            if (_currentLerp != null) StopCoroutine(_currentLerp);

            EnablePhysics(false);

            _currentLerp = Routinef.LoopUntil(() =>
            {

                currentTime += Time.fixedDeltaTime;
                transform.position = Vector3.Lerp(transform.position, target, Time.fixedDeltaTime * 10);

            }, () => (target - transform.position).magnitude < 0.1f, Time.fixedDeltaTime, this, () =>
            {
                onCompleted?.Invoke();
            });
        }

        public virtual void LerpToMove(Vector3 target, Action onCompleted = null)
        {
            float currentTime = 0;
            if (_currentLerp != null) StopCoroutine(_currentLerp);

            EnablePhysics(false);
            CanMove = false;

            _currentLerp = Routinef.LoopUntil(() =>
            {

                currentTime += Time.fixedDeltaTime;
                transform.position = Vector3.Lerp(transform.position, target, Time.fixedDeltaTime * 10);

            }, () => (target - transform.position).magnitude < 0.1f, Time.fixedDeltaTime, this, () =>
            {
                onCompleted?.Invoke();
                CanMove = true;
            });
        }

        public virtual void LerpTo(Vector3 target, Vector3 euler, Action onCompleted = null)
        {
            float currentTime = 0;
            if (_currentLerp != null) StopCoroutine(_currentLerp);

            CanMove = false;
            SetEuler(Vector3.zero);

            EnablePhysics(false);
            Taskf.LoopUntil(() =>
            {

                currentTime += Time.fixedDeltaTime;
                transform.position = Vector3.Lerp(transform.position, target, Time.fixedDeltaTime);

            }, () => (target - transform.position).magnitude < 0.1f, Time.fixedDeltaTime, this, out _currentLerp, () =>
            {
                onCompleted?.Invoke();
                CanMove = true;
            });
        }

        protected virtual void EnableWalk(bool v)
        {
            if (v)
            {
                MoveVector = Vector3.zero;
                return;
            }

            MoveVector = Vector3.zero;
        }

        protected virtual void EnableWalkJumpCouch(bool v)
        {
            if (v)
            {
                MoveVector = Vector3.zero;
                return;
            }

            MoveVector = Vector3.zero;
        }

        protected virtual void EnableClimbing(bool v)
        {
            if (v)
            {
                MoveVector = Vector3.zero;
                _entityRigidbody.useGravity = false;
                Debug.Log("Tst");
                return;
            }

            MoveVector = Vector3.zero;
            _entityRigidbody.useGravity = true;
        }

        public virtual void JumpTo(Vector3 start, Vector3 end, AnimationCurve curve)
        {
            float curTime = 0;

            Routinef.LoopUntil(() =>
            {
                float evaluated = curve.Evaluate(curTime);
                curTime += Time.fixedDeltaTime;

                _entityRigidbody.position = (start * (1 - evaluated)) + (end * evaluated);


            }, () => curTime > 0.95f, Time.fixedDeltaTime, this, () =>
            {
                curTime = 1;
                _entityRigidbody.position = end;

            });
        }

        public virtual void JumpFromHere(Vector3 end, AnimationCurve curve)
        {
            JumpTo(transform.position, end, curve);
        }
    
        protected virtual void Dash() { }
    }
}