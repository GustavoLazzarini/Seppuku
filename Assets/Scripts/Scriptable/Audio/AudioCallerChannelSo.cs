//Copyright Galactspace Studios 2022

//References
using System;
using UnityEngine;
using UnityEngine.Audio;
using Scriptable.Generic;

namespace Scriptable.Audio
{
	[CreateAssetMenu(menuName = "Game/Audio/Audio Caller")]
	public class AudioCallerChannelSo : ScriptableObject
	{
		[Space]
		[SerializeField] private AudioClipBoolChannelSo _playAudioChannel;
		public AudioClipBoolChannelSo PlayAudioChannel => _playAudioChannel;

		[SerializeField] private AudioConfigurationChannelSo _setConfigChannel;
		public AudioConfigurationChannelSo SetConfigChannel => _setConfigChannel;

		[SerializeField] private AudioMixerGroupChannelSo _setMixerGroupChannel;
		public AudioMixerGroupChannelSo SetMixerGroupChannel => _setMixerGroupChannel;

		[SerializeField] private ChannelSo _stopAllChannel;
		public ChannelSo StopAllChannel => _stopAllChannel;

		[SerializeField] private AudioClipChannelSo _stopClipChannel;
		public AudioClipChannelSo StopClipChannel => _stopClipChannel;

		public void Invoke(AudioClip clip, bool loop, AudioConfigurationSo config = null, AudioMixerGroup mixerGroup = null)
		{
			bool clipIsNull = clip == null;
			bool configIsNull = config == null;
			bool mixerGroupIsNull = mixerGroup == null;

			if (!configIsNull) _setConfigChannel?.Invoke(config);
			if (!mixerGroupIsNull) _setMixerGroupChannel?.Invoke(mixerGroup);
			if (!clipIsNull) _playAudioChannel?.Invoke(clip, loop);			
		}

		public void Link(Action<AudioClip, bool> onPlay, Action<AudioConfigurationSo> onConfig, Action<AudioMixerGroup> onMixerGroup, Action onStopAll, Action<AudioClip> onStopClip)
		{
			PlayAudioChannel.Channel += onPlay;
			SetConfigChannel.Channel += onConfig;
			SetMixerGroupChannel.Channel += onMixerGroup;
			StopAllChannel.Channel += onStopAll;
			StopClipChannel.Channel += onStopClip;
		}

		public void Unlink(Action<AudioClip, bool> onPlay, Action<AudioConfigurationSo> onConfig, Action<AudioMixerGroup> onMixerGroup, Action onStopAll, Action<AudioClip> onStopClip)
		{
			PlayAudioChannel.Channel -= onPlay;
			SetConfigChannel.Channel -= onConfig;
			SetMixerGroupChannel.Channel -= onMixerGroup;
			StopAllChannel.Channel -= onStopAll;
			StopClipChannel.Channel -= onStopClip;
		}
	}
}
