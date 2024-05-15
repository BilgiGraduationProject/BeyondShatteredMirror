using System;
using System.Collections.Generic;
using Runtime.Enums.UI;
using UnityEngine;
using Runtime.Signals;
using Runtime.Utilities;

namespace Runtime.Managers
{
    [System.Serializable]
    public class SFXClip
    {
        public SFXTypes type;
        public AudioClip clip;
    }
    
    public class SFXManager : MonoBehaviour
    {
        #region Self Variables

        #region Serialized Variables
        
        [Header("Audio Source Settings")]
        [SerializeField] private AudioSource musicAudioSource;
        [SerializeField] private AudioSource soundAudioSource;
        [Space(10)]
        [SerializeField] private List<SFXClip> audioClips = new List<SFXClip>();
        
        #endregion

        #endregion

        #region SubscribeEvents and UnsubscribeEvents
        
        private void OnEnable()
        {
            SubscribeEvents();
        }

        private void SubscribeEvents()
        {
            CoreUISignals.Instance.onPlayMusic += OnPlayMusic;
            CoreUISignals.Instance.onPlayOneShotSound += OnPlayOneShotSound;
            CoreUISignals.Instance.onStopMusic += OnStopMusic;
            CoreUISignals.Instance.onSetMusicVolume += OnSetMusicVolume;
            CoreUISignals.Instance.onSetSoundVolume += OnSetSoundVolume;
        }

        private void UnSubscribeEvents()
        {
            CoreUISignals.Instance.onPlayMusic -= OnPlayMusic;
            CoreUISignals.Instance.onPlayOneShotSound -= OnPlayOneShotSound;
            CoreUISignals.Instance.onStopMusic -= OnStopMusic;
            CoreUISignals.Instance.onSetMusicVolume -= OnSetMusicVolume;
            CoreUISignals.Instance.onSetSoundVolume -= OnSetSoundVolume;
        }

        private void OnDisable()
        {
            UnSubscribeEvents();
        }
        
        #endregion

        private void OnPlayMusic(SFXTypes type)
        {
            foreach (var sfxClip in audioClips)
            {
                if (sfxClip.type == type)
                {
                    musicAudioSource.clip = sfxClip.clip;
                    musicAudioSource.Play();
                    return;
                }
            }
        }
        
        private void OnStopMusic()
        {
            musicAudioSource.Stop();
        }
        
        private void OnPlayOneShotSound(SFXTypes type)
        {
            foreach (var sfxClip in audioClips)
            {
                if (sfxClip.type == type)
                {
                    soundAudioSource.PlayOneShot(sfxClip.clip);
                    return;
                }
            }
        }
        
        private void OnSetMusicVolume(float volume)
        {
            musicAudioSource.volume = volume;
        }

        private void OnSetSoundVolume(float volume)
        {
            soundAudioSource.volume = volume;
        }
    }
}
