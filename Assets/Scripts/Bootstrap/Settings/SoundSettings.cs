using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Scripts
{
    [CreateAssetMenu(fileName = "Sound Settings", menuName = "Configurations/Game Settings/Sound Settings", order = 0)]
    public class SoundSettings : ScriptableObject
    {
        [SerializeField] private List<AudioClipData> sounds;

        public AudioClipData GetSound(SoundType type)
        {
            var res = sounds.Where(d => d.SoundType == type).ToArray();
            if (type == SoundType.Shift)
            {
                return res[Random.Range(0, res.Length)];
            }
            
            return res.Single();
        }
    }
    
    [System.Serializable]
    public struct AudioClipData
    {
        public SoundType SoundType;
        public AudioClip Clip;
        public float Volume;
    }

    public enum SoundType
    {
        Collapse,
        Shift,
        Landed,
        Win,
        Fail
    }
}