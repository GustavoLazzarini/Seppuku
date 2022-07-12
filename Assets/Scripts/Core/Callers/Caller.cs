//Made by Galactspace Studios

using UnityEngine;

namespace Core.Callers
{
    public abstract class Caller : MonoBehaviour
    {
        [SerializeField] protected bool onStart;
        [SerializeField] protected bool onEnable;

        protected virtual void OnEnable()
        {
            OnEnableSub();
            if (onEnable) Call();
        }

        protected virtual void Start() 
        {
            if (onStart) Call();
        }

        protected virtual void OnEnableSub() {}

        public abstract void Call();
    }
}
