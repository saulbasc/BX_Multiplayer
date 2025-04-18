using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.GameManager.GameEvents.Player
{
    [Flags]
    public enum PlayerRole
    {
        None = 0,
        Admin = 1 << 0,
        Referee = 1 << 1,
        Player = 1 << 2,
        Spectator = 1 << 3,
    }
}
