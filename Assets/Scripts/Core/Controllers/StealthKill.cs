//Created by Galactspace

using UnityEngine;
using Core.Entities;

namespace Core.Controllers
{
    [RequireComponent(typeof(BoxCollider))]
    public class StealthKill : MonoBehaviour
    {
        public Vector3 KillPosition => _enemy.transform.localToWorldMatrix.MultiplyPoint3x4(_killOffset);

        [SerializeField] private Enemy _enemy;
        [SerializeField] private Vector3 _killOffset;

        private void OnValidate()
        {
            if (_enemy != null || transform.parent == null) return;
            _enemy = GetComponentInParent<Enemy>();
        }

        public void OnTriggerEnter(Collider other)
        {
            if (!other.IsPlayer()) return;
            Player.CurrentKillSpace = this;
        }

        private void OnTriggerExit(Collider other)
        {
            if (!other.IsPlayer()) return;
            Player.CurrentKillSpace = null;
        }

        public void StartedKill()
        {
            _enemy.StartKill();
        }

        public void KillEnemy()
        {
            _enemy.Kill();
            Player.CurrentKillSpace = null;

            Destroy(gameObject);
        }

        private void OnDrawGizmos()
        {
            Vector3 d = _enemy.transform.localToWorldMatrix.MultiplyPoint3x4(_killOffset);
            Gizmosf.DrawSphere(d, 0.1f, new Color(1, .1f, .1f));
        }
    }
}