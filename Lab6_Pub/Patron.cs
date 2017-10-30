using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab6_Pub
{
    class Patron
    {
        public String Name;
        List<string> NamesList = new List<string> { "James", "John", "Robert", "Michael", "William", "David", "Richard", "Joseph", "Thomas", "Charles", "Christopher", "Daniel", "Matthew", "Anthony", "Donald", "Mark", "Paul", "Steven", "Andrew", "Kenneth", "George", "Joshua", "Kevin", "Brian", "Edward", "Mary", "Patricia", "Jennifer", "Elizabeth", "Linda", "Barbara", "Susan", "Jessica", "Margaret", "Sarah", "Karen", "Nancy", "Betty", "Lisa", "Dorothy", "Sandra", "Ashley", "Kimberly", "Donna", "Carol", "Michelle", "Emily", "Amanda", "Helen", "Melissa" };
        Random Random = new Random();
        public Patron()
        {
            int index = Random.Next(0, NamesList.Count - 1);
            Name = NamesList[index];
        }
    }
}
