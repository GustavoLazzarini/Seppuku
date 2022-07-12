//Created by Galactspace

using UnityEngine;
using Core.Entities;
using UnityEngine.Events;

namespace Util
{
    public class EntityRotationChanger : MonoBehaviour
    {
        [SerializeField] private float _rightAngle;

        [Space]
        [SerializeField] private UnityEvent _onChangedAngle;

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out Entity entity))
            {
                if (entity.RightAngle == _rightAngle) return;

                entity.RightAngle = _rightAngle;
                _onChangedAngle?.Invoke();
            }
        }
    }
}