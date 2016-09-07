namespace PopulationSimulation
{
    public interface IReportEntryBuilder
    {
        ReportEntryBuilder AddAdults(int adults);
        ReportEntryBuilder AddBirths(int births);
        ReportEntryBuilder AddChildren(int children);
        ReportEntryBuilder AddDeaths(int deaths);
        ReportEntryBuilder AddManCount(int men);
        ReportEntryBuilder AddMarriages(int marriages);
        ReportEntryBuilder AddMonth(int month);
        ReportEntryBuilder AddSeniors(int seniors);
        ReportEntryBuilder AddWomanCount(int women);
        ReportEntry Build();
    }
}