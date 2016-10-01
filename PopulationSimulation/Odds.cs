namespace PopulationSimulation
{
    public class Odds
    {
        private int _denominator;
        private int _numerator;

        public int Numerator
        {
            get { return _numerator; }
            set { _numerator = value; }
        }

        public int Denominator
        {
            get { return _denominator; }
            set
            {
                _denominator = value;
                if (value == 0)
                {
                    _numerator = 0;
                    _denominator = 1;
                }
            }
        }

        public Odds(int numerator, int denominator)
        {
            Numerator = numerator;
            Denominator = denominator;
        }

        public Odds()
        {
            Numerator = 0;
            Denominator = 1;
        }
    }

}