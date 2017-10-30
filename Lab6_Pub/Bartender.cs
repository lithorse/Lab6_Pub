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
        public delegate void Listbox_Add_Delegate(String str);
        public delegate bool Check_Bar_Queue_Delegate();
        public delegate bool Check_Glasses_Delegate();
        public delegate void Change_Glasses_Delegate(int value);

        public event Action Drink_Served;

        public void Bartender_Work(Listbox_Add_Delegate listbox_Add_Delegate, Check_Bar_Queue_Delegate check_Bar_Queue, Check_Glasses_Delegate check_Glasses_Delegate, Change_Glasses_Delegate change_Glasses_Delegate)
        {
            listbox_Add_Delegate("The bartender waits for patrons");
            while (pubOpen || check_Bar_Queue())
            {
                if (check_Bar_Queue() && check_Glasses_Delegate())
                {
                    listbox_Add_Delegate("The bartender gets a glass");
                    Thread.Sleep(3000);
                    change_Glasses_Delegate(-1);
                    listbox_Add_Delegate("The bartender pours a beer");
                    Thread.Sleep(3000);
                    Drink_Served();
                }
            }
        }
    }
}
