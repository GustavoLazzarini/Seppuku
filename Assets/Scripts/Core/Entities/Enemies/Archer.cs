//Created by Galactspace

using UnityEngine;

namespace Core.Entities
{
    public class Archer : Enemy
    {
        private bool _isLookingDown;

        [Space]
        [SerializeField] private bool _lookUseMatrix;
        [SerializeField] private Vector3 _lookOffset;

        [Space]
        [SerializeField] private EnemyVision _shortVision;
        [SerializeField] private EnemyVision _longVision;

        private Vector3 LookPosition => !_lookUseMatrix ? transform.position + _lookOffset : transform.localToWorldMatrix.MultiplyPoint3x4(_lookOffset);

        protected override void Awake()
        {
            base.Awake();

            _shortVision.gameObject.SetActive(true);
            _longVision.gameObject.SetActive(false);
        }

        private void OnEnable()
        {
            _ai.PathCompleted += NextPoint;
        }

        private void OnDisable()
        {            
            _ai.PathCompleted -= NextPoint;
        }

        public override void Attack()
        {
            throw new System.NotImplementedException();
        }

        private void NextPoint()
        {
            int lastWalk = _walkIndex;
            Vector3 enterPos = transform.position;

            _walkIndex++;
            if (_walkIndex >= _walkPositions.Length) _walkIndex = 0;

            if (!_walkPositions[lastWalk].LookDown) return;

            Routinef.Cooldown(x =>
            {
                CanMove = x;
                _isLookingDown = !x;
                EAnimator.SetBool("LookingDown", !x);

                float lookAngle = (LookPosition - transform.position).normalized.ToString() switch
                {
                    "(1.00, 0.00, 0.00)" => 90,
                    "(-1.00, 0.00, 0.00)" => 270,
                    "(0.00, 0.00, 1.00)" => 0,
                    "(0.00, 0.00, -1.00)" => 180,
                    _ => throw new System.Exception("Look offset is strange")
                };

                if (x)
                {
                    Lerp(_walkPositions[lastWalk].Position, new Vector3(0, RightAngle, 0));
                    CanMove = true;

                    _shortVision.gameObject.SetActive(true);
                    _longVision.gameObject.SetActive(false);
                }
                else
                {
                    Lerp(LookPosition, new Vector3(0, lookAngle, 0));
                    CanMove = false;

                    _shortVision.gameObject.SetActive(false);
                    _longVision.gameObject.SetActive(true);
                }
            }, 5, this);
        }

        protected override Vector3 GetPosition()
        {
            if (_freeWalk)
                return new(Random.Range(_moveRect.min.x, _moveRect.max.x), transform.position.y, transform.position.z);

            Vector3 pos = _walkPositions[_walkIndex].Position;

            SetMoveAxis(_walkPositions[_walkIndex].Axis, _walkPositions[_walkIndex].MirrorAxis);

            return pos;
        }

        protected override void OnDrawGizmosSelected()
        {
            base.OnDrawGizmosSelected();
            Gizmosf.DrawSphere(LookPosition, 0.1f, Color.red);
        }
    }
}