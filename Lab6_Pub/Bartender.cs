using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Lab6_Pub
{
    class Bartender
    {
        bool pubOpen = true;
        public bool isHome = false;
        Glass glass;
        public delegate void Listbox_Add_Delegate(String str);
        public delegate bool Check_Patrons_Count_Delegate();
        public delegate bool Check_Bar_Queue_Delegate();
        public delegate bool Check_Glasses_Delegate();
        public delegate Glass Take_Glass_Delegate();

        int timeGetGlass = 3;
        int timePourDrink = 1;


        public event Action<Glass> Drink_Served;

        public void On_Close()
        {
            pubOpen = false;
        }

        public void Bartender_Work(double speed, Listbox_Add_Delegate listbox_Add_Delegate, Check_Patrons_Count_Delegate check_Patrons_Count_Delegate, Check_Bar_Queue_Delegate check_Bar_Queue_Delegate, Check_Glasses_Delegate check_Glasses_Delegate, Take_Glass_Delegate take_Glass_Delegate)
        {
            double manipulator = Math.Round((1000d * speed), 0);
            int intManipulator = (int)manipulator;

            listbox_Add_Delegate("The bartender waits for patrons");
            while (pubOpen || check_Patrons_Count_Delegate())
            {
                Thread.Sleep(10);

                if (check_Bar_Queue_Delegate() && check_Glasses_Delegate())
                {
                    listbox_Add_Delegate("The bartender gets a glass");
                    glass = take_Glass_Delegate();
                    Thread.Sleep(timeGetGlass * intManipulator);
                    listbox_Add_Delegate("The bartender pours a beer");
                    Thread.Sleep(timePourDrink * intManipulator);
                    Drink_Served(glass);
                    glass = null;
                }
            }
            isHome = true;
            listbox_Add_Delegate("The bartender goes home");
        }
    }
}
