using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Sound
{
    public class SoundManager : MonoBehaviour
    {
        [SerializeField] private AudioClip clickClip;
        [SerializeField] private List<AudioClip> shootClips;

        private AudioSource audioSource;

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }

        void Start()
        {
            SoundEvents.Instance.OnClickSound += ClickSound;
            SoundEvents.Instance.OnShootSound += ShootSound;
            audioSource = GetComponent<AudioSource>();
            audioSource.volume = 0.1f;
        }

        private void OnDestroy()
        {
            if (SoundEvents.Instance != null)
            {
                SoundEvents.Instance.OnClickSound -= ClickSound;
                SoundEvents.Instance.OnShootSound -= ShootSound;
            }
        }

        public void ClickSound()
        {
            audioSource.PlayOneShot(clickClip);
        }

        private void ShootSound()
        {
            int randomIndex = UnityEngine.Random.Range(0, shootClips.Count);
            audioSource.PlayOneShot(shootClips[randomIndex]);
        }
    }
}
