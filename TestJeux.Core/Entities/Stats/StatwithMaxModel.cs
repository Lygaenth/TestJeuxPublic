namespace TestJeux.Business.Entities.Stats
{
    public class StatwithMaxModel : StatModel
    {
        private int _maxvalue;
        public int MaxValue { get => _maxvalue; set => _maxvalue = value; }

        public StatwithMaxModel(string name, int value, int max)
            : base(name, value)
        {
            MaxValue = max;
        }
    }
}
