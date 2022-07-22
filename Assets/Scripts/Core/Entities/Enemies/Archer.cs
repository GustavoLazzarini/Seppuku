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

        private Vector3 LookPosition => !_lookUseMatrix ? transform.position + _lookOffset : transform.localToWorldMatrix.MultiplyPoint3x4(_lookOffset);

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

            if (_walkPositions[_walkIndex].LookDown)


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

                if (x) Lerp(_walkPositions[lastWalk].Position);
                else Lerp(LookPosition);

                SetEuler(new Vector3(0, !x ? lookAngle : RightAngle, 0));

            }, 5, this);
        }

        protected override Vector3 GetPosition()
        {
            if (_freeWalk)
                return new(Random.Range(_moveRect.min.x, _moveRect.max.x), transform.position.y, transform.position.z);

            Vector3 pos = _walkPositions[_walkIndex].Position;

            /*
            Vector3 relativePos = RightAngle switch
            {
                0 => new Vector3(_lookPositions[_walkIndex].x, _lookPositions[_walkIndex].y, transform.position.z),
                90 => new Vector3(transform.position.x, _lookPositions[_walkIndex].y, _lookPositions[_walkIndex].z),
                _ => throw new System.Exception("Right angle is not perpendicular to axis"),
            };

            if (Vector3.Distance(relativePos, pos) <= EnemyConfig.StopWalkDistance + 0.1f)
            {
                _walkIndex++;
                if (_walkIndex >= _lookPositions.Length) _walkIndex = 0;
                pos = _lookPositions[_walkIndex];

                Routinef.Cooldown(x =>
                {
                    CanMove = x;
                    _isLookingDown = !x;
                    _entityAnimator.SetBool("LookingDown", !x);
                }, 5, this);
            }
            */

            return pos;
        }

        protected override void OnDrawGizmosSelected()
        {
            base.OnDrawGizmosSelected();
            Gizmosf.DrawSphere(LookPosition, 0.1f, Color.red);
        }
    }
}