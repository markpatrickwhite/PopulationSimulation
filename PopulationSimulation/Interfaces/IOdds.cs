namespace PopulationSimulation.Interfaces
{
    public interface IOdds
    {
        int Denominator { get; set; }
        int Numerator { get; set; }
        bool OccuranceHappens();
    }
}