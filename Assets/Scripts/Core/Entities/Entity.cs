//Made by Galactspace Studios

using System;
using UnityEngine;
using Scriptable.Item;
using Core.Controllers;
using Scriptable.Entities;
using Core.Interactables;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Core.Entities
{
    public enum MoveStage
    {
        None,
        Walk,
        Climb,
        Runner
    }

    public class Entity : MonoBehaviour
    {
        public Inventory Inventory = new();

        [HideInInspector] public Collider ECollider;
        [HideInInspector] public Animator EAnimator;
        [HideInInspector] public Rigidbody ERigidbody;

        [HideInInspector] public List<Vector3> _forces = new();

        [HideInInspector] public Vector3 Euler;
        [HideInInspector] public Vector3 LastLerp;
        [HideInInspector] public Vector3 MoveVector;

        [HideInInspector] public Coroutine _cEulerLerp;
        [HideInInspector] public Coroutine _cPositionLerp;

        [HideInInspector] public float AcelerationRate;
        public SnapAxis MoveAxis;
        public bool MirrorAxis;

        [HideInInspector] public Stair CStair;

        public MoveStage EMoveStage;

        public bool IsStoped => EMoveStage == MoveStage.None;
        public bool IsWalking => EMoveStage == MoveStage.Walk;
        public bool IsClimbing => EMoveStage == MoveStage.Climb;

        [HideInInspector] public float CurrentLife;
        public int MaxLife => _config.MaxLife;

        public float RunSpeed => _config.RunSpeed;
        public float JumpSpeed => _config.JumpSpeed;
        public float StairJumpSpeed => _config.StaitJumpSpeed;

        [HideInInspector] public bool IsMoving;
        [HideInInspector] public bool IsDashing;
        [HideInInspector] public bool IsRunning;
        [HideInInspector] public bool IsCrouching;
        [HideInInspector] public bool IsAttacking; 
        [HideInInspector] public bool StairImpulsing;
        [HideInInspector] public bool IsAlive = true;
        [HideInInspector] public bool CanMove = true;
        [HideInInspector] public bool SlidingCompleted;
        [HideInInspector] public bool IsGrounded = true;

        [HideInInspector] public bool StopedDash;
        [HideInInspector] public bool PerformingDash;
        [HideInInspector] public Vector3 DashDirection;

        [HideInInspector] public bool CanStairUp;
        [HideInInspector] public bool CanStairDown;
        [HideInInspector] public bool CanStairLeft;
        [HideInInspector] public bool CanStairRight;

        public float RightAngle => MoveAxis switch
        {
            SnapAxis.X => MirrorAxis ? 270 : 90,
            SnapAxis.Z => MirrorAxis ? 180 : 0,
            _ => throw new NotImplementedException()
        };

        protected float ClampedLife(float damage) => Mathf.Clamp(CurrentLife - damage, 0, MaxLife);

        [Space]
        [SerializeField] protected EntityConfigurationSo _config;

        public void SetVelocity(Vector3 value)
        {
            ERigidbody.velocity = Vector3.zero;
            ERigidbody.velocity = GetForces() + value;
        }
        public void EnablePhysics(bool? gravity, bool? collision)
        {
            if (gravity.HasValue) ERigidbody.useGravity = gravity.Value;
            if (collision.HasValue) ECollider.enabled = collision.Value;
        }

        public Vector3 GetForces()
        {
            if (IsGrounded)
            {
                _forces.Clear();
                return Vector3.zero;
            }

            Vector3 f = default;
            for (int i = 0; i < _forces.Count; i++)
            {
                f += _forces[i];
                _forces[i] -= _forces[i] * Time.deltaTime * 2;
                if (_forces[i].magnitude < 0.1f)
                {
                    _forces.Remove(_forces[i]);
                    i--;
                }
            }

            return f;
        }

        protected virtual void Awake()
        {
            IsAlive = true;
            CurrentLife = MaxLife;

            ECollider = GetComponent<Collider>();
            EAnimator = GetComponent<Animator>();
            ERigidbody = GetComponent<Rigidbody>();

            SetMoveAxis(MoveAxis, MirrorAxis);
            SetMoveStage(EMoveStage, true);
        }
        protected void FixedUpdate()
        {
            MovableOffset();
            Action act = EMoveStage switch
            {
                MoveStage.None => delegate { },
                MoveStage.Walk => WalkMovement,
                MoveStage.Climb => ClimbMovement,
                MoveStage.Runner => RunnerMovement,
                _ => throw new NotImplementedException()
            };
            act();
        }

        public void SwitchMoveStage(MoveStage a, MoveStage b)
        {
            if (EMoveStage != a && EMoveStage != b) return;
            
            if (EMoveStage == a)
            {
                EMoveStage = b;
                return;
            }

            EMoveStage = a;
        }
        public void SetMoveStage(MoveStage value, bool over = false)
        {
            if (EMoveStage == value && !over) return;
            DisableCurrent(EMoveStage);
            EMoveStage = value;

            switch (value)
            {
                case MoveStage.None:
                    break;

                case MoveStage.Walk:
                    EnablePhysics(true, true);
                    EnableWalk(true);
                    break;

                case MoveStage.Runner:
                    EnablePhysics(true, true);
                    EnableRunner(true);
                    break;

                case MoveStage.Climb:
                    EnablePhysics(false, false);
                    EnableClimbing(true);
                    break;

                default:
                    throw new NotImplementedException();
            }

            void DisableCurrent(MoveStage v)
            {
                switch (v)
                {
                    case MoveStage.None:
                        break;

                    case MoveStage.Walk:
                        EnableWalk(false);
                        break;

                    case MoveStage.Runner:
                        EnableRunner(false);
                        break;

                    case MoveStage.Climb:
                        EnableClimbing(false);
                        break;

                    default:
                        throw new NotImplementedException();
                }
            }
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
        protected virtual void EnableRunner(bool v)
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
                ERigidbody.useGravity = false;
                return;
            }

            MoveVector = Vector3.zero;
            ERigidbody.useGravity = true;
        }

        protected virtual void WalkMovement()
        {
            if (StairImpulsing) return;

            if (!IsAlive || !CanMove)
            {
                SetVelocity(Vector3.zero);
                return;
            }

            if (MoveVector.magnitude <= 0.01f)
            {
                StopMovement();
                return;
            }

            if (StopedDash && IsDashing && !PerformingDash)
            {
                Dash();
                return;
            }

            if (PerformingDash)
            {
                Vector3 velocity = DashDirection * _config.DashSpeed;
                Vector3 nv = transform.forward.Abs() * velocity.x;

                Vector3 fv = nv;
                if (fv.x == 0) fv.x = ERigidbody.velocity.x;
                if (fv.z == 0) fv.z = ERigidbody.velocity.z;
                fv.y = ERigidbody.velocity.y;

                SetVelocity(fv);

                return;
            }

            float speed;
            float walkSpeed = (_config.WalkSpeed * (1 - AcelerationRate)) + (_config.RunSpeed * AcelerationRate);

            if (IsCrouching && !SlidingCompleted) speed = walkSpeed  * _config.SlideSpeedModifier;
            else if (IsCrouching) speed = (_config.CrouchSpeed * (1 - AcelerationRate)) + (_config.RunSpeed * AcelerationRate);
            else if (!IsGrounded) speed = walkSpeed * _config.AirSpeedModifier;
            else speed = walkSpeed;

            SetEuler();

            IsMoving = true;
            EAnimator.SetBool("IsWalking", true);

            float x = (speed * MoveVector).x;
            if (MirrorAxis) x *= -1;

            Vector3 vel = x * transform.forward.Abs();
            
            if (vel.x == 0) vel.x = ERigidbody.velocity.x;
            if (vel.z == 0) vel.z = ERigidbody.velocity.z;
            vel.y = ERigidbody.velocity.y;

            SetVelocity(vel);
        }
        protected virtual void ClimbMovement()
        {
            if (!IsAlive || !CanMove)
            {
                SetVelocity(Vector3.zero);
                return;
            }

            Vector3 m = MoveVector;

            if (!CanStairUp && m.y > 0)
                m.y = 0;
            if (!CanStairDown && m.y < 0)
                m.y = 0;

            if (!CanStairLeft && m.x < 0)
                m.x = 0;
            if (!CanStairRight && m.x > 0)
                m.x = 0;

            if (m.magnitude <= 0.01f)
            {
                StopMovement(Vector3.zero);
                return;
            }

            IsMoving = true;
            EAnimator.SetBool("IsWalking", true);

            Vector3 vel = m * _config.ClimbSpeed;
            if (MirrorAxis) vel.x *= -1;

            switch (MoveAxis)
            {
                case SnapAxis.Z:
                    float x = vel.x;
                    vel.x = vel.z;
                    vel.z = x;
                    break;

                default: break;
            }

            if (vel.z == 0) vel.z = ERigidbody.velocity.z;

            SetVelocity(vel);
        }
        protected virtual void RunnerMovement()
        {
            if (!IsAlive || !CanMove)
            {
                SetVelocity(Vector3.zero);
                return;
            }

            if (MoveVector.magnitude <= 0.01f)
            {
                StopMovement();
                return;
            }

            if (StopedDash && IsDashing && !PerformingDash)
            {
                Dash();
                return;
            }

            if (PerformingDash)
            {
                Vector3 velocity = DashDirection * _config.DashSpeed;
                Vector3 nv = transform.forward.Abs() * velocity.x;

                Vector3 fv = nv;
                if (fv.x == 0) fv.x = ERigidbody.velocity.x;
                if (fv.z == 0) fv.z = ERigidbody.velocity.z;
                fv.y = ERigidbody.velocity.y;

                SetVelocity(fv);

                return;
            }

            float speed;
            float walkSpeed = (_config.WalkSpeed * (1 - AcelerationRate)) + (_config.RunSpeed * AcelerationRate);

            if (IsCrouching && !SlidingCompleted) speed = walkSpeed * _config.SlideSpeedModifier;
            else if (IsCrouching) speed = (_config.CrouchSpeed * (1 - AcelerationRate)) + (_config.RunSpeed * AcelerationRate);
            else if (!IsGrounded) speed = walkSpeed * _config.AirSpeedModifier;
            else speed = walkSpeed;

            SetEuler();

            IsMoving = true;
            EAnimator.SetBool("IsWalking", true);

            Vector3 vel = (speed * MoveVector).x * transform.forward.Abs();
            if (vel.x == 0) vel.x = ERigidbody.velocity.x;
            if (vel.z == 0) vel.z = ERigidbody.velocity.z;
            vel.y = ERigidbody.velocity.y;

            SetVelocity(vel);
        }

        protected virtual void Dash() { }

        private void MovableOffset()
        {
            Collider[] cols = Physics
                .OverlapSphere(transform.position, 0.2f, LayerMask.GetMask("Movable"));

            if (cols.Length == 0) return;

            Movable mov = cols[0].GetComponentInParent<Movable>();
            if (mov != null) ERigidbody.position += mov.Delta;
        }
        protected virtual void StopMovement(Vector3? value = null)
        {
            if (StairImpulsing) return;

            value ??= new Vector3(0, ERigidbody.velocity.y, 0);

            IsMoving = false;
            SetVelocity(value.Value);
            EAnimator.SetBool("IsWalking", false);
        }

        public virtual void Damage(float damage)
        {
            if (!IsAlive) return;

            CurrentLife = ClampedLife(damage);
            if (CurrentLife <= 0) Die();
        }

        public void Die()
        {
            IsAlive = false;
            CanMove = false;

            OnDeath();
        }
        protected virtual void OnDeath() { }
        
        public void SetMoveAxis(SnapAxis axis, bool mirror)
        {
            MoveAxis = axis;
            MirrorAxis = mirror;
            
            switch (MoveAxis)
            {
                case SnapAxis.X:
                    ERigidbody.constraints = RigidbodyConstraints.FreezePositionZ   | RigidbodyConstraints.FreezeRotationX
                                                                                    | RigidbodyConstraints.FreezeRotationY
                                                                                    | RigidbodyConstraints.FreezeRotationZ;
                    break;

                case SnapAxis.Z:
                    ERigidbody.constraints = RigidbodyConstraints.FreezePositionX   | RigidbodyConstraints.FreezeRotationX
                                                                                    | RigidbodyConstraints.FreezeRotationY
                                                                                    | RigidbodyConstraints.FreezeRotationZ;
                    break;

                default:
                    throw new NotImplementedException();
            }
        }
        public void TeleportToObj(Transform obj) => transform.position = obj.position;
        public void SetEuler(Vector3? euler = null, float speed = 10, float threshold = 0.1f)
        {
            if (!euler.HasValue)
            {
                float y = MoveVector.x < 0 || (MoveVector.x == 0 && Euler.y == Mathf.Repeat(RightAngle + 180, 359)) ? Mathf.Repeat(RightAngle + 180, 359) : RightAngle;
                SetEuler(new Vector3(0, y, 0));
                return;
            }

            Euler = euler.Value;

            if (_cEulerLerp != null) StopCoroutine(_cEulerLerp);
            _cEulerLerp = Routinef.LoopWhile(() =>
            {
                transform.eulerAngles = Vector3.Lerp(transform.eulerAngles, euler.Value, Time.fixedDeltaTime * speed);
            }, () => (transform.eulerAngles - euler.Value).magnitude > threshold, Time.fixedDeltaTime, this);
        }
        
        public void CompleteSllide() => SlidingCompleted = true;

        public void Lerp(Vector3 target, Vector3? euler = null, float speed = 10,
            float threshold = 0.1f, bool stopMove = false, bool enablePhysics = false, bool resetEuler = false,
            Action onCompleted = null)
        {
            Vector3? startEuler =
                resetEuler ? transform.eulerAngles : null;

            if (euler.HasValue) SetEuler(euler);
            if (_cPositionLerp != null) StopCoroutine(_cPositionLerp);

            if (stopMove) CanMove = false;
            if (enablePhysics) EnablePhysics(false, false);

            _cPositionLerp = Routinef.LoopUntil(() =>
            {
                transform.position = Vector3.Lerp(transform.position, target, Time.fixedDeltaTime * speed);
            }, () => (target - transform.position).magnitude < threshold, Time.fixedDeltaTime, this, () =>
            {
                transform.position = target;

                onCompleted?.Invoke();
                if (stopMove) CanMove = true;
                if (enablePhysics) EnablePhysics(true, true);
                if (startEuler.HasValue) SetEuler(startEuler.Value);
            });
        }

        public void LinearLerp(Vector3 target, Vector3? euler = null, float speed = 10,
            float threshold = 1, bool stopMove = false, bool enablePhysics = false, bool resetEuler = false,
            Action onCompleted = null)
        {
            Vector3 start = transform.position;
            Vector3 direction = (target - transform.position).normalized;

            Vector3? startEuler =
                resetEuler ? transform.eulerAngles : null;

            if (euler.HasValue) SetEuler(euler);
            if (_cPositionLerp != null) StopCoroutine(_cPositionLerp);

            if (stopMove) CanMove = false;
            if (enablePhysics) EnablePhysics(false, false);

            _cPositionLerp = Routinef.LoopWhile((t) =>
            {
                transform.position = start + (direction * speed) * t;
            }, () => (target - transform.position).magnitude > threshold, Time.fixedDeltaTime, this, () =>
            {
                transform.position = target;

                onCompleted?.Invoke();
                if (stopMove) CanMove = true;
                if (enablePhysics) EnablePhysics(true, true);
                if (startEuler.HasValue) SetEuler(startEuler.Value);
            });
        }

        public void LerpAxis(SnapAxis axis, float worldPos, Vector3? euler = null, float speed = 10,
            float threshold = 0.1f, bool stopMove = false, bool enablePhysics = false, bool resetEuler = false,
            Action onCompleted = null)
        {
            Vector3? startEuler =
                resetEuler ? transform.eulerAngles : null;

            if (euler.HasValue) SetEuler(euler.Value);
            if (_cPositionLerp != null) StopCoroutine(_cPositionLerp);

            if (stopMove) CanMove = false;
            if (enablePhysics) EnablePhysics(false, false);

            _cPositionLerp = Routinef.LoopUntil(() =>
            {
                Vector3 target = transform.position;

                switch (axis)
                {
                    case SnapAxis.X:
                        target.x = worldPos; break;

                    case SnapAxis.Z:
                        target.z = worldPos; break;

                    default:
                        throw new NotImplementedException();
                }

                transform.position = Vector3.Lerp(transform.position, target, Time.fixedDeltaTime * speed);
            }, () => axis switch
            {
                SnapAxis.X => Mathf.Abs(worldPos - transform.position.x) < threshold,
                SnapAxis.Z => Mathf.Abs(worldPos - transform.position.z) < threshold,
                _ => throw new NotImplementedException(),
            }, Time.fixedDeltaTime, this, () =>
            {
                Debug.Log("Completed");
                onCompleted?.Invoke();
                if (stopMove) CanMove = true;
                if (enablePhysics) EnablePhysics(true, true);
                if (startEuler.HasValue) SetEuler(startEuler.Value);
            });
        }

        public void LinearLerpAxis(SnapAxis axis, float worldPos, Vector3? euler = null, float speed = 10,
            float threshold = 0.05f, bool stopMove = false, bool enablePhysics = false, bool resetEuler = false,
            Action onCompleted = null)
        {
            Vector3 target = axis switch
            {
                SnapAxis.X => new Vector3(worldPos, transform.position.y, transform.position.z),
                SnapAxis.Z => new Vector3(transform.position.x, transform.position.y, worldPos),
                _ => throw new NotImplementedException()
            };

            Vector3 start = transform.position;
            Vector3 direction = (target - transform.position).normalized;

            Vector3? startEuler =
                resetEuler ? transform.eulerAngles : null;

            if (euler.HasValue) SetEuler(euler.Value);
            if (_cPositionLerp != null) StopCoroutine(_cPositionLerp);

            if (stopMove) CanMove = false;
            if (enablePhysics) EnablePhysics(false, false);

            _cPositionLerp = Routinef.LoopWhile((t) =>
            {
                target = transform.position;

                switch (axis)
                {
                    case SnapAxis.X:
                        target.x = worldPos; break;

                    case SnapAxis.Z:
                        target.z = worldPos; break;

                    default:
                        throw new NotImplementedException();
                }

                ERigidbody.position = start + (direction * speed) * t;
            }, () => {
                bool completed = false;

                switch (axis)
                {
                    case SnapAxis.X:
                        completed = Mathf.Abs(worldPos - transform.position.x) > threshold;
                        break;

                    case SnapAxis.Z:
                        completed = Mathf.Abs(worldPos - transform.position.z) > threshold;
                        break;

                    default:
                        throw new NotImplementedException();
                }

                return completed;
            }, Time.fixedDeltaTime, this, () =>
            {
                transform.position = target;

                onCompleted?.Invoke();
                if (stopMove) CanMove = true;
                if (enablePhysics) EnablePhysics(true, true);
                if (startEuler.HasValue) SetEuler(startEuler.Value);
            });
        }
    }
}