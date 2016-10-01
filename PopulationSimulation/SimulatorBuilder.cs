using System;
using System.Collections.Generic;

namespace PopulationSimulation
{
    public class SimulatorBuilder
    {
        private readonly Simulator _simulator;
        public SimulatorBuilder()
        {
            _simulator = new Simulator();
        }

        public SimulatorBuilder AddStarterPeople(GenderType genderType, int count)
        {
            while (_simulator.Population.GetCountByGenderType(genderType) < count)
            {
                _simulator.Population.AddPerson(new Person(genderType));
            }
            return this;
        }

        public SimulatorBuilder AddPeople(IList<Person> people)
        {
            _simulator.Population.People = people;
            return this;
        }

        public SimulatorBuilder AddPerson(GenderType genderType)
        {
            _simulator.Population.AddPerson(new Person(genderType));
            return this;
        }

        public SimulatorBuilder AddReports(Dictionary<int, IReportEntry> reports)
        {
            _simulator.Report = reports;
            return this;
        }

        public SimulatorBuilder AddLogs(IList<Log> logs)
        {
            _simulator.Logs = logs;
            return this;
        }

        public SimulatorBuilder AddRandomNumberGenerator(Random random)
        {
            _simulator.Random = random;
            return this;
        }

        public Simulator Build()
        {
            return _simulator;
        }
    }
}
