//Made by Galactspace Studios

using UnityEngine;

namespace Util
{
    public class ObjectMover : MonoBehaviour
    {
        [SerializeField] private Vector3 moveSpeed;
        [SerializeField] private float multiplier;

        private Vector3 GetNext() => (Vector3)moveSpeed * multiplier * Time.fixedDeltaTime;
        
        private void SetPosition(Vector3 arg) => transform.position = arg;
        private void AddPosition(Vector3 arg) => SetPosition(transform.position + arg);

        private void FixedUpdate() => AddPosition(GetNext());
    }
}
