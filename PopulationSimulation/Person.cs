using System;
using System.Linq;

namespace PopulationSimulation
{
    public class Person : IPerson
    {
        private static readonly Random Random = new Random();
        public Person()
        {
            Gender = GenderType.Male;
            Age = 300;
            FirstName = GenerateRandomFirstName(Gender);
            LastName = GenerateRandomLastName();
            BirthFather = null;
            BirthMother = null;

            IsAlive = true;
            IsPregnant = false;
            PregnantTime = 0;
        }
        public Person(GenderType g, int a)
        {
            Gender = g;
            Age = a;
            FirstName = GenerateRandomFirstName(Gender);
            LastName = GenerateRandomLastName();
            BirthFather = null;
            BirthMother = null;

            IsAlive = true;
            IsPregnant = false;
            PregnantTime = 0;
        }
        public Person(int a)
        {
            Gender = GenderType.Male;
            Age = a;
            FirstName = GenerateRandomFirstName(Gender);
            LastName = GenerateRandomLastName();
            BirthFather = null;
            BirthMother = null;

            IsAlive = true;
            IsPregnant = false;
            PregnantTime = 0;
        }
        public Person(GenderType g)
        {
            Gender = g;
            Age = 300;
            FirstName = GenerateRandomFirstName(Gender);
            LastName = GenerateRandomLastName();
            BirthFather = null;
            BirthMother = null;

            IsAlive = true;
            IsPregnant = false;
            PregnantTime = 0;
        }
        public Person(GenderType g, int a, Person mother, Person father)
        {
            Gender = g;
            Age = a;
            FirstName = GenerateRandomFirstName(Gender);
            LastName = GenerateFixedLastName(father);
            BirthFather = father;
            BirthMother = mother;

            IsAlive = true;
            IsPregnant = false;
            PregnantTime = 0;
        }

        public GenderType Gender { get; set; }
        public AgeType AgeType
        {
            get
            {
                if (Age < 216) return AgeType.Child; // 216 months = 18 years
                else if (Age > 779) return AgeType.Senior; // 780 months = 65 years
                else return AgeType.Adult;
            }
        }
        public int Age { get; set; }
        public string Name { get { return string.Format("{0} {1}", FirstName, LastName); } }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public bool IsAlive { get; set; }
        public bool IsPregnant { get; set; }
        public int PregnantTime { get; set; }

        public Person Spouse { get; set; }
        public Person BirthFather { get; set; }
        public Person BirthMother { get; set; }

        private string GenerateRandomFirstName(GenderType g)
        {
            string[] nameList;
            switch (g)
            {
                case GenderType.Male:
                    nameList = Util.MaleNames;
                    break;
                case GenderType.Female:
                    nameList = Util.FemaleNames;
                    break;
                default:
                    return "Unknown";
            }
            string name = nameList[Random.Next(0, nameList.Count())];
            return name;
            //return NameList[_random.Next(0, NameList.Count())];
        }
        private string GenerateRandomLastName()
        {
            // Potential to add more possibilities
            return Util.LastNames[Random.Next(0, Util.LastNames.Count())];
        }
        private string GenerateFixedLastName(Person father)
        {
            try
            {
                // Potential to add more possibilities
                return father != null ? father.LastName : GenerateRandomLastName();
            }
            catch (Exception)
            {
                return "Error";
            }
            
        }
    }
}
