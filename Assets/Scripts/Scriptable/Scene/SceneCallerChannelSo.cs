//Made by Galactspace Studios

using System;
using UnityEngine;
using Scriptable.Generic;

namespace Scriptable.Scene
{
	[CreateAssetMenu(menuName = "Game/Scene/Scene Caller")]
    public class SceneCallerChannelSo : ScriptableObject
    {
        [SerializeField] private SceneChannelSo _loadChannel;
        public SceneChannelSo LoadChannel => _loadChannel;

        [SerializeField] private FloatChannelSo _loadProgressChannel;
        public FloatChannelSo LoadProgressChannel => _loadProgressChannel;

        [SerializeField] private StringChannelSo _preLoadChannel;
        public StringChannelSo PreLoadChannel => _preLoadChannel;

        [SerializeField] private ChannelSo _activatePreloadChannel;
        public ChannelSo ActivatePreloadChannel => _activatePreloadChannel;

        public void Link(Action<string, float, bool> onLoad, Action<string> onPreLoad, Action onActivatePreload)
        {
            LoadChannel.Link(onLoad);
            PreLoadChannel.Link(onPreLoad);
            ActivatePreloadChannel.Link(onActivatePreload);
        }

        public void Unlink(Action<string, float, bool> onLoad, Action<string> onPreLoad, Action onActivatePreload)
        {
            LoadChannel.Unlink(onLoad);
            PreLoadChannel.Unlink(onPreLoad);
            ActivatePreloadChannel.Unlink(onActivatePreload);
        }
    }
}
