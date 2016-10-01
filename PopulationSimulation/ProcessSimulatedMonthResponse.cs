using System.Collections.Generic;
using PopulationSimulation.Classes;

namespace PopulationSimulation
{
    public class ProcessSimulatedMonthResponse
    {
        public ProcessSimulatedMonthResponse()
        {
            DeadPeople = new List<Person>();
            NewMothers = new Stack<Person>();
            NewCouples = new List<Couple>();
        }

        public ProcessSimulatedMonthResponse(
            List<Person> deadPeople,
            Stack<Person> newMothers,
            List<Couple> newCouples)
        {
            DeadPeople = deadPeople;
            NewMothers = newMothers;
            NewCouples = newCouples;
        }

        public List<Person> DeadPeople { get; private set; }
        public Stack<Person> NewMothers { get; private set; }
        public List<Couple> NewCouples { get; private set; }
    }
}