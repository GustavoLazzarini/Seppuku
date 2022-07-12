//Made by Galactspace Studios

using UnityEngine;

namespace Util
{
    public class PositionSetter : MonoBehaviour
    {
        [Space]
        [SerializeField] private Vector3 position;

        public void Set() => Set(position);
        public void Set(Vector2 arg) => Set((Vector3)arg);
        public void Set(Vector3 arg) => gameObject.transform.position = arg;
    }
}
