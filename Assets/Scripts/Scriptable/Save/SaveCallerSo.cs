//Copyright Galactspace Studios 2022

//References
using System;
using UnityEngine;
using Scriptable.Generic;
using Scriptable.Abstract;
using Scriptable.Configuration;

namespace Scriptable.Save
{
	[CreateAssetMenu(menuName = "Game/Save/Save Caller")]
	public class SaveCallerSo : ScriptableObject
	{
		//Variables		
		public Func<string, object, object> GetChannel { get; set; }

		[Space]
		public StringChannelSo SetSaveChannel;
		public StringObjectChannelSo SetChannel;

		[Space]
		public ChannelSo LoadSettingsChannel;
		public ChannelSo SaveSettingsChannel;

		[Space]
		public ChannelSo SaveChannel;
		public ChannelSo LoadChannel;

		//Methods
		public T Get<T>(string key, object defaultValue)
		{
			if (GetChannel != null) return (T)GetChannel.Invoke(key, defaultValue);
			else return default;
		}

		public void Save() => SaveChannel?.Invoke();
		public void Load() => LoadChannel?.Invoke();
		public void SetSave(string save) => SetSaveChannel?.Invoke(save);
		public void Set(string key, object value) => SetChannel?.Invoke(key, value);
		public void SaveSettings() => SaveSettingsChannel?.Invoke();
		public void LoadSettings() => LoadSettingsChannel?.Invoke();
	}
}
