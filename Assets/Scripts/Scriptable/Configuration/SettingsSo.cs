//Copyright Galactspace Studios 2022

//References
using System;
using UnityEngine;
using UnityEngine.Audio;
using Scriptable.Generic;
using UnityEngine.Rendering.HighDefinition;

namespace Scriptable.Configuration
{
	[CreateAssetMenu(menuName = "Game/Configuration/Settings")]
	public class SettingsSo : ScriptableObject
	{
		[Space]
		[SerializeField] private AudioMixer mixer;
		[SerializeField] private BoolChannelSo postProcessChannel;

		//Variables
		[Space]
		[SerializeField] private AudioSpeakerMode _speakerMode = AudioSpeakerMode.Stereo;
		public AudioSpeakerMode SpeakerMode 
		{
			get => _speakerMode;
			set => _speakerMode = value;
		}

		[Space]
		[SerializeField] [Range(-80, 0)] private float _masterVolume;
		public float MasterVolume
		{
			get => _masterVolume;
			set
			{
				_masterVolume = value;
				mixer.SetFloat("masterVolume", value);
			}
		}

		[SerializeField] [Range(-80, 0)] private float _musicVolume;
		public float MusicVolume
		{
			get => _musicVolume;
			set
			{
				_musicVolume = value;
				mixer.SetFloat("musicVolume", value);
			}
		}

		[SerializeField] [Range(-80, 0)] private float _sfxVolume;
		public float SfxVolume
		{
			get => _sfxVolume;
			set
			{
				_sfxVolume = value;
				mixer.SetFloat("sfxVolume", value);
			}
		}

		[Space]
		[SerializeField] private bool _vSync = true;
		public bool VSync 
		{
			get => _vSync;
			set 
			{
				_vSync = value;
				QualitySettings.vSyncCount = value ? 1 : 0;
			}
		}

		[SerializeField] private int _resolution = -1;
		public int Resolution
		{
			get => _resolution;
			set
			{				
				_resolution = value;
			
				if (_resolution == -1)
					_resolution = Screen.resolutions.Length - 1;

				#if !UNITY_EDITOR
					Resolution res = Screen.resolutions[_resolution];
					Screen.SetResolution(res.width, res.height, Fullscreen);
				#endif
			}
		}

		[SerializeField] private int _display = -1;
		public int CurrentDisplay
		{
			get => _display;
			set
			{
				_display = value;

				DebugManager.Info($"There are {Display.displays.Length} displays in this computer.");
				DebugManager.Info($"You are on the display: ({Display.main.systemWidth}x{Display.main.systemHeight})");
				
				PlayerPrefs.SetInt("UnitySelectMonitor", value);
				Resolution = Resolution;
			}
		}

		[Space]
		[SerializeField] private bool _fullscreen = true;
		public bool Fullscreen
		{
			get => _fullscreen;
			set 
			{
				_fullscreen = value;
				Screen.fullScreen = value;
			}
		}

		[Space]
		[SerializeField] private bool _postProcess = true;
		public bool PostProcess
		{
			get => _postProcess;
			set 
			{
				_postProcess = value;
				postProcessChannel.Invoke(value);
			}
		}

		[SerializeField] private int _textureQuality = 0;
		public int TextureQuality
		{
			get => _textureQuality;
			set 
			{
				_textureQuality = value;
				QualitySettings.masterTextureLimit = value;
			}
		}

		public void ApplyDocProperties(Func<string, object, Type, object> get)
		{
			SpeakerMode = (AudioSpeakerMode)(int)get("speakerMode", AudioSpeakerMode.Stereo, typeof(AudioSpeakerMode));

			MasterVolume = (float)get("MasterVolume", 0, typeof(float));
			MusicVolume = (float)get("MusicVolume", 0, typeof(float));
			SfxVolume = (float)get("SfxVolume", 0, typeof(float));

			VSync = (bool)get("VSync", true, typeof(bool));
			
			Resolution = (int)get("Resolution", Screen.resolutions.Length - 1, typeof(int));
			CurrentDisplay = (int)get("Display", -1, typeof(int));

			Fullscreen = (bool)get("Fullscreen", true, typeof(bool));

			PostProcess = (bool)get("PostProcess", true, typeof(bool));
			TextureQuality = (int)get("TextureQuality", 0, typeof(int));
		}
	}
}
