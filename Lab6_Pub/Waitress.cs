using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Lab6_Pub
{
    class Waitress
    {
        bool pubOpen = true;
        List<Glass> glassList = new List<Glass>();

        int timeGetDirtyGlasses = 10;
        int timeCleanDirtyGlasses = 15;


        public void On_Close()
        {
            pubOpen = false;
        }

        public delegate Glass Get_Dirty_Glass_Delegate();
        public delegate bool Check_Dirty_Glasses_Delegate();
        public delegate bool Get_Patrons_Count_Delegate();
        public delegate void Place_Clean_Glass_Delegate(Glass glass);
        public delegate void Listbox_Add_Delegate(string str);

        public void WaitressWork(double speed, Listbox_Add_Delegate listbox_Add_Delegate, Get_Dirty_Glass_Delegate get_Dirty_Glass_Delegate, Get_Patrons_Count_Delegate get_Patrons_Count_Delegate, Place_Clean_Glass_Delegate place_Clean_Glass_Delegate, Check_Dirty_Glasses_Delegate check_Dirty_Glasses_Delegate)
        {

            listbox_Add_Delegate("The waitress is awaiting dirty glasses");
            double manipulator = Math.Round((1000d * speed), 0);
            int intManipulator = (int)manipulator;

            while (pubOpen || get_Patrons_Count_Delegate())
            {
                Thread.Sleep(10);
                if (!check_Dirty_Glasses_Delegate())
                {
                    if (!pubOpen && !get_Patrons_Count_Delegate())
                    {
                        break;
                    }

                    listbox_Add_Delegate("The waitress gets the dirty glasses");
                    Thread.Sleep(timeGetDirtyGlasses * intManipulator);

                    while (!check_Dirty_Glasses_Delegate())
                    {
                        glassList.Add(get_Dirty_Glass_Delegate());
                    }

                    if (glassList.Count() == 1)
                    {
                        listbox_Add_Delegate("The waitress cleans 1 glass");
                    }
                    else
                    {
                        listbox_Add_Delegate($"The waitress cleans {glassList.Count()} glasses");
                    }
                    Thread.Sleep(timeCleanDirtyGlasses * intManipulator);

                    if (glassList.Count() == 1)
                    {
                        listbox_Add_Delegate("The waitress places 1 clean glass on the shelf");
                    }
                    else
                    {
                        listbox_Add_Delegate($"The waitress places {glassList.Count()} clean glasses on the shelf");
                    }

                    foreach (Glass glass in glassList)
                    {
                        place_Clean_Glass_Delegate(glass);
                    }

                    glassList.Clear();
                }
            }

            listbox_Add_Delegate("The waitress goes home");
        }
    }
}
