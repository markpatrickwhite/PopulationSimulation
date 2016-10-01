using PopulationSimulation.Interfaces;

namespace PopulationSimulation.Classes
{
    public class Log : ILog
    {
        public Log(int m, string t)
        {
            Month = m;
            Text = t;
        }

        public int Month { get; set; }
        public string Text { get; set; }
    }
}