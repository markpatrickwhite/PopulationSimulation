using System.Collections.Generic;

namespace PopulationSimulation
{
    public interface IPopulation
    {
        int Count { get; }
        IList<Log> Logs { get; }
        IList<Person> People { get; set; }
        Dictionary<int, IReportEntry> Report { get; }

        void AddPerson(Person p);
        int GetCountByGenderType(GenderType g);
        int GetCountByAgeType(AgeType a);
        void Initialize();
        void Process(int month);
    }
}