
using Assets.Scripts.Commons;

namespace Assets.Scripts.Lobbi.Logic
{
    public class PlayerStatus : DefaultSingleton<PlayerStatus>
    {
        public bool JoinedGame {  get; set; }
        public bool InGame {  get; set; }
    }
}
