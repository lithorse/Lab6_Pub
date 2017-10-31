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
        public Glass Glass;

        public void On_Close()
        {
            pubOpen = false;
        }

        public delegate Glass Get_Dirty_Glass_Delegate();
        public delegate bool Get_Patrons_Count_Delegate();
        public delegate void Place_Clean_Glass_Delegate(Glass glass);
        public delegate void Listbox_Add_Delegate(string str);

        public void WaitressWork( Listbox_Add_Delegate listbox_Add_Delegate, Get_Dirty_Glass_Delegate get_Dirty_Glass_Delegate, Get_Patrons_Count_Delegate get_Patrons_Count_Delegate, Place_Clean_Glass_Delegate place_Clean_Glass_Delegate)
        {
            listbox_Add_Delegate("The waitress is awaiting dirty glasses");
            while (pubOpen || get_Patrons_Count_Delegate())
            {
                while (Glass == null)
                {
                    //try
                    //{
                        Glass = get_Dirty_Glass_Delegate();
                //}
                //    catch
                //{
                //}
            }
                listbox_Add_Delegate("The waitress gets a dirty glass");
                Thread.Sleep(10000);
                listbox_Add_Delegate("The waitress cleans the glass");
                Thread.Sleep(15000);
                listbox_Add_Delegate("The waitress places the clean glass on the shelf");
                place_Clean_Glass_Delegate(Glass);
                Glass = null;
            }
            listbox_Add_Delegate("The waitress goes home");
        }
    }
}
