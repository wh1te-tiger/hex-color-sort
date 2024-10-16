using UnityEngine;

namespace Scripts
{
    public class SoundService
    {
        
        private readonly SoundSettings _soundSettings;
        private readonly WorldAudioSource _worldAudioSource;

        public SoundService(SoundSettings soundSettings, WorldAudioSource worldAudioSource)
        {
            _soundSettings = soundSettings;
            _worldAudioSource = worldAudioSource;
        }

        public void PlaySound(SoundType type, AudioSource source = null)
        {
            var data = _soundSettings.GetSound(type);
            
            source ??= _worldAudioSource.Value;
           
            source.volume = data.Volume;
            source.clip = data.Clip;
            source.Play();
        }
    }
}