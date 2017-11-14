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

        int slowGuests = 2;
        int minTime = 10;
        int maxTime = 20;

        List<string> NamesList = new List<string> { "James", "John", "Robert", "Michael", "William", "David", "Richard", "Joseph", "Thomas", "Charles", "Christopher", "Daniel", "Matthew", "Anthony", "Donald", "Mark", "Paul", "Steven", "Andrew", "Kenneth", "George", "Joshua", "Kevin", "Brian", "Edward", "Mary", "Patricia", "Jennifer", "Elizabeth", "Linda", "Barbara", "Susan", "Jessica", "Margaret", "Sarah", "Karen", "Nancy", "Betty", "Lisa", "Dorothy", "Otto den Store", "Rikard Lejonhjärta", "Sandra", "Ashley", "Kimberly", "Donna", "Carol", "Michelle", "Emily", "Amanda", "Helen", "Melissa" };
        Random Random = new Random();
        public delegate void Add_To_Listbox_Patron(String str);
        public delegate bool FirstInQueue(Patron patron);
        public delegate Chair Take_Chair();
        public delegate void Go_Home(Chair chair, Glass glass);
        public delegate bool Check_For_Available_Chair();

        public Patron(int index)
        {
            //int index = Random.Next(0, NamesList.Count - 1);  //Left over from previous build check for robustness of current build before removal
            Name = NamesList[index];
        }
        public void Drink(bool guestStaylonger, double speed, Add_To_Listbox_Patron add_To_Listbox_Patron, FirstInQueue firstInQueue, Take_Chair take_Chair, Go_Home go_Home, Check_For_Available_Chair check_For_Available_Chair)
        {
            double manipulator = Math.Round((1000d * speed), 0);
            int intManipulator = (int)manipulator;
            while (true)
            {
                Thread.Sleep(10);
                if (firstInQueue(this) && check_For_Available_Chair())
                {
                    Chair = take_Chair();
                    add_To_Listbox_Patron($"{Name} sits down and drinks the beer");
                    if (guestStaylonger)
                    {
                        Thread.Sleep(Random.Next((minTime * intManipulator*slowGuests), (maxTime * intManipulator*slowGuests)));
                    }
                    else
                    {
                        Thread.Sleep(Random.Next((minTime * intManipulator), (maxTime * intManipulator)));
                    }
                    add_To_Listbox_Patron($"{Name} finishes the beer and leaves the bar");
                    go_Home(Chair, Glass);
                    return;
                }
            }
        }

    }
}
