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
        Glass glass;
        public delegate void Listbox_Add_Delegate(String str);
        public delegate bool Check_Bar_Queue_Delegate();
        public delegate bool Check_Glasses_Delegate();
        public delegate Glass Take_Glass_Delegate();

        public event Action<Glass> Drink_Served;

        public void On_Close()
        {
            pubOpen = false;
        }

        public void Bartender_Work(Listbox_Add_Delegate listbox_Add_Delegate, Check_Bar_Queue_Delegate check_Bar_Queue, Check_Glasses_Delegate check_Glasses_Delegate, Take_Glass_Delegate take_Glass_Delegate)
        {
            listbox_Add_Delegate("The bartender waits for patrons");
            while (pubOpen || check_Bar_Queue())
            {
                Thread.Sleep(10);

                if (check_Bar_Queue() && check_Glasses_Delegate())
                {
                    listbox_Add_Delegate("The bartender gets a glass");
                    glass = take_Glass_Delegate();
                    Thread.Sleep(3000);
                    listbox_Add_Delegate("The bartender pours a beer");
                    Thread.Sleep(3000);
                    Drink_Served(glass);
                    glass = null;
                }
            }
            listbox_Add_Delegate("The bartender goes home");
        }
    }
}
