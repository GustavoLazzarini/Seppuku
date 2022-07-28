//Created by Galactspace

using UnityEngine;
using Core.Entities;
using UnityEngine.Events;

namespace Util
{
    public class EntityRotationChanger : MonoBehaviour
    {
        [SerializeField] private bool _mirror;
        [SerializeField] private SnapAxis _moveAxis;

        [Space]
        [SerializeField] private UnityEvent _onChangedAngle;

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out Entity entity))
            {
                if (entity.MoveAxis == _moveAxis) return;

                entity.SetMoveAxis(_moveAxis, _mirror);
                _onChangedAngle?.Invoke();
            }
        }
    }
}