namespace PopulationSimulation
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

        public ReportEntryBuilder AddManCount(int men)
        {
            if (men >= 0)
            {
                _reportEntry.ManCount = men;
                _reportEntry.Count += men;
            }
            return this;
        }
        public ReportEntryBuilder AddWomanCount(int women)
        {
            if (women >= 0)
            {
                _reportEntry.WomanCount = women;
                _reportEntry.Count += women;
            }
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

        public ReportEntryBuilder AddChildren(int children)
        {
            if (children >= 0) { _reportEntry.ChildCount = children; }
            return this;
        }
        public ReportEntryBuilder AddAdults(int adults)
        {
            if (adults >= 0) { _reportEntry.AdultCount = adults; }
            return this;
        }
        public ReportEntryBuilder AddSeniors(int seniors)
        {
            if (seniors >= 0) { _reportEntry.SeniorCount = seniors; }
            return this;
        }

        public ReportEntry Build()
        {
            var totalMenAndWomen = _reportEntry.ManCount + _reportEntry.WomanCount;
            var totalAgeGroups = _reportEntry.ChildCount +
                                 _reportEntry.AdultCount +
                                 _reportEntry.SeniorCount;
            var isCountsMatch = (totalAgeGroups != totalMenAndWomen ||
                                 totalMenAndWomen != _reportEntry.Count);
            return isCountsMatch ? new ReportEntry() : _reportEntry;
        }
    }
}
