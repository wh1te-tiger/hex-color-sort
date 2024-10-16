using UnityEngine;

namespace Scripts
{
    public class UiAudioSource
    {
        public AudioSource Value;

        public UiAudioSource(AudioSource value)
        {
            Value = value;
        }
    }
    
    public class WorldAudioSource
    {
        public AudioSource Value;

        public WorldAudioSource(AudioSource value)
        {
            Value = value;
        }
    }
}