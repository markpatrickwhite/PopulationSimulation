using PopulationSimulation.Classes;

namespace PopulationSimulation.Interfaces
{
    public interface IPerson
    {
        GenderType Gender { get; set; }
        AgeType AgeType { get; }
        int Age { get; set; }
        string Name { get; }
        string FirstName { get; set; }
        string LastName { get; set; }
        bool IsAlive { get; set; }
        bool IsPregnant { get; set; }
        int PregnantTime { get; set; }
        Person Spouse { get; set; }
        Person BirthFather { get; set; }
        Person BirthMother { get; set; }
        bool IsAdult { get; }
        bool IsUnmarried { get; }
        bool IsMarried { get; }
        bool IsMale { get; }
        bool IsFemale { get; }
    }
}