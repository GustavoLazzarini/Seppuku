//Copyright Galactspace Studio

using Core.Entities;
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

        public Vector3 Offset => _offset;
        public Vector3 Position => transform.position + _offset;
        public Vector3 Size => new(transform.lossyScale.x * _size.x, transform.lossyScale.y * _size.y, transform.lossyScale.z * _size.z);

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (_trackObject == null) _trackObject = FindObjectOfType<Protagonist>().transform;
        }
#endif

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
            
            p.x = Mathf.Clamp(pos.x, colCenter.x - (Size.x / 2) + 0.3f, colCenter.x + (Size.x / 2) - 0.3f);
            p.y = Mathf.Clamp(pos.y, colCenter.y - (Size.y / 2) + 0.3f, colCenter.y + (Size.y / 2) - 0.3f);
            p.z = Mathf.Clamp(pos.z, colCenter.z - (Size.z / 2) + 0.3f, colCenter.z + (Size.z / 2) - 0.3f);

            return p;
        }

        public void OnDrawGizmosSelected()
        {
            Gizmosf.DrawSphere((transform.position + _offset - (Size / 2)), 0.1f, Color.white);

            Gizmosf.DrawCubeWithBorder(transform.position + _offset, Size, new Color(0, 1, 0, 0.13f), new Color(0, 1, 0, 1));
        }
    }
}