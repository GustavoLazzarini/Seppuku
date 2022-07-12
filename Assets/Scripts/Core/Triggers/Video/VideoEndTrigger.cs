//Made by Galactspace Studios

using UnityEngine.Video;

namespace Core.Triggers
{
    public class VideoEndTrigger : Trigger
    {
        private VideoPlayer _player;

        private void Awake() => _player = GetComponent<VideoPlayer>(); 
        private void OnEnable() => _player.loopPointReached += OnVideoEnd;
        private void OnDisable() => _player.loopPointReached -= OnVideoEnd;

        private void OnVideoEnd(VideoPlayer arg)
        {
            Call();
        }
    }
}
