using System;
using System.Collections.Generic;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PopulationSimulation.Builders;
using PopulationSimulation.Classes;
using PopulationSimulation.Interfaces;

namespace PopulationSimulation.UnitTests
{
    [TestClass]
    public class SimulatorTests
    {
        [TestMethod]
        public void AgePersonOneMonth_ShouldSetPersonAgeTo100_WhenPersonAgeIs99()
        {
            var person = new Person() {Age = 99};
            var simulator = new SimulatorBuilder()
                                            .AddLogs(new List<Log>())
                                            .AddReports(new Dictionary<int, IReportEntry>())
                                            .AddStarterPeople(GenderType.Male, 1)
                                            .AddStarterPeople(GenderType.Female, 1)
                                            .AddRandomNumberGenerator(new Random())
                                            .Build();
            var sut = person.Age;
            sut = simulator.IncrementPersonAgeOneMonth(sut);
            sut.Should().Be(100);
        }
    }
}
