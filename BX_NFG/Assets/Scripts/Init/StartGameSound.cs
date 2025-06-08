
using System;
using System.Collections;
using Assets.Scripts.Sound;
using UnityEngine;

namespace Assets.Scripts.Init
{
    public class StartGameSound : MonoBehaviour
    {
        private bool ready;

        private void Awake()
        {
            MusicManager.OnManagerSet += OnManagerSet;
            StartCoroutine(WaitForSoundManager());
        }

        private void Start()
        {
            SoundEvents.Instance.RaiseGameSound();
        }

        private void OnManagerSet()
        {
            ready = true;
        }

        private IEnumerator WaitForSoundManager()
        {
            while (!ready)
            {
                yield return null;
            }
            SoundEvents.Instance.RaiseGameSound();
        }
    }
}
