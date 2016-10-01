using System;
using System.Collections.Generic;
using PopulationSimulation.Builders;
using PopulationSimulation.Classes;

namespace PopulationSimulation.Interfaces
{
    public interface ISimulatorBuilder
    {
        SimulatorBuilder AddLogs(IList<Log> logs);
        SimulatorBuilder AddPeople(IList<Person> people);
        SimulatorBuilder AddPerson(GenderType genderType);
        SimulatorBuilder AddRandomNumberGenerator(Random random);
        SimulatorBuilder AddReports(Dictionary<int, IReportEntry> reports);
        SimulatorBuilder AddStarterPeople(GenderType genderType, int count);
        Simulator Build();
    }
}