namespace PopulationSimulation.Interfaces
{
    public interface ICouple
    {
        IPerson Husband { get; set; }
        IPerson Wife { get; set; }
    }
}