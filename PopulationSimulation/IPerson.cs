namespace PopulationSimulation
{
    public interface IPerson
    {
        int Age { get; set; }
        AgeType AgeType { get; }
        Person BirthFather { get; set; }
        Person BirthMother { get; set; }
        string FirstName { get; set; }
        GenderType Gender { get; set; }
        bool IsAlive { get; set; }
        bool IsPregnant { get; set; }
        string LastName { get; set; }
        string Name { get; }
        int PregnantTime { get; set; }
        Person Spouse { get; set; }
    }
}