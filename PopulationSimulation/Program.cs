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
                var reportOutput =
                    string.Format("{0}\t{1}\t{2}\t{3}\t{4}\t{5}\t{6}\t{7}\t{8}\t{9}", 
                    r.Month, r.Count, r.ManCount, r.WomanCount, r.Births, r.Deaths, r.Marriages, r.ChildCount, r.AdultCount, r.SeniorCount);
                //string reportOutput =
                //    $"{r.Month}\t{r.Count}\t{r.ManCount}\t{r.WomanCount}\t{r.Births}\t{r.Deaths}\t{r.Marriages}\t{r.ChildCount}\t{r.AdultCount}\t{r.SeniorCount}";
                WriteToOutput(fw, reportOutput);
            }

            WriteToOutput(fw, "");
            WriteToOutput(fw, "Age Breakdown::");
            WriteToOutput(fw, "--------------------------");
            WriteToOutput(fw, string.Format("Children: \t{0}", p.GetCountByAgeType(AgeType.Child)));
            WriteToOutput(fw, string.Format("Adults: \t{0}", p.GetCountByAgeType(AgeType.Adult)));
            WriteToOutput(fw, string.Format("Seniors: \t{0}",p.GetCountByAgeType(AgeType.Senior)));
            //WriteToOutput(fw, $"Children: \t{p.GetCountByAgeType(AgeType.Child)}"); // C#6
            //WriteToOutput(fw, $"Adults: \t{p.GetCountByAgeType(AgeType.Adult)}"); // C#6
            //WriteToOutput(fw, $"Seniors: \t{p.GetCountByAgeType(AgeType.Senior)}"); // C#6

            WriteToOutput(fw, "");
            WriteToOutput(fw, "Event Log::");
            WriteToOutput(fw, "--------------------------");
            foreach (var log in p.Logs) { WriteToOutput(fw, string.Format("Month {0}: {1}", log.Month, log.Text)); }
            //foreach (var log in p.Logs) { WriteToOutput(fw, $"Month {log.Month}: {log.Text}"); } //C#6

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
                    string.Format("{0}|{1}|{2}|{3}|{4}|{5}|{6}|{7}",
                    person.FirstName, person.LastName, personGender, person.AgeType, person.Age / 12, spouseInfo, fatherName, motherName
                    ));
                //WriteToOutput(fw,
                //    $"{person.FirstName}|{person.LastName}|{personGender}|{person.AgeType}|{person.Age / 12}|{spouseInfo}|{fatherName}|{motherName}"); //C#6
            }

            WriteToOutput(fw, "-------------------------- /END");
        }
    }
}
