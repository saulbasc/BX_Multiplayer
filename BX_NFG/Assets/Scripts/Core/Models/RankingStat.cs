namespace Assets.Scripts.Core.Models
{
    public class RankingStat
    {
        private string name;
        private int value;

        public string Name => name;
        public int Value => value;

        public RankingStat(string name, int value)
        {
            this.name = name;
            this.value = value;
        }
    }
}
