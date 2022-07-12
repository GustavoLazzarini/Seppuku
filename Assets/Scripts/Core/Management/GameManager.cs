//Made by Galactspace Studios

using Util;
using UnityEngine;
using Scriptable.Generic;
using UnityEngine.Rendering;
using Scriptable.Transition;
using Scriptable.InputSystem;
using Scriptable.Configuration;
using UnityEngine.EventSystems;
using SMan = UnityEngine.SceneManagement;

namespace Core.Management
{
    public class GameManager : MonoBehaviour
    {
        private static GameManager instance;
        public static GameManager Instance => instance;

        private SaveManager _saveManager;

        private bool isQuitting;

        [SerializeField] private GameObject eventSystem;

        [Space]
        [SerializeField] private GameInputSo gameInput;
        [SerializeField] private GameConfigurationSo gameConfiguration;
        [SerializeField] private BoolChannelSo postProcessChannel;

        [Space]
        [SerializeField] private TransitionCallerSo transitionCaller;
        [SerializeField] private ChannelSo quitGameChannel;

        private int CurrentFramerate => (int)(1/Time.deltaTime);

        private bool OnTargetFramerate => CurrentFramerate == gameConfiguration.TargetFramerate;
        private bool VolumeHasComponentEnabler(Volume vol) => vol.gameObject.GetComponent<ComponentEnabler>() != null;

        private Volume[] allVols => FindObjectsOfType<Volume>();

        private void SetFramerate(int target) => Application.targetFrameRate = target;
        private void AddPostProcessController(Volume vol) => vol.gameObject.AddComponent<ComponentEnabler>().Setup(vol, postProcessChannel);

        private void Awake() 
        {
            if (instance != null)
            {
                DebugManager.Warning($"[GameManager] GameManager already instantiated, destroying this copy ('{gameObject.name}')");
                Destroy(gameObject);
                return;
            }

            instance = this;
            DontDestroyOnLoad(gameObject);
            
            EventSystem.current = Instantiate(eventSystem, Vector3.zero, Quaternion.identity, transform).GetComponent<EventSystem>();
         
            _saveManager = GetComponent<SaveManager>();

            DebugManager.Warning($"[GameManager] Added new instance of GameManger!");
            AddComponentEnablerToVolumes();

            Application.wantsToQuit += WantsToQuit;
            SMan.SceneManager.activeSceneChanged += ActiveSceneChanged;
            
            SetFramerate(gameConfiguration.TargetFramerate);
        }

        private void Update()
        {
            if (!OnTargetFramerate) SetFramerate(gameConfiguration.TargetFramerate);
        }

        private bool WantsToQuit()
        {
            DebugManager.Info($"[GameManager] App Quitting");
            
            DebugManager.CloseWriter();

            _saveManager.Save();
            _saveManager.Set("Settings", GameSettings);
            
            return true;
        }

        private void ActiveSceneChanged(SMan.Scene last, SMan.Scene current)
        {
            AddComponentEnablerToVolumes();
            transitionCaller.OutChannel.Invoke(null);
        }

        private void AddComponentEnablerToVolumes()
        {
            DebugManager.Info($"[GameManager] Adding ComponentEnablers to volumes in scene");
            Volume[] sceneVols = allVols;
            for (int i = 0; i < sceneVols.Length; i++)
                if (!VolumeHasComponentEnabler(sceneVols[i])) AddPostProcessController(sceneVols[i]);
        }

        private void OnEnable() => quitGameChannel.Link(QuitGame);
        private void OnDisable() => quitGameChannel.Unlink(QuitGame);

        private void QuitGame()
        {
            if (isQuitting) return;

            isQuitting = true;
            Application.Quit();
        }
    }
}
