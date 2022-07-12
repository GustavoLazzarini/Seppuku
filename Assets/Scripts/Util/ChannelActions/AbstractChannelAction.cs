//Made by Galactspace Studios

using UnityEngine;
using UnityEngine.Events;
using Scriptable.Abstract;

namespace Util
{
    public abstract class ChannelAction<T> : MonoBehaviour
    {
        [SerializeField] private bool multiple;
        [SerializeField] private ChannelSo<T> channelSo;

        [Space]
        [SerializeField] private UnityEvent<T> onEvent;

        private void OnEnable() => channelSo.Link(Invoke);
        private void OnDisable() => channelSo.Unlink(Invoke);
    
        private void Invoke(T arg)
        {
            onEvent.Invoke(arg);
            if (!multiple) Destroy(gameObject);
        }
    }
}
