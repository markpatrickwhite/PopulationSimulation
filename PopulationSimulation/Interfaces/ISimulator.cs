using System;
using System.Collections.Generic;
using PopulationSimulation.Classes;

namespace PopulationSimulation.Interfaces
{
    public interface ISimulator
    {
        IList<Log> Logs { get; set; }
        Random Random { set; }
        Dictionary<int, IReportEntry> Report { get; set; }

        int IncrementPersonAgeOneMonth(int age);
        void RunSimulation(int numberOfMonths);
    }
}