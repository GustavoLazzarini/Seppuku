//Made by Galactspace Studios

using UnityEngine;
using UnityEngine.Events;
using Scriptable.Generic;

namespace Util
{
    public class ChannelAction : MonoBehaviour
    {
        [SerializeField] private bool multiple;
        [SerializeField] private ChannelSo channelSo;

        [Space]
        [SerializeField] private UnityEvent onEvent;

        private void OnEnable() => channelSo.Link(Invoke);
        private void OnDisable() => channelSo.Unlink(Invoke);
    
        private void Invoke()
        {
            onEvent.Invoke();
            if (!multiple) Destroy(gameObject);
        }
    }
}
