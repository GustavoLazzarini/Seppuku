//Made by Galactspace Studios

using Pathfinding;
using Scriptable.Entities;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Core.Entities
{
    public abstract class Enemy : Entity
    {
        [System.Serializable]
        protected struct MovePoint
        {
            public bool Relax;
            public bool LookDown;
            public Vector3 Position;

            public MovePoint(bool relax, bool lookDown, Vector3 position)
            {
                Relax = relax;
                LookDown = lookDown;
                Position = position;
            }
        }

        protected EntityPathfinder _ai;
        protected List<EnemyVision> _visions = new();
        protected Collider _entityCollider;

        protected bool _freeWalk;
        protected int _walkIndex;
        protected float _updateTime;

        protected Vector3 CollisionPosition => transform.localToWorldMatrix.MultiplyPoint3x4(EnemyConfig.CollisionOffset);

        [Space]
        [SerializeField] protected bool _walk = true;

        [Space]
        [SerializeField] protected Bounds _moveRect;

        [Space]
        [SerializeField] protected MovePoint[] _walkPositions;

        [Space]
        [SerializeField] protected UnityEvent _onDeath;

        protected EnemyConfigurationSo EnemyConfig => (EnemyConfigurationSo)_config;

        public void RegisterVision(EnemyVision vision) => _visions.Add(vision);
        public void UnregisterVision(EnemyVision vision) => _visions.Remove(vision);

        public void TriggerDeathAnimation() => EAnimator.SetTrigger("Death");

        private void OnValidate()
        {
            if (_moveRect.size == Vector3.zero && _moveRect.center == Vector3.zero)
            {
                _moveRect.extents = Vector3.one;
                _moveRect.center = transform.position;
            }
        }

        protected override void Awake()
        {
            base.Awake();
            _entityCollider = GetComponent<Collider>();
            _ai = new EntityPathfinder((EnemyConfigurationSo)_config, GetComponent<Seeker>(), ERigidbody, RightAngle);

            if (_walkPositions.Length == 0)
            {
                _freeWalk = true;
                return;
            }

            _freeWalk = false;
            _walkIndex = 0;
        }

        private void Start()
        {
            SetMoveStage(MoveStage.Walk);
        }

        private bool IsPlayerVisible()
        {
            for (int i = 0; i < _visions.Count; i++)
            {
                if (_visions[i].IsVisible) return true;
            }

            return false;
        }

        public void EnableIsAttacking() { IsAttacking = true; }
        public void DisableIsAttacking() { IsAttacking = false; }

        private void Update()
        {
            if (!_walk || !IsAlive)
            {
                MoveVector = Vector3.zero;
                return;
            }

            if (_ai.CompletedPath && _updateTime < 0)
                _updateTime = Time.time + EnemyConfig.RelaxTime;

            CreatePath();

            MoveVector = RightAngle switch
            {
                0 => new Vector3(0, 0, _ai.WalkVector.z),
                90 => new Vector3(_ai.WalkVector.x, 0, 0),
                _ => throw new System.Exception("Right angle is strange (use 90 or 0)")
            };
            _ai.Tick();

            if (!IsAttacking && (Player.transform.position - CollisionPosition).magnitude < EnemyConfig.CollisionDistance)
            {
                Attack();
            }

            //if (_ai.CompletedPath && Time.time < _updateTime)
            //{
            //    MoveVector = Vector3.zero;
            //}
        }

        private void CreatePath()
        {
            bool playerVisible = IsPlayerVisible();
            if (playerVisible && _updateTime < 0) _updateTime = (Time.time + EnemyConfig.PathUpdateRate / 3); 

            if (Time.time < _updateTime || _updateTime < 0) return;

            _updateTime = -1;

            if (playerVisible)
            {
                IsRunning = true;
                AcelerationRate = 1;
                _updateTime = Time.time + (EnemyConfig.PathUpdateRate / 3);
                _ai.SetupPath(new(Player.transform.position.x, transform.position.y, Player.transform.position.z), 1f, true);
                return;
            }

            IsRunning = false;
            AcelerationRate = 0;

            _ai.SetupPath(GetPosition());
        }

        protected virtual Vector3 GetPosition()
        {
            if (_freeWalk) 
                return new(Random.Range(_moveRect.min.x, _moveRect.max.x), transform.position.y, transform.position.z);

            Vector3 pos = _walkPositions[_walkIndex].Position;
            if (Vector3.Distance(transform.position, pos) <= EnemyConfig.StopWalkDistance + 0.5f)
            {
                _walkIndex++;
                if (_walkIndex >= _walkPositions.Length) _walkIndex = 0;
                pos = _walkPositions[_walkIndex].Position;
            }

            return pos;
        }

        public void PlayerNotify(Vector3 position)
        {

        }

        public void Kill()
        {
            IsAlive = false;
            TriggerDeathAnimation();
        }

        public void StartKill()
        {
            _walk = false;
            EnablePhysics(false, false);
            _entityCollider.enabled = false;
        }

        public abstract void Attack();

        protected virtual void OnDrawGizmosSelected()
        {
            Gizmosf.DrawSphere(CollisionPosition, EnemyConfig.CollisionDistance, new Color(1, 0, 0, 0.1f));

            for (int i = 0; i < _walkPositions.Length; i++)
            {
                Gizmosf.DrawSphere(_walkPositions[i].Position, 0.1f, new Color(0, 0.5f, 0.5f, 1));
                if (i > 0) Gizmosf.DrawLine(_walkPositions[i - 1].Position, _walkPositions[i].Position, new Color(0, 0.5f, 0.5f), 2);
            }

            if (_walkPositions.Length > 0) return;

            Gizmosf.DrawSphere(_moveRect.min, .1f, Color.white);
            Gizmosf.DrawSphere(_moveRect.max, .1f, Color.white);
            Gizmosf.DrawCubeWithBorder(_moveRect.center, _moveRect.size, new(.3f, .3f, 1, .2f), new(0.3f, 0.3f, 1, 1));
        }

        public void ApplyCollisionDamage() => Player.Damage(EnemyConfig.CollisionDamage);
    }
}
