//Made by Galactspace Studios

using System;
using UnityEngine;
using System.Threading.Tasks;

namespace Core.Transitions
{
    public abstract class Transition : MonoBehaviour
    {
        public async void In(Action onComplete)
        {
            await TransitionIn();
            onComplete?.Invoke();
        }

        public async void Out(Action onComplete)
        {
            await TransitionOut();
            onComplete?.Invoke();
            Destroy(gameObject);
        }

        protected abstract Task TransitionIn();
        protected abstract Task TransitionOut();
    }
}
