﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace Lab6_Pub
{
    class Bouncer
    {
        int minTime = 3;
        int maxTime = 10;
        int timePatronToBar = 1;


        Random Random = new Random();
        bool pubOpen = true;
        public bool isHome = false;
        public delegate void Listbox_Add_Delegate(String str);
        public delegate void Add_Patron_To_Queue_Delegate(Patron patron);
        public delegate void Change_Patrons_Counter_Delegate(int value);


        public void On_Close()
        {
            pubOpen = false;
        }

        public void Bouncer_Work(double speed, bool couplesNight, bool busScenario, Listbox_Add_Delegate listbox_Add_Delegate, Add_Patron_To_Queue_Delegate add_Patron_To_Queue_Delegate, Change_Patrons_Counter_Delegate change_Patrons_Counter_Delegate)
        {
            double manipulator;
            if (busScenario)
            {
                manipulator = Math.Round((1000d * speed * 2), 0);
            }
            else
            {
                manipulator = Math.Round((1000d * speed), 0);
            }
            int intManipulator = (int)manipulator;
            Random rnd = new Random();
            if (busScenario)
            {
                Task.Run(() =>
                {
                    Thread.Sleep(20000);
                    for (int i = 0; i < 15; i++)
                    {
                        Patron latestPatron = new Patron(rnd.Next(0, 52));   //perhaps move the list here -> less "magic numbers" 
                    listbox_Add_Delegate(latestPatron.Name + " enters the bar - BUSS LOAD");
                        change_Patrons_Counter_Delegate(+1);
                        add_Patron_To_Queue_Delegate(latestPatron);
                    }
                });
            }

            while (pubOpen)
            {
                Thread.Sleep(Random.Next(minTime * intManipulator, maxTime * intManipulator));
                if (pubOpen)
                {
                    Task.Run(() =>
                    {
                        Patron latestPatron = new Patron(rnd.Next(0, 52));   //perhaps move the list here -> less "magic numbers" 
                        listbox_Add_Delegate(latestPatron.Name + " enters the bar");
                        change_Patrons_Counter_Delegate(+1);
                        Thread.Sleep(timePatronToBar * intManipulator);
                        add_Patron_To_Queue_Delegate(latestPatron);
                    });
                    if (couplesNight)
                    {
                        Task.Run(() =>
                        {
                            Patron latestPatronPlusOne = new Patron(rnd.Next(0, 52));   //perhaps move the list here -> less "magic numbers"
                            listbox_Add_Delegate(latestPatronPlusOne.Name + " enters the bar");
                            change_Patrons_Counter_Delegate(+1);
                            Thread.Sleep(timePatronToBar * intManipulator);
                            add_Patron_To_Queue_Delegate(latestPatronPlusOne);
                        });
                    }
                }
            }
            isHome = true;
            listbox_Add_Delegate("The bouncer goes home");
        }
    }
}