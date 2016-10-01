using System.Collections.Generic;

namespace PopulationSimulation
{
    public interface IPopulation
    {
        int Count { get; }
        IList<Person> People { get; set; }

        void AddPerson(Person p);
        int GetCountByGenderType(GenderType g);
        int GetCountByAgeType(AgeType a);
    }
}