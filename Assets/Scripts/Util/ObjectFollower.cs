//Created by Galactspace

using UnityEngine;

namespace Util
{
    public class ObjectFollower : MonoBehaviour
    {
        [SerializeField] private Transform _object;

        [SerializeField] private bool _x = true;
        [SerializeField] private bool _y = true;
        [SerializeField] private bool _z = true;

        private void OnValidate()
        {
            FixedUpdate();
        }

        private void FixedUpdate()
        {
            if (_object == null) return;

            if (_x && _y && _z)
            {
                transform.position = _object.position;
                return;
            }

            if (_x) transform.position = new Vector3(_object.transform.position.x, transform.position.y, transform.position.z);
            if (_y) transform.position = new Vector3(transform.position.x, _object.transform.position.y, transform.position.z);
            if (_z) transform.position = new Vector3(transform.position.x, transform.position.y, _object.transform.position.z);
        }
    }
}