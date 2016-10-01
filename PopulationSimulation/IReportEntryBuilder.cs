﻿namespace PopulationSimulation
{
    public interface IReportEntryBuilder
    {
        ReportEntryBuilder AddMonth(int month);
        ReportEntryBuilder AddBirths(int births);
        ReportEntryBuilder AddDeaths(int deaths);
        ReportEntryBuilder AddMarriages(int marriages);
        ReportEntryBuilder AddPopulation(IPopulation population);
        ReportEntry Build();
    }
}