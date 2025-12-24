using Configs;
using Message;
using Message.Messages;
using R3;
using UnityEngine;
using VContainer;

namespace Audio.Managers
{
    public class AudioManager : IAudioManager
    {
        [Inject] private AudioConfig _audioConfig;
        [Inject] private IMessageBroker _messageBroker;
        private AudioSource _backgroundMusicSource;
        private AudioSource _sfxSource;
        public void Initialize()
        {
            var audioManager = new GameObject("_AudioManager_");
            var backgroundMusicObject = new GameObject("BackgroundMusic");
            var sfxSourceObject = new GameObject("SfxSource");
            
            backgroundMusicObject.transform.SetParent(audioManager.transform);
            sfxSourceObject.transform.SetParent(audioManager.transform);
            
            backgroundMusicObject.AddComponent<AudioSource>();
            sfxSourceObject.AddComponent<AudioSource>();
            
            _backgroundMusicSource = backgroundMusicObject.GetComponent<AudioSource>();
            _sfxSource = sfxSourceObject.GetComponent<AudioSource>();
            
            _backgroundMusicSource.playOnAwake = false;
            _backgroundMusicSource.volume = 0.15f;
            _backgroundMusicSource.loop = true;
            _backgroundMusicSource.clip = _audioConfig.backgroundMusic;
            _backgroundMusicSource.Play();
            
            _sfxSource.playOnAwake = false;

            _messageBroker.Receive<PlaySfxMessage>().Subscribe(message =>
            {
                PlaySfx(message.Id);
            });
        }

        public void PlaySfx(string sfxId)
        {
            var audioClip = _audioConfig.GetSfxSound(sfxId);
            _sfxSource.PlayOneShot(audioClip);
        }

        public void PlayBackgroundMusic()
        {
            _backgroundMusicSource.Play();
        }

        public void StopBackgroundMusic()
        {
            _backgroundMusicSource.Stop();
        }

        public void PauseBackgroundMusic()
        {
            _backgroundMusicSource.Pause();
        }

        private AudioClip GetAudioClip(string audioId)
        {
            return null;
        }
    }
}