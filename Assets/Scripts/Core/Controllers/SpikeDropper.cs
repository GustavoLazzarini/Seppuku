//Created by Galactspace

using UnityEngine;

namespace Core.Controllers
{
    public class SpikeDropper : MonoBehaviour
    {
        private Rigidbody _rbody;

        [SerializeField] private float _speed;

        private void Awake()
        {
            _rbody = GetComponent<Rigidbody>();

            _rbody.useGravity = false;
        }

        [ContextMenu("Drop")]
        public void Drop()
        {
            _rbody.velocity = new(0, -_speed);
        }

        public void OnCollisionEnter(Collision collision)
        {
            if (collision.IsPlayer())
            {
                Player.Death();
            }
        }
    }
}