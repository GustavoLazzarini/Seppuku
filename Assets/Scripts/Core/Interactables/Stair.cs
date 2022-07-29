//Created by Galactspace

using System;
using System.Linq;
using UnityEngine;
using Core.Entities;
using Core.Controllers;
using Scriptable.Generic;
using System.Collections.Generic;
using UnityEngine.Events;

namespace Core.Interactables
{
    public class Stair : Interactable
    {
        public bool Active => _protagonist.CurrentStair == this;
        public bool EnterGrounded { get => _groundEnter; set => _groundEnter = value; }
        public bool IsUp => (_protagonist.transform.position - _up.Position).magnitude < (_protagonist.transform.position - _down.Position).magnitude;

        private float _enterTime;
        private Vector2 _moveVector;

        private CubeCollider[] _areaColliders;

        [Space]
        [SerializeField] private bool _groundEnter;
        
        [Space]
        [SerializeField] private SnapAxis _axis;
        [SerializeField] private bool _flipEuler;

        [Space]
        [SerializeField] private GameObject _area;
        [SerializeField] private Bubble _up, _down, _insideBubble;

        [Space]
        [SerializeField] private bool _disableUp;
        [SerializeField] private bool _disableDown;
        [SerializeField] private bool _disableLeft;
        [SerializeField] private bool _disableRight;

        [Space]
        [SerializeField] private UnityEvent _onEnter;
        [Space]
        [SerializeField] private UnityEvent _onExit;

        protected override void OnEnable()
        {
            base.OnEnable();
            _colliders.RegisterOnEnter(TickColliders);
            _colliders.RegisterOnExit(TickColliders);
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            _colliders.UnregisterOnEnter(TickColliders);
            _colliders.UnregisterOnExit(TickColliders);
        }

        protected override void Awake()
        {
            base.Awake();

            _enterTime = Time.time;

            if (_axis != SnapAxis.X && _axis != SnapAxis.Z)
            {
                Debug.LogError("Stair axis need to be X or Z");
                Destroy(gameObject);
                return;
            }

            if (Extensions.HasNull(_up, _down, _insideBubble))
            {
                Debug.LogError("Bubbles not linked");
                Destroy(gameObject);
                return;
            }

            _areaColliders = _area.GetComponentsInChildren<CubeCollider>();
            if (_areaColliders.Length <= 0)
            {
                Debug.Log("Stair area doesn't have any cube colliders");
                Destroy(gameObject);
                return;
            }
        }

        public bool Jump()
        {
            SetActive(true, Player.transform.position.y > _areaColliders.MidPoint().y);
            return false;
        }

        public void JumpInside()
        {
            Vector3 impulse = _moveVector.normalized;
            if (Player.MirrorAxis) impulse.x *= -1;

            if (_axis == SnapAxis.X)
            {
                var x = impulse.x;
                impulse.x = impulse.z;
                impulse.z = x;
            }

            SetActive(false);

            if (_moveVector.magnitude < 0.2f) return;

            _moveVector = Vector2.zero;
            Player.MoveVector = Vector2.zero;

            Player.StairImpulsing = true;
            Player.SetVelocity(impulse * Player.StairJumpSpeed);
        }

        public void SetMoveVector(Vector2 value) => _moveVector = value;
    
        private bool InsideArea(Vector3 position)
        {
            for (int i = 0; i < _areaColliders.Length; i++)
            {
                if (_areaColliders[i].InsideCollider(position)) return true;
            }

            return false;
        }

        public void SetActive(bool value, bool up = false, CubeCollider area = null)
        {
            if (value == Active) return;

            Player.CanStairUp = true;
            Player.CanStairDown = true;
            Player.CanStairLeft = true;
            Player.CanStairRight = true;

            if (!value)
            {
                _enterTime = Time.unscaledTime + .5f;
                switch (_axis)
                {
                    case SnapAxis.X:
                        if (up)
                        {
                            _protagonist.Lerp(_up.Position, new Vector3(0, Player.RightAngle, 0), 5, 0.1f, true, true);
                            break;
                        }

                        _protagonist.LerpAxis(_axis, _down.Position.x, new Vector3(0, Player.RightAngle, 0), 5, 0.05f, true, true);
                        break;
                    case SnapAxis.Z:
                        if (up)
                        {
                            _protagonist.Lerp(_up.Position, new Vector3(0, Player.RightAngle, 0), 5, 0.1f, true, true);
                            break;
                        }

                        _protagonist.LerpAxis(_axis, _down.Position.z, new Vector3(0, Player.RightAngle, 0), 5, 0.05f, true, true);
                        break;

                    default:
                        Debug.LogError("Stair Axis Need To Be X or Z");
                        break;
                }

                _protagonist.CurrentStair = null;
                _protagonist.SetMoveStage(MoveStage.Walk);
                _onExit?.Invoke();

                return;
            }

            if (Time.unscaledTime < _enterTime) return;
            _enterTime = Time.unscaledTime + .5f;

            CubeCollider tArea = area ?? NearestArea();

            Vector3 p = tArea.ClosestInside(_protagonist.UpStair);
            p.y -= (Player.UpStair.y - Player.transform.position.y);
            
            Vector3 euler = _protagonist.transform.localEulerAngles;

            switch (_axis)
            {
                case SnapAxis.X:
                    p.x = _insideBubble.Position.x;
                    euler.y = _flipEuler ? 270 : 90;
                    break;

                case SnapAxis.Y:
                    p.y = _insideBubble.Position.y;
                    break;

                case SnapAxis.Z:
                    p.z = _insideBubble.Position.z;
                    euler.y = _flipEuler ? 180 : 0;
                    break;
            }

            _protagonist.Lerp(tArea.ClosestInside(p), euler, 5, 0.1f, true, false, false, () =>
            {
                _protagonist.ERigidbody.useGravity = false;
                _protagonist.CurrentStair = this;
            });

            _protagonist.SetMoveStage(MoveStage.Climb);
            _protagonist.IsGroundCheck();

            _onEnter?.Invoke();
        }

        protected override void Update()
        {
            if (Active)
            {
                if (_moveVector.x < 0 && !InsideArea(_protagonist.LeftStair))
                {
                    if (_disableLeft)
                    {
                        Player.CanStairLeft = false;
                        Player.CanStairRight = true;
                    }
                    else
                    {
                        SetActive(false);
                        return;
                    }
                }
                else if (_moveVector.x > 0 && !InsideArea(_protagonist.RightStair))
                {
                    if (_disableRight)
                    {
                        Player.CanStairLeft = true;
                        Player.CanStairRight = false;
                    }
                    else
                    {
                        SetActive(false);
                        return;
                    }
                }
                else
                {
                    Player.CanStairLeft = true;
                    Player.CanStairRight = true;
                }

                if (_moveVector.y > 0 && !InsideArea(_protagonist.UpStair))
                {
                    if (_disableUp)
                    {
                        Player.CanStairUp = false;
                        Player.CanStairDown = true;
                    }
                    else
                    {
                        _protagonist.Set_Anim_FinishClimbing();
                        _protagonist.CanMove = false;
                        return;
                    }
                }
                else if (_moveVector.y < 0 && !InsideArea(_protagonist.DownStair))
                {
                    if (_disableDown)
                    {
                        Player.CanStairUp = true;
                        Player.CanStairDown = false;
                    }
                    else
                    {
                        SetActive(false);
                        return;
                    }
                }
                else
                {
                    Player.CanStairUp = true;
                    Player.CanStairDown = true;
                }

                SetMoveVector(_protagonist.MoveVector);
                return;
            }

            bool _insideAnyArea = _areaColliders.ContainsPoint(Player.transform.position, out CubeCollider area);

            if (_insideAnyArea && !Player.IsGrounded)
            {
                SetActive(true);
                return;
            }
        }

        private void DisableWalk(Vector3 moveVec)
        {
            _protagonist.MoveVector = moveVec;
            _protagonist.EAnimator.SetBool("IsWalking", false);
        }

        private void TickColliders()
        {
            if (InsideAnyCollider())
            {
                if (Active) return;
                
                if (_groundEnter)
                {
                    SetActive(true);
                    return;
                }

                Player.RegisterJump(Jump);
            }
            else Player.UnregisterJump(Jump);
        }

        private CubeCollider NearestArea()
        {
            int index = -1;
            float minDistance = float.PositiveInfinity;

            for (int i = 0; i < _areaColliders.Length; i++)
            {
                float distance = (_areaColliders[i].Position - Player.transform.position).magnitude;

                if (distance < minDistance)
                {
                    index = i;
                    minDistance = distance;
                }
            }

            return index < 0 ? null : _areaColliders[index];
        }
    }
}