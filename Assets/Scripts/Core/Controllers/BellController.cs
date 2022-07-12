//Created by Galactspace

using Core.Entities;
using UnityEngine;

namespace Core.Controllers
{
    public class BellController : MonoBehaviour
    {
        private bool _played;

        [SerializeField] private AudioClip _dingSound;

        [Space]
        [SerializeField] private Vector3 _offset;
        [SerializeField] private float _range;

        private void OnCollisionEnter(Collision collision)
        {
            if (_played) return;
            if (!collision.IsPlayer()) return;

            Collider[] entities = Physics.OverlapSphere(transform.position + _offset, _range);
            foreach (Collider c in entities)
            {
                if (c.TryGetComponent(out Enemy e))
                {
                    e.PlayerNotify(transform.position);
                }
            }

            _played = true;
            AudioMan.PlaySfx3D(_dingSound, transform.position, .7f, 1, 1, false, false);
        }

        private void OnCollisionExit(Collision collision)
        {
            if (!collision.IsPlayer()) return;
            _played = false;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmosf.DrawSphere(transform.position + _offset, _range, new(.4f, 0f, 0.8f, 0.4f));
        }
    }
}