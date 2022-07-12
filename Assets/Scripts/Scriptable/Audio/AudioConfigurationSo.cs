//Made by Galactspace Studios

using UnityEngine;

namespace Scriptable.Audio
{
    [CreateAssetMenu(menuName = "Game/Audio/Audio Configuration")]
    public class AudioConfigurationSo : ScriptableObject
    {
        [Space]
        [SerializeField] private bool bypassEffects;
        [SerializeField] private bool bypassListenerEffects;
        [SerializeField] private bool bypassReverbZones;

        [Space]
        [SerializeField] [Range(0, 1)] private float volume = 1;
        [SerializeField] [Range(-3, 3)] private float pitch = 1;
        [SerializeField] [Range(-1, 1)] private float stereoPan = 0;
        [SerializeField] [Range(0, 1)] private float spatialBlend = 0;
        [SerializeField] [Range(0, 1.1f)] private float reverbZoneMix = 1;

        public void ApplySettings(ref AudioSource arg)
        {
            arg.bypassEffects = bypassEffects;
            arg.bypassListenerEffects = bypassListenerEffects;
            arg.bypassReverbZones = bypassReverbZones;

            arg.volume = volume;
            arg.pitch = pitch;
            arg.panStereo = stereoPan;
            arg.spatialBlend = spatialBlend;
            arg.reverbZoneMix = reverbZoneMix;
        }
    }
}