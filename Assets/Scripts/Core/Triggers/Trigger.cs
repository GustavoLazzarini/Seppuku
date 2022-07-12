//Made by Galactspace Studios

using UnityEngine;
using UnityEngine.Events;

namespace Core.Triggers
{
    public class Trigger : MonoBehaviour
    {
        [SerializeField] private bool multiple;
        [SerializeField] private UnityEvent onTrigger;

        protected virtual void Call()
        {
            onTrigger?.Invoke();
            if (!multiple) Destroy(gameObject);
        }
    }
}
