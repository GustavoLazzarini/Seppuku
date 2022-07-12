//Copyright Galactspace Studio

using System;
using UnityEngine;

namespace Core.Controllers
{
    public class CubeCollider : MonoBehaviour
    {
        private bool _insideTrigger;
        public bool InsideTrigger
        { 
            get => _insideTrigger;
            private set
            {
                if (_insideTrigger == value) return;

                _insideTrigger = value;

                if (value) OnEnter?.Invoke();
                else OnExit?.Invoke();
            }
        }

        [SerializeField] private Vector3 _size = new Vector3(1, 1, 1);
        [SerializeField] private Vector3 _offset;

        [Space]
        [SerializeField] private Transform _trackObject; 

        public event Action OnEnter;
        public event Action OnExit;

        private void FixedUpdate()
        {
            if (_trackObject != null) InsideTrigger = InsideCollider(_trackObject.position);
        }

        public void SetBounds(Vector3 offset, Vector3 size)
        {
            _size = size;
            _offset = offset;
        }

        public bool InsideCollider(Vector3 position)
        {
            float x = position.x;
            float y = position.y;
            float z = position.z;

            Vector3 pos = transform.position + _offset;

            Vector3 size = new Vector3(transform.lossyScale.x * _size.x, transform.lossyScale.y * _size.y, transform.lossyScale.z * _size.z);

            float borderLeft = pos.x - (size.x / 2);
            float borderRight = pos.x + (size.x / 2);

            float borderTop = pos.y + (size.y / 2);
            float borderBottom = pos.y - (size.y / 2);

            float borderFront = pos.z - (size.z / 2);
            float borderBack = pos.z + (size.z / 2);

            if (!(x > borderLeft && x < borderRight)) return false;
            if (!(y > borderBottom && y < borderTop)) return false;
            if (!(z > borderFront && z < borderBack)) return false;

            return true;
        }

        public Vector3 ClosestInside(Vector3 pos)
        {
            Vector3 p = new();

            Vector3 colCenter = transform.position + _offset;
            
            p.x = Mathf.Clamp(pos.x, colCenter.x - (_size.x / 2), colCenter.x + (_size.x / 2));
            p.y = Mathf.Clamp(pos.y, colCenter.y - (_size.y / 2), colCenter.y + (_size.y / 2));
            p.z = Mathf.Clamp(pos.z, colCenter.z - (_size.z / 2), colCenter.z + (_size.z / 2));

            return p;
        }

        public void OnDrawGizmosSelected()
        {
            Gizmosf.DrawCubeWithBorder(transform.position + _offset, new Vector3(transform.lossyScale.x * _size.x, transform.lossyScale.y * _size.y, transform.lossyScale.z * _size.z), new Color(0, 1, 0, 0.13f), new Color(0, 1, 0, 1));
        }
    }
}