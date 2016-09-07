using System;
using System.Collections.Generic;

namespace PopulationSimulation
{
    public class PopulationBuilder
    {
        private readonly Population _population;
        public PopulationBuilder()
        {
            _population = new Population();
        }

        public PopulationBuilder AddStarterPeople(GenderType genderType, int count)
        {
            while (_population.GetCountByGenderType(genderType) < count)
            {
                _population.AddPerson(new Person(genderType));
            }
            return this;
        }

        public PopulationBuilder AddPeople(IList<Person> people)
        {
            _population.People = people;
            return this;
        }

        public PopulationBuilder AddPerson(GenderType genderType)
        {
            _population.AddPerson(new Person(genderType));
            return this;
        }

        public PopulationBuilder AddReports(Dictionary<int, IReportEntry> reports)
        {
            _population.Report = reports;
            return this;
        }

        public PopulationBuilder AddLogs(IList<Log> logs)
        {
            _population.Logs = logs;
            return this;
        }

        public PopulationBuilder AddRandomNumberGenerator(Random random)
        {
            _population.Random = random;
            return this;
        }

        public Population Build()
        {
            return _population;
        }
    }
}
