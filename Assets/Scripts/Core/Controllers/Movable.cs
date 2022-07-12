//Created by Galactspace

using Core.Attributes;
using System;
using UnityEngine;

namespace Core.Controllers
{
    public class Movable : MonoBehaviour
    {
        private Vector3[] _words;

        private int _index = 0;

        [SerializeField] private bool _autoMove;
        [SerializeField] private float _betweenDelay;

        [Space]
        [SerializeField, Range(0.001f, 0.4f)] private float _speed = 0.05f;
        
        [Space]
        [SerializeField] private Vector3[] _positions;

#if UNITY_EDITOR
        [Space]
        [SerializeField][Button] private bool _next;
        [SerializeField][Button] private bool _first;
        [SerializeField][Button] private bool _last;

        private void OnValidate()
        {
            if (_next)
            {
                _next = false;
                if (UnityEditor.EditorApplication.isPlaying) Next();
            }

            if (_first)
            {
                _first = false;
                if (UnityEditor.EditorApplication.isPlaying) First();
            }

            if (_last)
            {
                _last = false;
                if (UnityEditor.EditorApplication.isPlaying) Last();
            }
        }
#endif

        private void Awake()
        {
            _words = new Vector3[_positions.Length];
            for (int i = 0; i < _words.Length; i++) _words[i] = _positions[i] + transform.position;
        }

        public void Next()
        {
            if (_betweenDelay > 0)
            {
                int ind = _index;
                _index = -1;

                Routinef.Invoke(() => _index = (int)Mathf.Repeat(ind + 1, _words.Length), _betweenDelay, this);
                return;
            }

            _index = (int)Mathf.Repeat(_index + 1, _words.Length);
        }

        public void First()
        {
            _index = 0;
        }
        public void Last()
        {
            _index = _words.Length - 1;
        }

        private void FixedUpdate()
        {
            if (_index < 0) return;

            Vector3 delta = _words[_index] - transform.position;
            
            if (_autoMove && delta.magnitude < 0.2f) Next();
            transform.position += delta.normalized * _speed * Mathf.Clamp01(Mathf.Pow(delta.magnitude, 0.5f));
        }

        private void OnDrawGizmosSelected()
        {
            if (_positions.Length <= 0) return;
            for (int i = 0; i < _positions.Length; i++)
            {
                Gizmosf.DrawSphere(transform.position + _positions[i], 0.1f, Color.cyan);
                if (i > 0) Gizmosf.DrawLine(transform.position + _positions[i - 1], transform.position + _positions[i], Color.cyan, 2);
            }
        }
    }
}