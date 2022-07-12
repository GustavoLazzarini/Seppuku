//Made by Galactspace Studios

using UnityEngine;
using Scriptable.Scene;
using Scriptable.Transition;

using USceneManager = UnityEngine.SceneManagement.SceneManager;
using System.Threading.Tasks;

namespace Core.Management
{
    public class SceneManager : MonoBehaviour
    {
        private AsyncOperation _preloadedScene;

        [SerializeField] private SceneCallerChannelSo callerChannel;
        [SerializeField] private TransitionCallerSo transitionCaller;

        private bool HasPreloaded => _preloadedScene != null;
        private bool IsLoading { get; set; }

        private float LoadingProgress => _preloadedScene.progress;

        public async void Preload(string name)
        {
            if (HasPreloaded || IsLoading) return;

            _preloadedScene = USceneManager.LoadSceneAsync(name);
            _preloadedScene.allowSceneActivation = false;
            IsLoading = true;

            while (HasPreloaded && LoadingProgress < 0.9f)
            {
                UpdateLoading();
                await Task.Yield();
            }
        }

        public void ActivatePreloaded()
        {
            if (!HasPreloaded || !IsLoading) return;
            
            _preloadedScene.allowSceneActivation = true;
            _preloadedScene = null;
            IsLoading = false;
        }

        public async void Load(string name, float delay, bool transition)
        {
            if (IsLoading) return;
            
            Preload(name);
            await Taskf.WaitSeconds(delay);

            if (transition) transitionCaller.InChannel.Invoke(ActivatePreloaded);
            else ActivatePreloaded();
        }

        public void Reload(bool transition)
        {
            if (IsLoading) return;

            Player.CanMove = false;
            Load(Player.gameObject.scene.name, 0, transition);
        }

        private void UpdateLoading() => callerChannel.LoadProgressChannel.Invoke(LoadingProgress);
    }
}
