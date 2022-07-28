//Copyright Galactspace Studio

using System;
using UnityEngine;
using Core.Entities;

using URandom = UnityEngine.Random;

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
        public Vector3 Size => Vector3.Scale(transform.lossyScale, _size);
        public Vector3 Position => transform.localToWorldMatrix.MultiplyPoint3x4(_offset);

        public Vector3 LocalMax => transform.localToWorldMatrix.MultiplyPoint3x4(_offset + (_size / 2));
        public Vector3 LocalMin => transform.localToWorldMatrix.MultiplyPoint3x4(_offset - (_size / 2));

        private Vector3 WorldMin => _offset - (_size / 2);
        private Vector3 WorldMax => _offset + (_size / 2);

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

        public Vector3 RandomPos()
        {
            float borderLeft = Mathf.Min(LocalMin.x, LocalMax.x);
            float borderRight = Mathf.Max(LocalMin.x, LocalMax.x);

            float borderTop = Mathf.Max(LocalMin.y, LocalMax.y);
            float borderBottom = Mathf.Min(LocalMin.y, LocalMax.y);

            float borderFront = MathF.Min(LocalMin.z, LocalMax.z);
            float borderBack = MathF.Max(LocalMin.z, LocalMax.z);

            return new(URandom.Range(borderLeft, borderRight),
                URandom.Range(borderBottom, borderTop),
                URandom.Range(borderBack, borderFront));
        }

        public void SetBounds(Vector3 offset, Vector3 size)
        {
            _size = size;
            _offset = offset;
        }

        public bool InsideCollider(Vector3 position)
        {
            Vector3 pos = transform.worldToLocalMatrix.MultiplyPoint3x4(position);

            float x = pos.x;
            float y = pos.y;
            float z = pos.z;

            float borderLeft = WorldMin.x;
            float borderRight = WorldMax.x;

            float borderTop = WorldMax.y;
            float borderBottom = WorldMin.y;

            float borderFront = WorldMin.z;
            float borderBack = WorldMax.z;

            if (!(x > borderLeft && x < borderRight)) return false;
            if (!(y > borderBottom && y < borderTop)) return false;
            if (!(z > borderFront && z < borderBack)) return false;

            return true;
        }

        public Vector3 ClosestInside(Vector3 pos)
        {
            Vector3 p = new();

            pos = transform.worldToLocalMatrix.MultiplyPoint3x4(pos);

            p.x = Mathf.Clamp(pos.x, WorldMin.x + 0.3f, WorldMax.x - 0.3f);
            p.y = Mathf.Clamp(pos.y, WorldMin.y + 0.3f, WorldMax.y - 0.3f);
            p.z = Mathf.Clamp(pos.z, WorldMin.z + 0.3f, WorldMax.z - 0.3f);

            return transform.localToWorldMatrix.MultiplyPoint3x4(p);
        }

        public void OnDrawGizmosSelected()
        {
            Gizmosf.DrawCubeWithBorder(_offset, _size, new Color(0, 1, 0, 0.13f), new Color(0, 1, 0, 1), transform.localToWorldMatrix);
        }
    }
}