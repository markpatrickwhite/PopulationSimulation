using System;
using System.Collections.Generic;
using System.Linq;
using PopulationSimulation.Builders;
using PopulationSimulation.Interfaces;

namespace PopulationSimulation.Classes
{
    public class Simulator : ISimulator
    {
        public Simulator()
        {
            Population = new Population();
            Random = new Random();
            Report = new Dictionary<int, IReportEntry>();
            Logs = new List<Log>();
        }

        private int _monthOfSimulation;
        public readonly IPopulation Population;
        public Random Random { private get; set; }
        public Dictionary<int, IReportEntry> Report { get; set; }
        public IList<Log> Logs { get; set; }


        public void RunSimulation(int numberOfMonths)
        {
            _monthOfSimulation = 0;
            CreateReportEntry(0, 0, 0);
            while (_monthOfSimulation < numberOfMonths)
            {
                _monthOfSimulation++;
                ProcessSimulatedMonth();
            }
        }

        private void ProcessSimulatedMonth()
        {
            var peopleToProcess = Population.People.Where(p => p.IsAlive).ToList();

            var response = ProcessPeople(peopleToProcess);
            var deadPeopleCount = response.DeadPeople.Count;
            var marriedPeopleCount = response.NewCouples.Count * 2;
            var newBirthsCount = response.NewMothers.Count;

            DeliverNewBabies(response.NewMothers, peopleToProcess);
            UpdateMarriedPeopleInPopulation(peopleToProcess, response.NewCouples);
            Population.People = peopleToProcess.Where(p => p.IsAlive).ToList();

            CreateReportEntry(newBirthsCount, deadPeopleCount, marriedPeopleCount);
        }

        private ProcessSimulatedMonthResponse ProcessPeople(List<Person> peopleToProcess)
        {
            var processResponse = new ProcessSimulatedMonthResponse();
            var unmarriedPeople = peopleToProcess.Where(p => p.AgeType == AgeType.Adult && p.Spouse == null).ToList();

            foreach (var person in peopleToProcess)
            {
                ProcessPerson(person, processResponse, unmarriedPeople);
            }
            return processResponse;
        }

        private void ProcessPerson(Person person, ProcessSimulatedMonthResponse processResponse, ICollection<Person> unmarriedPeople)
        {
            person.Age = IncrementPersonAgeOneMonth(person.Age);
            if (KillPersonOnRandomChance(person, new Odds(Random, 1, 1000)))
            {
                person.IsAlive = false;
                processResponse.DeadPeople.Add(person);
            }
            var newCouple = MarryPersonOnRandomChance(unmarriedPeople, person, new Odds(Random, 2, 10));
            if (newCouple != null)
            {
                processResponse.NewCouples.Add(newCouple);
            }
            if (BirthOccursWhenPregnancyIsDue(person))
            {
                processResponse.NewMothers.Push(person);
            }
            AdvancePregnancyByOneMonth(person);
            if (PregnancyOccursOnRandomChanceForMarriedWomen(person, new Odds(Random, 3, 100)))
            {
                person.IsPregnant = true;
            }
        }

        private static void AdvancePregnancyByOneMonth(IPerson person)
        {
            if (person.IsPregnant) { person.PregnantTime++; }
        }

        private static bool BirthOccursWhenPregnancyIsDue(IPerson person)
        {
            var isPregnantWomanDueForDelivery = person.IsPregnant && person.PregnantTime >= 9;
            if (!isPregnantWomanDueForDelivery) return false;
            person.IsPregnant = false;
            person.PregnantTime = 0;
            return true;
        }

        private void DeliverNewBabies(Stack<Person> newMothers, ICollection<Person> peopleToProcess)
        {
            while (newMothers.Count > 0)
            {
                var mother = newMothers.Pop();
                var newborn = Random.Next(0, 2) != 0
                    ? new Person(GenderType.Female, 0, mother, mother.Spouse)
                    : new Person(GenderType.Male, 0, mother, mother.Spouse);
                peopleToProcess.Add(newborn);
                AddToLogs($"Congratulations to {mother.Name} and {mother.Spouse.Name} on their new baby: {newborn.Name}.");
            }
        }

        private static void UpdateMarriedPeopleInPopulation(List<Person> peopleToProcess, IEnumerable<Couple> newCouples)
        {
            foreach (var c in newCouples)
            {
                var Husband = peopleToProcess.FirstOrDefault(p => p == c.Husband);
                if (Husband != null) { Husband.Spouse = c.Wife as Person;}
                var Wife = peopleToProcess.FirstOrDefault(p => p == c.Wife);
                if (Wife != null) { Wife.Spouse = c.Husband as Person; }
            }
        }

        private bool PregnancyOccursOnRandomChanceForMarriedWomen(IPerson person, IOdds odds)
        {
            var isMarried = person.IsMarried && person.Spouse.IsAlive;
            var pregnancyFailsToOccur = person.IsMale || person.IsPregnant || !isMarried || !person.IsAdult || !odds.OccuranceHappens();
            if (pregnancyFailsToOccur) return false;
            AddToLogs($"{person.Name} became pregnant.");
            return true;
        }

        private Couple MarryPersonOnRandomChance(ICollection<Person> unmarried, Person person, IOdds odds)
        {
            var spouse = unmarried.FirstOrDefault(p => p != person && p.Gender != person.Gender);
            var marriageFailsToOccur = spouse == null || person.IsMarried || !person.IsAdult || !odds.OccuranceHappens();
            if (marriageFailsToOccur) return null;

            var newCouple = new Couple(person, spouse);
            unmarried.Remove(person);
            unmarried.Remove(spouse);
            AddToLogs($"Congratulations to newlyweds {person.Name} and {spouse.Name}.");
            return newCouple;
        }

        private void AddToLogs(string logText)
        {
            Logs.Add(new Log(_monthOfSimulation, logText));
        }

        private bool KillPersonOnRandomChance(IPerson person, IOdds odds)
        {
            if (!odds.OccuranceHappens()) return false;
            AddToLogs($"Let us please mourn the passing of {person.Name}.");
            return true;
        }

        public int IncrementPersonAgeOneMonth(int age)
        {
            return ++age;
        }

        private void CreateReportEntry(int births, int deaths, int marriages)
        {
            Report.Add(
                _monthOfSimulation,
                new ReportEntryBuilder().AddMonth(_monthOfSimulation)
                    .AddPopulation(Population)
                    .AddBirths(births)
                    .AddDeaths(deaths)
                    .AddMarriages(marriages)
                    .Build()
                );
        }
    }
}
