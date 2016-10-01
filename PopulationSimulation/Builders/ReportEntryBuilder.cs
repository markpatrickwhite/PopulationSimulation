using PopulationSimulation.Classes;
using PopulationSimulation.Interfaces;

namespace PopulationSimulation.Builders
{
    public class ReportEntryBuilder : IReportEntryBuilder
    {
        private readonly ReportEntry _reportEntry;

        public ReportEntryBuilder()
        {
            _reportEntry = new ReportEntry();
        }

        public ReportEntryBuilder AddMonth(int month)
        {
            if (month >= 0) { _reportEntry.Month = month; }
            return this;
        }

        public ReportEntryBuilder AddPopulation(IPopulation population)
        {
            _reportEntry.Count = population.Count;
            _reportEntry.ManCount = population.GetCountByGenderType(GenderType.Male);
            _reportEntry.WomanCount = population.GetCountByGenderType(GenderType.Female);
            _reportEntry.ChildCount = population.GetCountByAgeType(AgeType.Child);
            _reportEntry.AdultCount = population.GetCountByAgeType(AgeType.Adult);
            _reportEntry.SeniorCount = population.GetCountByAgeType(AgeType.Senior);
            return this;
        }

        public ReportEntryBuilder AddBirths(int births)
        {
            if (births >= 0) { _reportEntry.Births = births; }
            return this;
        }
        public ReportEntryBuilder AddDeaths(int deaths)
        {
            if (deaths >= 0) { _reportEntry.Deaths = deaths; }
            return this;
        }
        public ReportEntryBuilder AddMarriages(int marriages)
        {
            if (marriages >= 0) { _reportEntry.Marriages = marriages; }
            return this;
        }

        public ReportEntry Build()
        {
            var totalMenAndWomen = _reportEntry.ManCount + _reportEntry.WomanCount;
            var totalAgeGroups = _reportEntry.ChildCount +
                                 _reportEntry.AdultCount +
                                 _reportEntry.SeniorCount;
            var isCountsMatch = (totalAgeGroups == totalMenAndWomen &&
                                 totalMenAndWomen == _reportEntry.Count);
            return isCountsMatch ? _reportEntry : new ReportEntry();
        }
    }
}
