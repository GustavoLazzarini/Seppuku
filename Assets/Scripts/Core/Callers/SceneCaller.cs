//Made by Galactspace Studios

using UnityEngine;
using Core.Attributes;
using Scriptable.Scene;

namespace Core.Callers
{
    public class SceneCaller : Caller
    {
        [SerializeField] private SceneCallerChannelSo caller;

        [Space]
        [SerializeField] private bool transition;
        [SerializeField] [Scene] private string scene;
        [SerializeField] private float delay;

        public override void Call() => caller.LoadChannel.Invoke(scene, delay, transition);
        public void Call(string scene, float delay, bool transition) => caller.LoadChannel.Invoke(scene, delay, transition);
    }
}
