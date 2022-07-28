//Created by Galactspace

using UnityEngine;
using UnityEngine.Events;

namespace Core.Interactables
{
    public class Bridge : MonoBehaviour
    {
        public bool IsEnabled;
        [SerializeField] private float _enableSpeed;

        [Space]
        [SerializeField] private Vector3 _enableEuler;
        [SerializeField] private Vector3 _disabledEuler;

        [Space]
        [SerializeField] private UnityEvent<bool> onEnable;

        private void Awake()
        {
            Enable(IsEnabled);
        }

        public void Enable(bool value)
        {
            gameObject.LerpRotation(this, value ? _enableEuler : _disabledEuler, _enableSpeed);
            onEnable?.Invoke(value);
        }

        public void Enable(bool value, bool lerp)
        {
            if (lerp)
            {
                Enable(value);
                return;
            }
            
            transform.localEulerAngles = value ? _enableEuler : _disabledEuler;
            onEnable?.Invoke(value);
        }
    }
}