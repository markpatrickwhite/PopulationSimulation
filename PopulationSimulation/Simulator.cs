using System;
using System.Collections.Generic;
using System.Linq;

namespace PopulationSimulation
{
    public class Simulator
    {
        public Simulator()
        {
            Population = new Population();
            Random = new Random();
            Report = new Dictionary<int, IReportEntry>();
            Logs = new List<Log>();
        }

        public IPopulation Population;
        public Random Random { get; set; }
        public Dictionary<int, IReportEntry> Report { get; set; }
        public IList<Log> Logs { get; set; }


        public void RunSimulation(int numberOfMonths)
        {
            CreateReportEntry(0, 0, 0, 0);
            for (var s = 0; s < numberOfMonths; s++)
            {
                ProcessSimulatedMonth(s + 1);
            }
        }
        public void ProcessSimulatedMonth(int month)
        {
            var peopleToProcess = Population.People.Where(p => p.IsAlive).ToList();

            var deadPeople = new List<Person>();
            var newMothers = new Stack<Person>();
            var newSpouses = new Dictionary<Person, Person>();

            var unmarriedPeople = peopleToProcess.Where(p => p.AgeType == AgeType.Adult && p.Spouse == null).ToList();

            foreach (var person in peopleToProcess)
            {
                AgePersonOneMonth(person);
                KillPersonOnRandomChance(month, person, deadPeople, new Odds(1, 1000));
                MarryPersonOnRandomChance(month, unmarriedPeople, person, newSpouses, new Odds(2, 10));
                PregnancyOccursOnRandomChanceForMarriedWomen(month, person, newMothers, new Odds(3, 100));
            }
            DeliverNewBabies(month, newMothers, peopleToProcess);
            UpdateMarriedPeopleInPopulation(peopleToProcess, newSpouses);
            Population.People = peopleToProcess.Where(p => p.IsAlive).ToList();

            CreateReportEntry(month, newMothers.Count, deadPeople.Count, newSpouses.Count / 2);
        }

        private void DeliverNewBabies(int month, Stack<Person> newMothers, List<Person> peopleToProcess)
        {
            while (newMothers.Count > 0)
            {
                var mother = newMothers.Pop();
                var newborn = Random.Next(0, 2) != 0
                    ? new Person(GenderType.Female, 0, mother, mother.Spouse)
                    : new Person(GenderType.Male, 0, mother, mother.Spouse);
                peopleToProcess.Add(newborn);
                Logs.Add(new Log(month,
                    $"Congratulations to {mother.Name} and {mother.Spouse.Name} on their new baby: {newborn.Name}."));
            }
        }

        private static void UpdateMarriedPeopleInPopulation(List<Person> peopleToProcess, Dictionary<Person, Person> newSpouses)
        {
            foreach (var p in peopleToProcess.Where(newSpouses.ContainsKey)) { p.Spouse = newSpouses[p]; }
        }

        private void PregnancyOccursOnRandomChanceForMarriedWomen(int month, Person person, Stack<Person> newMothers, Odds odds)
        {
            var isPersonAdult = person.AgeType == AgeType.Adult;
            var isChildBearingAge = isPersonAdult;
            var isMarried = person.Spouse != null && person.Spouse.IsAlive;
            var isPersonMale = person.Gender == GenderType.Male;
            if (isPersonMale) return;
            var isPregnantWomanDueForDelivery = person.IsPregnant && person.PregnantTime >= 9;
            if (isPregnantWomanDueForDelivery)
            {
                person.IsPregnant = false;
                person.PregnantTime = 0;
                newMothers.Push(person);
            }
            // if pregnant and not due
            else if (person.IsPregnant)
            {
                person.PregnantTime++;
            }
            // random 3% chance of pregnancy  (Married Adults only, for simplicity)
            else if (!person.IsPregnant && isMarried && isChildBearingAge && Random.Next(0, odds.Denominator) < odds.Numerator)
            {
                person.IsPregnant = true;
                Logs.Add(new Log(month, $"{person.Name} became pregnant."));
            }
        }

        private void MarryPersonOnRandomChance(int month,
            List<Person> unmarried, Person person, Dictionary<Person, Person> newSpouses, Odds marriageOdds)
        {
            var isPersonAdult = person.AgeType == AgeType.Adult;
            var isPersonUnmarried = person.Spouse == null;
            var oddsOfGettingMarried = Random.Next(0, marriageOdds.Denominator) < marriageOdds.Numerator;

            if (!isPersonAdult || !isPersonUnmarried || !oddsOfGettingMarried) return;
            var spouse = unmarried.FirstOrDefault(p => p != person && p.Gender != person.Gender);
            if (spouse == null) return;
            newSpouses.Add(person, spouse);
            newSpouses.Add(spouse, person);
            spouse.Spouse = person;
            person.Spouse = spouse;
            unmarried.Remove(person);
            unmarried.Remove(spouse);
            Logs.Add(new Log(month, $"Congratulations to newlyweds {person.Name} and {spouse.Name}."));
        }

        private void KillPersonOnRandomChance(int month, Person person, List<Person> deadPeople, Odds odds)
        {
            if (OccuranceHappens(odds))
            {
                person.IsAlive = false;
                deadPeople.Add(person);
                Logs.Add(new Log(month, $"Let us please mourn the passing of {person.Name}."));
            }
        }

        private bool OccuranceHappens(Odds odds)
        {
            return Random.Next(0, odds.Denominator) <= odds.Numerator;
        }

        private static void AgePersonOneMonth(Person person)
        {
            person.Age++; // age the person
        }

        private void CreateReportEntry(int month, int births, int deaths, int marriages)
        {
            Report.Add(
                month,
                new ReportEntryBuilder().AddMonth(month)
                    .AddPopulation(Population)
                    .AddBirths(births)
                    .AddDeaths(deaths)
                    .AddMarriages(marriages)
                    .Build()
                );
        }
    }
}
