
namespace Assets.Scripts.Core.Models
{
    public class Attribute
    {
        public AttributeEnum Name { get; set; }
        public int Value { get; set; }

        public Attribute(AttributeEnum name, int value)
        {
            Name = name;
            Value = value;
        }
    }
}
