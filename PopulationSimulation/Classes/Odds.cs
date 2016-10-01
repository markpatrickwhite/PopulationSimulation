using System;
using PopulationSimulation.Interfaces;

namespace PopulationSimulation.Classes
{
    public class Odds : IOdds
    {
        private int _denominator;
        private int _numerator;
        private readonly Random _random;

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

        public Odds(Random random, int numerator, int denominator)
        {
            _random = random;
            Numerator = numerator;
            Denominator = denominator;
        }

        public Odds()
        {
            _random = new Random();
            Numerator = 0;
            Denominator = 1;
        }

        public bool OccuranceHappens()
        {
            return _random.Next(0, Denominator) <= Numerator;
        }
    }

}