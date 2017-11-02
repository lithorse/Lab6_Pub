using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Lab6_Pub
{
    public class Patron
    {
        public String Name;
        public Chair Chair;
        public Glass Glass;
        List<string> NamesList = new List<string> { "James", "John", "Robert", "Michael", "William", "David", "Richard", "Joseph", "Thomas", "Charles", "Christopher", "Daniel", "Matthew", "Anthony", "Donald", "Mark", "Paul", "Steven", "Andrew", "Kenneth", "George", "Joshua", "Kevin", "Brian", "Edward", "Mary", "Patricia", "Jennifer", "Elizabeth", "Linda", "Barbara", "Susan", "Jessica", "Margaret", "Sarah", "Karen", "Nancy", "Betty", "Lisa", "Dorothy", "Sandra", "Ashley", "Kimberly", "Donna", "Carol", "Michelle", "Emily", "Amanda", "Helen", "Melissa" };
        Random Random = new Random();
        public delegate void Add_To_Listbox_Patron(String str);
        public delegate bool FirstInQueue(Patron patron);
        public delegate Chair Take_Chair();
        public delegate void Go_Home(Chair chair, Glass glass);

        public Patron()
        {
            int index = Random.Next(0, NamesList.Count - 1);
            Name = NamesList[index];
        }
        public void Drink(Add_To_Listbox_Patron add_To_Listbox_Patron, FirstInQueue firstInQueue, Take_Chair take_Chair, Go_Home go_Home)
        {
            while (true)
            {
                Thread.Sleep(10);
                if (firstInQueue(this))
                {
                    add_To_Listbox_Patron($"{Name} sits down and drinks the beer");
                    Chair = take_Chair();
                    Thread.Sleep(Random.Next(10000, 20000));
                    add_To_Listbox_Patron($"{Name} finishes the beer and leaves the bar");
                    go_Home(Chair, Glass);
                    return;
                }
            }
        }

    }
}
