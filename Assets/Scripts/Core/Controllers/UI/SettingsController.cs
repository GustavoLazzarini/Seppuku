//Made by Galactspace Studios

using UnityEngine;
using Core.UIElements;
using Scriptable.Save;
using Scriptable.Generic;
using UnityEngine.UIElements;
using Scriptable.Configuration;

namespace Core.Controllers.UI
{
    public class SettingsController : UIController
    {
        private UISlider _masterVolume;
        private UISlider _musicVolume;
        private UISlider _sfxVolume;
        private UISlider _renderScale;

        private UISliderInt _resolution;
        private UISliderInt _textureQuality;

        private UIToggle _vSync;
        private UIToggle _fullscreen;
        private UIToggle _postProcess;

        [Space]
        [SerializeField] private SettingsSo settings;
        [SerializeField] private ChannelSo menuChannel;
        [SerializeField] private SaveCallerSo saveChannel;

        protected override void OnEnable() 
        {
            base.OnEnable();

            LoadSettings();
            LoadElements();
            UpdateElements();
            LinkElements();

            gameInput.InteractChannel.Link(OnInteract);
            gameInput.BackChannel.Link(OnBack);
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            UnlinkElements();

            gameInput.InteractChannel.Unlink(OnInteract);
            gameInput.BackChannel.Unlink(OnBack);
        }

        private void OnInteract()
        {
            SaveSettings();
            OnBack();
        }

        private void OnBack()
        {
            LoadSettings();
            menuChannel.Invoke();
        }

        private void LoadElements()
        {
            _masterVolume = GetSlider("MasterVolume");
            _musicVolume = GetSlider("MusicVolume");
            _sfxVolume = GetSlider("SfxVolume");

            _resolution = GetSliderInt("Resolution");
            _textureQuality = GetSliderInt("TextureQuality");

            _vSync = GetToggle("VSync");
            _fullscreen = GetToggle("Fullscreen");
            _postProcess = GetToggle("PostProcess");
        }

        private void UpdateElements()
        {
            _masterVolume.value = settings.MasterVolume;
            _musicVolume.value = settings.MusicVolume;
            _sfxVolume.value = settings.SfxVolume;

            _resolution.lowValue = 0;
            _resolution.highValue = Screen.resolutions.Length - 1;
            _resolution.value = settings.Resolution;
            _resolution.Subdivisions = _resolution.highValue;

            _textureQuality.value = settings.TextureQuality;

            _vSync.value = settings.VSync;
            _fullscreen.value = settings.Fullscreen;
            _postProcess.value = settings.PostProcess;
        }

        private void LinkElements()
        {
            _masterVolume.RegisterValueChangedCallback(SetMasterVolume);
            _musicVolume.RegisterValueChangedCallback(SetMusicVolume);
            _sfxVolume.RegisterValueChangedCallback(SetSfxVolume);

            _resolution.RegisterValueChangedCallback(SetResolution);
            _textureQuality.RegisterValueChangedCallback(SetTextureQuality);

            _vSync.RegisterValueChangedCallback(SetVSync);
            _fullscreen.RegisterValueChangedCallback(SetFullscreen);
            _postProcess.RegisterValueChangedCallback(SetPostProcessing);
        }

        private void UnlinkElements()
        {
            _masterVolume.UnregisterValueChangedCallback(SetMasterVolume);
            _musicVolume.UnregisterValueChangedCallback(SetMusicVolume);
            _sfxVolume.UnregisterValueChangedCallback(SetSfxVolume);

            _resolution.UnregisterValueChangedCallback(SetResolution);
            _textureQuality.UnregisterValueChangedCallback(SetTextureQuality);

            _vSync.UnregisterValueChangedCallback(SetVSync);
            _fullscreen.UnregisterValueChangedCallback(SetFullscreen);
            _postProcess.UnregisterValueChangedCallback(SetPostProcessing);
        }

        public void SaveSettings()
        {
            DebugManager.Info($"[SettingsController] SavedSettings");
            saveChannel.SaveSettings();
        }
        
        public void LoadSettings()
        {
            DebugManager.Info($"[SettingsController] LoadedSettings");
            saveChannel.LoadSettings();
        }

        public void SetSpeakerMode(ChangeEvent<int> arg) => settings.SpeakerMode = (AudioSpeakerMode)arg.newValue;
        public void SetMasterVolume(ChangeEvent<float> arg) => settings.MasterVolume = arg.newValue;
        public void SetMusicVolume(ChangeEvent<float> arg) => settings.MusicVolume = arg.newValue;
        public void SetSfxVolume(ChangeEvent<float> arg) => settings.SfxVolume = arg.newValue;
        public void SetVSync(ChangeEvent<bool> arg) => settings.VSync = arg.newValue;
        public void SetResolution(ChangeEvent<int> arg) => settings.Resolution = arg.newValue;
        public void SetFullscreen(ChangeEvent<bool> arg) => settings.Fullscreen = arg.newValue;
        public void SetPostProcessing(ChangeEvent<bool> arg) => settings.PostProcess = arg.newValue;
        public void SetTextureQuality(ChangeEvent<int> arg) => settings.TextureQuality = -arg.newValue;
    }
}
