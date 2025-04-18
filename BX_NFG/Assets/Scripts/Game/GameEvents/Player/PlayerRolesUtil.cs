using System;

namespace Assets.Scripts.GameManager.GameEvents.Player
{
    public static class PlayerRoleUtils
    {
        public static bool HasRole(PlayerRole roles, PlayerRole roleToCheck)
        {
            return (roles & roleToCheck) != 0;
        }

        public static PlayerRole AddRole(PlayerRole roles, PlayerRole roleToAdd)
        {
            return roles | roleToAdd;
        }

        public static PlayerRole RemoveRole(PlayerRole roles, PlayerRole roleToRemove)
        {
            return roles & ~roleToRemove;
        }

        public static bool IsOnly(PlayerRole roles, PlayerRole specificRole)
        {
            return roles == specificRole;
        }

        public static bool HasAnyRole(PlayerRole roles)
        {
            return roles != PlayerRole.None;
        }
    }
}
