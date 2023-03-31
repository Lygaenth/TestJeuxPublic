namespace TestJeux.Business.Entities.Stats
{
    public class StatModel
    {
        public string Name { get; set; }
        public int Value { get; set; }

        public StatModel(string name, int value)
        {
            Name = name;
            Value = value;
        }
    }
}
