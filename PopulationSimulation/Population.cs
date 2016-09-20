using System;
using System.Collections.Generic;
using System.Linq;

namespace PopulationSimulation
{
    public class Population : IPopulation
    {
        public Population(
            Random random,
            IList<Person> people,
            Dictionary<int, IReportEntry> report,
            IList<Log> logs)
        {
            Random = random;
            People = people;
            Report = report;
            Logs = logs;
        }

        public Population()
        {
            People = new List<Person>();
            Random = new Random();
            Report = new Dictionary<int, IReportEntry>();
            Logs = new List<Log>();
        }

        public Random Random { get; set; }
        public IList<Person> People { get; set; }
        public Dictionary<int, IReportEntry> Report { get; set; }
        public IList<Log> Logs { get; set; }

        public int Count => People.Count;

        public int GetCountByGenderType(GenderType g)
        {
            var peopleOfGender = People.Where(p => p.Gender == g);
            if (peopleOfGender == null) { return 0;}
            return peopleOfGender.Count();
        }

        public int GetCountByAgeType(AgeType a) { return People.Count(p => p.AgeType == a); }
        public void AddPerson(Person p) { People.Add(p); }

        public void Initialize()
        {
            Report.Add(
                0,
                new ReportEntryBuilder().AddMonth(0)
                                        .AddManCount(GetCountByGenderType(GenderType.Male))
                                        .AddWomanCount(GetCountByGenderType(GenderType.Female))
                                        .AddBirths(0)
                                        .AddDeaths(0)
                                        .AddMarriages(0)
                                        .Build()
            );
        }

        public void Process(int month)
        {
            var peopleToProcess = People.ToList();
            var newCount = peopleToProcess.Count;
            var originalCount = People.Count;

            peopleToProcess.Add(new Person());

            newCount = peopleToProcess.Count;
            originalCount = People.Count;


            var newMothers = new Stack<Person>();
            var deadPeople = new List<Person>();
            var newSpouses = new Dictionary<Person, Person>();
            var unmarried = People.Where(p => p.AgeType == AgeType.Adult).Where(p => p.Spouse == null).ToList();

            foreach (var person in People)
            {
                if (!person.IsAlive)
                {
                    // don't process the dead
                    continue;
                }

                person.Age++; // age the person

                // random 0.1% chance to kill the person
                if (Random.Next(0, 1000) <= 1)
                {
                    person.IsAlive = false;
                    deadPeople.Add(person);
                    Logs.Add(new Log(month, $"Let us please mourn the passing of {person.Name}."));
                }


                // random chance for unmarried people to get married
                var isPersonAdult = person.AgeType == AgeType.Adult;
                var isPersonUnmarried = person.Spouse == null;
                if (isPersonAdult && isPersonUnmarried)
                {
                    if (Random.Next(0, 10) < 2)
                    {
                        foreach (var spouseCandidate in unmarried)
                        {
                            // Potential to add more possibilities
                            if (person == spouseCandidate ||
                                person.Gender == spouseCandidate.Gender ||
                                spouseCandidate.Spouse != null) { continue; }
                            newSpouses.Add(person, spouseCandidate);
                            newSpouses.Add(spouseCandidate, person);
                            spouseCandidate.Spouse = person;
                            person.Spouse = spouseCandidate;
                            unmarried.Remove(person);
                            unmarried.Remove(spouseCandidate);
                            Logs.Add(new Log(month,
                                $"Congratulations to newlyweds {person.Name} and {spouseCandidate.Name}."));
                            break;
                        }
                    }
                }

                // handle pregnancies
                var isChildBearingAge = isPersonAdult;
                var isMarried = person.Spouse != null && person.Spouse.IsAlive;
                var isPersonMale = person.Gender == GenderType.Male;
                if (isPersonMale) continue;
                var isPregnantWomanDueForDelivery = person.IsPregnant && person.PregnantTime >= 9;
                if (isPregnantWomanDueForDelivery)
                {
                    person.IsPregnant = false;
                    person.PregnantTime = 0;
                    newMothers.Push(person);
                }
                // if pregnant and not due
                else if (person.IsPregnant) { person.PregnantTime++; }
                // random 3% chance of pregnancy  (Married Adults only, for simplicity)
                else if (!person.IsPregnant && isMarried && isChildBearingAge && Random.Next(0, 100) < 3)
                {
                    person.IsPregnant = true;
                    Logs.Add(new Log(month, $"{person.Name} became pregnant."));
                }
            }

            // new people are born!
            var births = newMothers.Count;
            while (newMothers.Count > 0)
            {
                var mother = newMothers.Pop();
                var newborn = Random.Next(0, 2) != 0 ? new Person(GenderType.Female, 0, mother, mother.Spouse) : new Person(GenderType.Male, 0, mother, mother.Spouse);
                People.Add(newborn);
                Logs.Add(new Log(month,
                    $"Congratulations to {mother.Name} and {mother.Spouse.Name} on their new baby: {newborn.Name}."));
            }

            // people got married
            var marriages = newSpouses.Count;
            foreach (var p in People)
            {
                if (!newSpouses.ContainsKey(p)) continue;
                p.Spouse = newSpouses[p];
                break;
            }

            // bury the dead
            var deaths = deadPeople.Count;
            People = People.Where(p => p.IsAlive).ToList();
            Report.Add(
                0,
                new ReportEntryBuilder().AddMonth(month)
                                        .AddManCount(GetCountByGenderType(GenderType.Male))
                                        .AddWomanCount(GetCountByGenderType(GenderType.Female))
                                        .AddBirths(births)
                                        .AddDeaths(deaths)
                                        .AddMarriages(marriages)
                                        .AddChildren(GetCountByAgeType(AgeType.Child))
                                        .AddAdults(GetCountByAgeType(AgeType.Child))
                                        .AddSeniors(GetCountByAgeType(AgeType.Child))
                                        .Build()
            );
        }
    }
}
