namespace PopulationSimulation
{
    public class Log
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