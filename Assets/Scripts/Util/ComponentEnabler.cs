//Made by Galactspace Studios

using UnityEngine;
using Scriptable.Generic;
using System.Threading.Tasks;

namespace Util
{
    public class ComponentEnabler : MonoBehaviour
    {
        [SerializeField] private MonoBehaviour component;
        [SerializeField] private BoolChannelSo channelSo;

        private void ReenableThis() => this.enabled = true;

        public void Setup(MonoBehaviour component, BoolChannelSo channel)
        {
            this.channelSo = channel;
            this.component = component;

            this.enabled = false;
            Taskf.Invoke(ReenableThis, .5f);
        }

        private void EnableComponent(bool arg) => component.enabled = arg;

        private void Start() => component.enabled = channelSo.Baked;
        private void OnEnable() => channelSo.Channel += EnableComponent;
        private void OnDisable() => channelSo.Channel -= EnableComponent;
    }
}
