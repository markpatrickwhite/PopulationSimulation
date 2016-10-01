using System;
using System.Collections.Generic;
using System.IO;

namespace PopulationSimulation
{
    public class Program
    {
        private const int LengthOfSimulationInMonths = 480; // 40 years
        private const string ReportFilename = "out.txt";
        private static Random _randomNumber;
        private static Dictionary<int, IReportEntry> _report;
        private static IList<Log> _logs;
        private static StreamWriter _fw;
        private static readonly int StartingPopulationCount = 25;

        public static void Main(string[] args)
        {
            //TODO: replace with IoC
            InitializeComponents();

            var population = new PopulationBuilder()
                                            .AddLogs(_logs)
                                            .AddReports(_report)
                                            .AddStarterPeople(GenderType.Male, StartingPopulationCount)
                                            .AddStarterPeople(GenderType.Female, StartingPopulationCount)
                                            .AddRandomNumberGenerator(_randomNumber)
                                            .Build();

            population.Initialize();
            for (var s = 0; s < LengthOfSimulationInMonths; s++)
            {
                population.Process(s + 1);
            }
            DisplayReport(population, _fw);

            Console.ReadKey();
        }

        private static void InitializeComponents()
        {
            _randomNumber = new Random();
            //_people = new List<Person>();
            _report = new Dictionary<int, IReportEntry>();
            _logs = new List<Log>();
            _fw = new StreamWriter(ReportFilename, false);
        }

        private static void DisplayReport(Population population, StreamWriter fw)
        {
            Display(fw, population);
            fw.Close();
        }

        public static void WriteToConsole(string t) { Console.WriteLine(t); }
        public static void WriteToFile(StreamWriter fw, string t) { fw.WriteLine(t); }
        public static void WriteToOutput(StreamWriter fw, string t) { WriteToConsole(t); WriteToFile(fw, t); }
        public static void Display(StreamWriter fw, Population p)
        {
            WriteToOutput(fw, "Population Report::");
            WriteToOutput(fw, "--------------------------");
            WriteToOutput(fw, "Month\tCount\tMen\tWomen\tBirths\tDeaths\tMarried\tKids\tAdults\tElderly");
            foreach (var item in p.Report)
            {
                var r = item.Value;
                string reportOutput =
                    $"{r.Month}\t{r.Count}\t{r.ManCount}\t{r.WomanCount}\t{r.Births}\t{r.Deaths}\t{r.Marriages}\t{r.ChildCount}\t{r.AdultCount}\t{r.SeniorCount}";
                WriteToOutput(fw, reportOutput);
            }

            WriteToOutput(fw, "");
            WriteToOutput(fw, "Age Breakdown::");
            WriteToOutput(fw, "--------------------------");
            WriteToOutput(fw, $"Children: \t{p.GetCountByAgeType(AgeType.Child)}");
            WriteToOutput(fw, $"Adults: \t{p.GetCountByAgeType(AgeType.Adult)}");
            WriteToOutput(fw, $"Seniors: \t{p.GetCountByAgeType(AgeType.Senior)}");

            WriteToOutput(fw, "");
            WriteToOutput(fw, "Event Log::");
            WriteToOutput(fw, "--------------------------");
            foreach (var log in p.Logs) { WriteToOutput(fw, $"Month {log.Month}: {log.Text}"); }

            WriteToOutput(fw, "");
            WriteToOutput(fw, "_people::");
            WriteToOutput(fw, "--------------------------");
            WriteToOutput(fw, "FName|LName|Gender|AgeType|Age|Married?|Father|Mother");
            foreach (var person in p.People)
            {
                var personGender = person.Gender == GenderType.Male ? "M" : "F";
                var spouseInfo = (person.Spouse != null ? "married to " + person.Spouse.Name : "unmarried");
                var fatherName = person.BirthFather != null ? person.BirthFather.Name : "Unknown";
                var motherName = person.BirthMother != null ? person.BirthMother.Name : "Unknown";
                WriteToOutput(fw,
                    $"{person.FirstName}|{person.LastName}|{personGender}|{person.AgeType}|{person.Age / 12}|{spouseInfo}|{fatherName}|{motherName}");
            }

            WriteToOutput(fw, "-------------------------- /END");
        }
    }
}
