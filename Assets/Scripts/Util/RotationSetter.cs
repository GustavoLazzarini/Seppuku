//Made by Galactspace Studios

using Core;
using UnityEngine;

namespace Util
{
    public class RotationSetter : UnityEngine.MonoBehaviour
    {
        [Space]
        [SerializeField] private bool lerp;
        [SerializeField] private Vector3 rotation;

        public void Set() => Set(rotation);
        public void Set(Vector2 arg) => Set((Vector3)arg);
        public void Set(Vector3 arg) => transform.eulerAngles = arg;

        public void SetLocal() => SetLocal(rotation);
        public void SetLocal(Vector2 arg) => SetLocal((Vector3)arg);
        public void SetLocal(Vector3 arg)
        {
            if (lerp)
            {
                gameObject.LerpRotation(this, arg, 5);
                return;
            }

            transform.localEulerAngles = arg;
        }
    }
}
