namespace PopulationSimulation
{
    public interface IReportEntry
    {
        int AdultCount { get; set; }
        int Births { get; set; }
        int ChildCount { get; set; }
        int Count { get; set; }
        int Deaths { get; set; }
        int ManCount { get; set; }
        int Marriages { get; set; }
        int Month { get; set; }
        int SeniorCount { get; set; }
        int WomanCount { get; set; }
    }
}