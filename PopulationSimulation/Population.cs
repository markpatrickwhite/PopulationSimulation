using System.Collections.Generic;
using System.Linq;

namespace PopulationSimulation
{
    public class Population : IPopulation
    {
        public Population()
        {
            People = new List<Person>();
        }

        public IList<Person> People { get; set; }

        public int Count => People.Count;
        public int GetCountByGenderType(GenderType g) { return People.Count(p => p.Gender == g); }
        public int GetCountByAgeType(AgeType a) { return People.Count(p => p.AgeType == a); }
        public void AddPerson(Person p) { People.Add(p); }

    }
}
