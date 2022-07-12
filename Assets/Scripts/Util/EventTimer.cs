//Made by Galactspace Studios

using UnityEngine;
using UnityEngine.Events;

namespace Util
{
    public class EventTimer : MonoBehaviour
    {
        private float _started = -1;

        [SerializeField] private bool onStart;
        [SerializeField] private float delay;

        [Space]
        [SerializeField] private UnityEvent onEnd;

        public void StartTimer() => _started = Time.time;

        private void Start()
        {
            if (onStart) StartTimer();
        }
        
        private void Update()
        {
            if (_started > -1 && Time.time > _started + delay)
            {
                _started = -1;
                onEnd.Invoke();

                if (onStart)
                    Destroy(this);
            }
        }
    }
}
