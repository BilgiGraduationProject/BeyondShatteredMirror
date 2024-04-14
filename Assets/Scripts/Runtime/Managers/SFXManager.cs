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
        [SerializeField] private AudioSource audioSource;
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
            CoreUISignals.Instance.onPlaySFX += OnPlaySFX;
        }

        private void UnSubscribeEvents()
        {
            CoreUISignals.Instance.onPlaySFX -= OnPlaySFX;
        }

        private void OnDisable()
        {
            UnSubscribeEvents();
        }
        
        #endregion

        private void Awake()
        {
            GetReferences();
        }
        
        void GetReferences()
        {
            audioSource = GetComponent<AudioSource>();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.G))
            {
                OnPlaySFX(SFXTypes.FactoryWhispers);
            }
            if (Input.GetKeyDown(KeyCode.H))
            {
                OnPlaySFX(SFXTypes.ButtonMenu);
            }
        }

        public void OnPlaySFX(SFXTypes type)
        {
            foreach (var sfxClip in audioClips)
            {
                if (sfxClip.type == type)
                {
                    audioSource.PlayOneShot(sfxClip.clip);
                    print(sfxClip.clip.name + " is playing".ColoredText(Color.cyan));
                    return;
                }
            }
        }
    }
}
