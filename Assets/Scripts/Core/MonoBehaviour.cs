//Copyright Galactspace Studio

using UnityEngine;
using Core.Entities;
using Core.Management;
using Scriptable.Configuration;

namespace Core
{
    public class MonoBehaviour : UnityEngine.MonoBehaviour
    {
        private static SettingsSo _settings;
        public static SettingsSo GameSettings
        {
            get => _settings ??= SaveMan.Get("Settings", () => ScriptableObject.CreateInstance<SettingsSo>());
            set => _settings = value;
        }        

        private static GameManager _gameMan;
        public static GameManager GameMan => _gameMan ??= FindObjectOfType<GameManager>();

        private static AudioManager _audioMan;
        public static AudioManager AudioMan => _audioMan ??= FindObjectOfType<AudioManager>();

        private static CursorManager _cursorMan;
        public static CursorManager CursorMan => _cursorMan ??= FindObjectOfType<CursorManager>();

        private static DebugManager _debugMan;
        public static DebugManager DebugMan => _debugMan ??= FindObjectOfType<DebugManager>();

        private static EventSystemManager _eventSystemMan;
        public static EventSystemManager EventSystemMan => _eventSystemMan ??= FindObjectOfType<EventSystemManager>();

        private static SaveManager _saveMan;
        public static SaveManager SaveMan => _saveMan ??= FindObjectOfType<SaveManager>();

        private static TransitionManager _transitionMan;
        public static TransitionManager TransitionMan => _transitionMan ??= FindObjectOfType<TransitionManager>();

        private static PopupManager _popupMan;
        public static PopupManager PopupMan => _popupMan ??= FindObjectOfType<PopupManager>();

        private static InputSystemManager _inputSystemManager;
        public static InputSystemManager InputMan => _inputSystemManager ??= FindObjectOfType<InputSystemManager>();

        private static SceneManager _sceneMan;
        public static SceneManager SceneManager => _sceneMan ??= FindObjectOfType<SceneManager>();

        private static Protagonist _player;
        public static Protagonist Player { get => _player ??= FindObjectOfType<Protagonist>(); set => _player = value; }
    }
}