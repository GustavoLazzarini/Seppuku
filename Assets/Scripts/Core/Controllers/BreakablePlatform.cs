//Created by Galactspace

using UnityEngine;

namespace Core
{
    public class BreakablePlatform : MonoBehaviour
    {
        private float _insideSpan;

        [SerializeField, Min(0)] private float _breakTime;

        private void Awake()
        {
            _insideSpan = -1;
        }

        private void Update()
        {
            if (_insideSpan >= 0) 
                _insideSpan += Time.deltaTime;
        
            if (_insideSpan > _breakTime)
                gameObject.SetActive(false);
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (!collision.IsPlayer()) return;
            _insideSpan = 0;
        }

        private void OnCollisionExit(Collision collision)
        {
            if (!collision.IsPlayer()) return;
            _insideSpan = -1;
        }
    }
}