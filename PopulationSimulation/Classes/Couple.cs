using PopulationSimulation.Interfaces;

namespace PopulationSimulation.Classes
{
    public class Couple : ICouple
    {
        public Couple()
        {
        }

        public Couple(IPerson husband, IPerson wife)
        {
            Husband = husband;
            Wife = wife;
        }

        public IPerson Husband { get; set; }
        public IPerson Wife { get; set; }
    }
}