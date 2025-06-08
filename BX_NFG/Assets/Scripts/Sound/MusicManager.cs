using System;
using UnityEngine;
using UnityEngine.Audio;

namespace Assets.Scripts.Sound
{
    public class MusicManager : MonoBehaviour
    {
        [SerializeField] private AudioClip matchSoundClip;
        [SerializeField] private AudioClip gameSoundClip;

        private AudioSource audioSource;

        public static Action OnManagerSet;

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }

        void Start()
        {
            SoundEvents.Instance.OnMatchSound += StartMatchSound;
            SoundEvents.Instance.OnEndMatchSound += StopMatchSound;
            SoundEvents.Instance.OnGameSound += StartGameSound;
            SoundEvents.Instance.OnEndGameSound += StopGameSound;
            audioSource = GetComponent<AudioSource>();
            audioSource.volume = 0.2f;
            OnManagerSet?.Invoke();
        }

        private void OnDestroy()
        {
            if (SoundEvents.Instance != null)
            {
                SoundEvents.Instance.OnMatchSound -= StartMatchSound;
                SoundEvents.Instance.OnEndMatchSound -= StopMatchSound;
                SoundEvents.Instance.OnGameSound -= StartGameSound;
                SoundEvents.Instance.OnEndGameSound -= StopGameSound;
            }
        }

        private void StartMatchSound()
        {
            audioSource.Stop();
            audioSource.clip = matchSoundClip;
            audioSource.loop = true;
            audioSource.volume = 0.2f;
            audioSource.Play();
        }

        private void StopMatchSound()
        {
            audioSource.Stop();
            audioSource.clip = gameSoundClip;
            audioSource.loop = true;
            audioSource.volume = 0.01f;
            audioSource.Play();
        }

        private void StartGameSound()
        {
            audioSource.Stop();
            audioSource.clip = gameSoundClip;
            audioSource.loop = true;
            audioSource.volume = 0.01f;
            audioSource.Play();
        }

        private void StopGameSound()
        {
            audioSource.Stop();
            audioSource.clip = matchSoundClip;
            audioSource.loop = true;
            audioSource.volume = 0.2f;
            audioSource.Play();
        }
    }
}
