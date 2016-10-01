using System;
using System.Collections.Generic;
using System.IO;

namespace PopulationSimulation
{
    public class Program
    {
        private const int LengthOfSimulationInMonths = 480; // 480 months = 40 years
        private const string ReportFilename = "out.txt";
        private static Random _randomNumber;
        private static Dictionary<int, IReportEntry> _report;
        private static IList<Log> _logs;
        private static StreamWriter _fw;
        private static readonly int StartingPopulationCount = 25; // yields 25m + 25f
        private static Simulator _simulator;

        public Program()
        {
            _randomNumber = new Random();
            _report = new Dictionary<int, IReportEntry>();
            _logs = new List<Log>();
            _fw = new StreamWriter(ReportFilename, false);
        }

        public static void Main(string[] args)
        {
            var program = new Program();
            //TODO: replace with IoC
            _simulator = new SimulatorBuilder()
                                            .AddLogs(_logs)
                                            .AddReports(_report)
                                            .AddStarterPeople(GenderType.Male, StartingPopulationCount)
                                            .AddStarterPeople(GenderType.Female, StartingPopulationCount)
                                            .AddRandomNumberGenerator(_randomNumber)
                                            .Build();

            _simulator.RunSimulation(LengthOfSimulationInMonths);
            DisplayReport(_simulator, _fw);

            Console.ReadKey();
        }

        private static void DisplayReport(Simulator simulator, StreamWriter fw)
        {
            Display(fw, simulator);
            fw.Close();
        }

        public static void WriteToConsole(string t) { Console.WriteLine(t); }
        public static void WriteToFile(StreamWriter fw, string t) { fw.WriteLine(t); }
        public static void WriteToOutput(StreamWriter fw, string t) { WriteToConsole(t); WriteToFile(fw, t); }
        public static void Display(StreamWriter fw, Simulator simulator)
        {
            WriteToOutput(fw, "Population Report::");
            WriteToOutput(fw, "--------------------------");
            WriteToOutput(fw, "Month\tCount\tMen\tWomen\tBirths\tDeaths\tMarried\tKids\tAdults\tElderly");
            foreach (var item in simulator.Report)
            {
                var r = item.Value;
                string reportOutput =
                    $"{r.Month}\t{r.Count}\t{r.ManCount}\t{r.WomanCount}\t{r.Births}\t{r.Deaths}\t{r.Marriages}\t{r.ChildCount}\t{r.AdultCount}\t{r.SeniorCount}";
                WriteToOutput(fw, reportOutput);
            }

            WriteToOutput(fw, "");
            WriteToOutput(fw, "Age Breakdown::");
            WriteToOutput(fw, "--------------------------");
            WriteToOutput(fw, $"Children: \t{simulator.Population.GetCountByAgeType(AgeType.Child)}");
            WriteToOutput(fw, $"Adults: \t{simulator.Population.GetCountByAgeType(AgeType.Adult)}");
            WriteToOutput(fw, $"Seniors: \t{simulator.Population.GetCountByAgeType(AgeType.Senior)}");

            WriteToOutput(fw, "");
            WriteToOutput(fw, "Event Log::");
            WriteToOutput(fw, "--------------------------");
            foreach (var log in simulator.Logs) { WriteToOutput(fw, $"Month {log.Month}: {log.Text}"); }

            WriteToOutput(fw, "");
            WriteToOutput(fw, "_people::");
            WriteToOutput(fw, "--------------------------");
            WriteToOutput(fw, "FName|LName|Gender|AgeType|Age|Married?|Father|Mother");
            foreach (var person in simulator.Population.People)
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
