using PopulationSimulation.Interfaces;

namespace PopulationSimulation.Classes
{
    public class ReportEntry : IReportEntry
    {
        public int Month { get; set; }
        public int Count { get; set; }
        public int ManCount { get; set; }
        public int WomanCount { get; set; }
        public int Births { get; set; }
        public int Deaths { get; set; }
        public int Marriages { get; set; }
        public int ChildCount { get; set; }
        public int AdultCount { get; set; }
        public int SeniorCount { get; set; }
    }
}