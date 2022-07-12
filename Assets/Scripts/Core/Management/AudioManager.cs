//Made by Galactspace Studios

using System;
using System.Linq;
using UnityEngine;
using Scriptable.Audio;
using UnityEngine.Audio;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Core.Management
{
    public class AudioManager : MonoBehaviour
    {
        private List<AudioSource> _usingSources;
        private Queue<AudioSource> _finishedSources;
        private Stack<AudioSource> _availableSources;

        private Transform _sourcesHolder;
        public Transform SourcesHolder => _sourcesHolder ??= CreateHolder();
        
        [Space]
        [SerializeField] private AudioMixerGroup _masterMixer;
        [SerializeField] private AudioMixerGroup _musicMixer;
        [SerializeField] private AudioMixerGroup _sfxMixer;

        private void SetAsHolderChild(Transform t) => t.parent = SourcesHolder;

        private Transform CreateHolder()
        {
            Transform holder = new GameObject("Holder").transform;
            holder.parent = transform;
            return holder;
        }

        private AudioSource CreateSource()
        {
            Transform sourceT = new GameObject("Source", typeof(AudioSource)).transform;
            AudioSource source = sourceT.GetComponent<AudioSource>();
            source.playOnAwake = false;
            SetAsHolderChild(sourceT);
            return source;
        }

        private AudioSource Request()
        {
            if (_availableSources.Count == 0) return CreateSource();
            if (_usingSources.Contains(_availableSources.Peek())) return CreateSource();

            return _availableSources.Pop();
        }

        private async void SetSourceAvailable(AudioSource source, bool fade = false)
        {
            if (fade)
            {
                await Fade(source, 0);
                source.Stop();
            }
            else source.Stop();

            source.loop = false;
            source.gameObject.name = "Available";
            _availableSources.Push(source);
        }

        private async Task Fade(AudioSource source, float volume)
        {
            while (Mathf.Abs(source.volume - volume) > 0.1f)
            {
                source.volume = ((source.volume * 3f) + (float)volume) / 4f;
                await Taskf.WaitSeconds(0.02f);
            }
            source.volume = volume;
        }

        private void CheckAvailables()
        {
            for (int i = 0; i < _usingSources.Count; i++)
            {
                AudioSource current = _usingSources[i];
                if (current.isPlaying || current.loop) continue;
                _finishedSources.Enqueue(current);
                _usingSources.Remove(current);
            }
        }

        private void UpdateQueue()
        {
            for (int i = 0; i < _finishedSources.Count; i++)
                SetSourceAvailable(_finishedSources.Dequeue());
        }
        
        private void Awake()
        {
            _usingSources = new List<AudioSource>();
            _finishedSources = new Queue<AudioSource>();
            _availableSources = new Stack<AudioSource>();
        }

        private void LateUpdate()
        {
            CheckAvailables();
            UpdateQueue();
        }

        private void OnStopAll() => StopAll();
        private void OnPlay(AudioClip clip, bool loop) => PlayMaster(clip, 1, 1, loop, false);
        private void OnStopClip(AudioClip clip) => StopClip(clip, false);

        public void Play(AudioClip clip, AudioMixerGroup mixer = null, float volume = 1, float pitch = 1, bool loop = false, bool fade = false)
        {
            if (clip == null) return;
            AudioSource source = Request();

            source.clip = clip;
            source.gameObject.name = $"Playing '{clip.name}'";

            source.outputAudioMixerGroup = mixer;
            source.volume = volume;
            source.pitch = pitch;
            source.loop = loop;

            source.spatialBlend = 0;
            source.transform.position = Vector3.zero;

            _usingSources.Add(source);

            if (fade)
            {
                source.volume = 0;
                source.Play();

                _ = Fade(source, volume);
            }
            else source.Play();
        }
        public void Play3D(AudioClip clip, Vector3 position, float blend, AudioMixerGroup mixer = null, float volume = 1, float pitch = 1, bool loop = false, bool fade = false)
        {
            if (clip == null) return;
            AudioSource source = Request();

            source.clip = clip;
            source.gameObject.name = $"Playing '{clip.name}'";

            source.outputAudioMixerGroup = mixer;
            source.volume = volume;
            source.pitch = pitch;
            source.loop = loop;

            source.spatialBlend = blend;
            source.transform.position = position;

            _usingSources.Add(source);

            if (fade)
            {
                source.volume = 0;
                source.Play();

                _ = Fade(source, volume);
            }
            else source.Play();
        }

        public void PlayMaster(AudioClip clip, float volume = 1, float pitch = 1, bool loop = false, bool fade = false)
        {
            Play(clip, _masterMixer, volume, pitch, loop, fade);
        }
        public void PlayMaster3D(AudioClip clip, Vector3 position, float blend, float volume = 1, float pitch = 1, bool loop = false, bool fade = false)
        {
            Play3D(clip, position, blend, _masterMixer, volume, pitch, loop, fade);
        }

        public void PlayMusic(AudioClip clip, float volume = 1, float pitch = 1, bool loop = false, bool fade = false)
        {
            Play(clip, _musicMixer, volume, pitch, loop, fade);
        }
        public void PlayMusic3D(AudioClip clip, Vector3 position, float blend, float volume = 1, float pitch = 1, bool loop = false, bool fade = false)
        {
            Play3D(clip, position, blend, _musicMixer, volume, pitch, loop, fade);
        }

        public void PlaySfx(AudioClip clip, float volume = 1, float pitch = 1, bool loop = false, bool fade = false)
        {
            Play(clip, _sfxMixer, volume, pitch, loop, fade);
        }
        public void PlaySfx3D(AudioClip clip, Vector3 position, float blend, float volume = 1, float pitch = 1, bool loop = false, bool fade = false)
        {
            Play3D(clip, position, blend, _sfxMixer, volume, pitch, loop, fade);
        }

        public void StopAll(bool fade = false)
        {
            for (int i = 0; i < _usingSources.Count; i++)
            {
                AudioSource current = _usingSources[i];
                SetSourceAvailable(current, fade);
            }
        }
        public void StopAllMasters(bool fade = false)
        {
            Stop(x => x.outputAudioMixerGroup == _masterMixer, fade);
        }
        public void StopAllSongs(bool fade = false)
        {
            Stop(x => x.outputAudioMixerGroup == _musicMixer, fade);
        }
        public void StopAllSfxs(bool fade = false)
        {
            Stop(x => x.outputAudioMixerGroup == _sfxMixer, fade);
        }

        public void Stop(Func<AudioSource, bool> predicate, bool fade = false)
        {
            foreach (AudioSource source in _usingSources.Where(predicate))
                SetSourceAvailable(source, fade);
        }

        public void StopClip(AudioClip clip, bool fade = false) => Stop(x => x.clip == clip, fade);
    }
}
