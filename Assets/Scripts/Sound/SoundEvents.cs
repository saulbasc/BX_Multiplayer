using System;
using Assets.Scripts.Commons;

namespace Assets.Scripts.Sound
{
    public class SoundEvents : DefaultSingleton<SoundEvents>
    {
        public event Action OnClickSound;
        public event Action OnShootSound;
        public event Action OnMatchSound;
        public event Action OnEndMatchSound;
        public event Action OnGameSound;
        public event Action OnEndGameSound;

        public void RaiseClickSound() => OnClickSound?.Invoke();
        public void RaiseShootSound() => OnShootSound?.Invoke();
        public void RaiseMatchSound() => OnMatchSound?.Invoke();
        public void RaiseEndMatchSound() => OnEndMatchSound?.Invoke();
        public void RaiseGameSound() => OnGameSound?.Invoke();
        public void RaiseEndGameSound() => OnEndGameSound?.Invoke();
    }
}
