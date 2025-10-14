using System;
using System.Collections.Generic;
using UnityEngine;

namespace Configs
{
    [CreateAssetMenu(fileName = "AudioConfig", menuName = "Configs/AudioConfig")]
    public class AudioConfig : ScriptableObject
    {
        [Header("Background Music")]
        public AudioClip backgroundMusic;
        
        [Header("SFX")]
        public List<AudioClipData> sfxSounds;

        public AudioClip GetSfxSound(string id)
        {
            var audioClipData = sfxSounds.Find(x => x.id == id);
            if (audioClipData != null) return audioClipData.audioClip;
            Debug.LogError("There is no sound effect with the id " + id + "!");
            return null;
        }
    }

    [Serializable]
    public class AudioClipData
    {
        public string id;
        public AudioClip audioClip;
    }
}