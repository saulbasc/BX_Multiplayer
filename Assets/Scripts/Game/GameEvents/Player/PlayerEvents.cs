
namespace Assets.Scripts.Game.GameEvents.Player
{
    public static class PlayerEvents
    {
        public delegate void ShootAction();
        public static ShootAction OnShootAction;

        public delegate void PassAction();
        public static PassAction OnPassAction;
    }
}
