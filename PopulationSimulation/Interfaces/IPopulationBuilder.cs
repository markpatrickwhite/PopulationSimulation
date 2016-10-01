using System.Collections.Generic;
using PopulationSimulation.Builders;
using PopulationSimulation.Classes;

namespace PopulationSimulation.Interfaces
{
    public interface IPopulationBuilder
    {
        PopulationBuilder AddPeople(IList<Person> people);
        PopulationBuilder AddPerson(GenderType genderType);
        PopulationBuilder AddStarterPeople(GenderType genderType, int count);
        Population Build();
    }
}