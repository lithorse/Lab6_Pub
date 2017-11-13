using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Lab6_Pub
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            DataContext = this;
            InitializeComponent();
            Change_Barqueue += Change_PatronsInBarQueue;
            Change_ChairQueue += Change_PatronsInChairQueue;
        }
        ConcurrentQueue<Patron> barQueue = new ConcurrentQueue<Patron>();
        ConcurrentQueue<Patron> chairQueue = new ConcurrentQueue<Patron>();
        ConcurrentStack<Chair> chairStack = new ConcurrentStack<Chair>();
        ConcurrentQueue<Glass> dirtyGlassQueue = new ConcurrentQueue<Glass>();
        ConcurrentStack<Glass> cleanGlassStack = new ConcurrentStack<Glass>();

        Random random = new Random();
        int counter;
        int patronsCounter;
        public bool pubOpen;
        bool couplesNight;
        bool guestStayLonger;
        bool fastWaitress;
        bool busScenario;

        int availableChairs;
        int chairs;
        int glasses;
        int cleanGlasses;
        int dirtyGlasses;
        int timer;
        double speed;

        double barPixelWidth = 106;
        int barChartMaxHeight = 100;


        int patronsInBarQueue = 0;
        int patronsInChairQueue = 0;

        public int PatronsCounter
        {
            get { return patronsCounter; }
            set
            {
                patronsCounter = value;
                Dispatcher.Invoke(() =>
                {
                    Label_Patron_Count.Content = "" + patronsCounter;
                    Label_Available_Chairs_Count.Content = "" + availableChairs;
                });
            }
        }

        public int PatronsInBarQueue
        {
            get { return patronsInBarQueue; }
            set
            {
                patronsInBarQueue = value;
                Dispatcher.Invoke(() =>
                {
                    Label_Patron_BarQueue_Count.Content = "" + patronsInBarQueue;
                });
            }
        }
        public int PatronsInChairQueue
        {
            get { return patronsInChairQueue; }
            set
            {
                patronsInChairQueue = value;
                Dispatcher.Invoke(() =>
                {
                    Label_Patron_ChairQueue_Count.Content = "" + patronsInChairQueue;
                });
            }
        }

        public int Chairs
        {
            get { return chairs; }
            set
            {
                chairs = value;
            }
        }

        public int ChairStackCount
        {
            get { return chairStack.Count; }
            set
            {
                availableChairs = value;
                Dispatcher.Invoke(() =>
                {
                    if (availableChairs == Chairs)
                    {
                        Rectangle_AvailableChairs.Width = barPixelWidth;
                    }
                    else if (availableChairs == 0)
                    {
                        Rectangle_AvailableChairs.Width = 0;
                    }
                    else
                    {
                        Rectangle_AvailableChairs.Width = availableChairs * (barPixelWidth / Chairs);

                    }
                    Label_Available_Chairs_Count.Content = "" + availableChairs;
                });
            }

        }


        public int Glasses
        {
            get { return glasses; }
            set
            {
                glasses = value;
            }
        }

        public int CleanGlassStackCount
        {
            get { return cleanGlassStack.Count; }
            set
            {
                cleanGlasses = value;
                Dispatcher.Invoke(() =>
                {
                    if (cleanGlasses == Glasses)
                    {
                        Rectangle_Clean_Glasses.Width = barPixelWidth;
                    }
                    else if (cleanGlasses == 0)
                    {
                        Rectangle_Clean_Glasses.Width = 0;
                    }
                    else
                    {
                        Rectangle_Clean_Glasses.Width = CleanGlassStackCount * (barPixelWidth / Glasses);

                    }
                    Label_Clean_Glasses_Count.Content = "" + cleanGlasses;
                });
            }
        }

        public int DirtyGlassQueueCount
        {
            get { return dirtyGlassQueue.Count; }
            set
            {
                dirtyGlasses = value;
                Dispatcher.Invoke(() =>
                {
                    Label_Dirty_Glasses_Count.Content = "" + dirtyGlasses;
                });
            }
        }

        public int Timer
        {
            get { return timer; }
            set
            {
                timer = value;
                Dispatcher.Invoke(() =>
                {
                    Timer_TextBox.Text = "" + timer;
                });
            }
        }

        Bouncer bouncer;
        Waitress waitress;
        Bartender bartender;

        private void Button_Start_Click(object sender, RoutedEventArgs e)
        {
            if (CheckBoxCouplesNight.IsChecked == true)
            {
                couplesNight = true;
            }
            if (CheckBoxGuestsStayLonger.IsChecked == true)
            {
                guestStayLonger = true;
            }
            if (CheckBoxFastWaitress.IsChecked == true)
            {
                fastWaitress = true;
            }
            if (CheckBoxBusScenario.IsChecked == true)
            {
                busScenario = true;
            }
            Dispatcher.Invoke(() =>
            {
                Rectangle_AvailableChairs_Back.Width = (((barPixelWidth)));
                Rectangle_Clean_Glasses_Back.Width = (((barPixelWidth)));
                CheckBoxCouplesNight.IsEnabled = false;
                CheckBoxGuestsStayLonger.IsEnabled = false;
                CheckBoxFastWaitress.IsEnabled = false;
                CheckBoxBusScenario.IsEnabled = false;
                Glasses_TextBox.IsEnabled = false;
                Chair_TextBox.IsEnabled = false;
                Timer_TextBox.IsEnabled = false;
                Speed_TextBox.IsEnabled = false;
            });
            try
            {
                Chairs = Int32.Parse(Chair_TextBox.Text);
            }
            catch (Exception)
            {
                MessageBox.Show("Invalid input, \n - Chairs");
                return;
            }
            if (Chairs < 1)
            {
                MessageBox.Show("Invalid input \n - Need atleast one chair");
                return;
            }

            try
            {
                Glasses = Int32.Parse(Glasses_TextBox.Text);
            }
            catch (Exception)
            {
                MessageBox.Show("Invalid input, \n - Glasses");
                return;
            }
            if (Glasses < 1)
            {
                MessageBox.Show("Invalid input \n - Need atleast one glass");
                return;
            }

            try
            {
                Timer = Int32.Parse(Timer_TextBox.Text);
            }
            catch (Exception)
            {
                MessageBox.Show("Invalid input, \n - Timer");
                return;
            }
            if (Timer < 0)
            {
                MessageBox.Show("Invalid input \n - Timer can't be less than 0");
                return;
            }
            else if (Timer == 0)
            {
                MessageBox.Show("Invalid input \n - Timer can't be 0");
                return;
            }

            try
            {
                speed = Convert.ToDouble(Speed_TextBox.Text);
            }
            catch (Exception)
            {
                MessageBox.Show("Invalid input, \n - Speed");
                return;
            }
            if (speed < 0)
            {
                MessageBox.Show("Invalid input \n - Speed can't be less than 0");
                return;
            }
            else if (speed == 0)
            {
                MessageBox.Show("Invalid input \n - Speed can't be 0");
                return;
            }

            bouncer = new Bouncer();
            waitress = new Waitress();
            bartender = new Bartender();

            bartender.Drink_Served += On_Drink_Served;
            Close_Pub += bouncer.On_Close;
            Close_Pub += bartender.On_Close;
            Close_Pub += waitress.On_Close;

            counter = 1;
            pubOpen = true;

            Task.Run(() =>
            {
                for (int i = 0; i < glasses; i++)
                {
                    cleanGlassStack.Push(new Glass());
                }

                for (int i = 0; i < chairs; i++)
                {
                    chairStack.Push(new Chair());
                };
                CleanGlassStackCount = cleanGlassStack.Count();
                ChairStackCount = chairStack.Count();
            });

            Task.Run(() => Start_Timer());
            Task.Run(() => bouncer.Bouncer_Work(speed, couplesNight, busScenario, Add_To_Listbox_Patrons, Add_Patron_To_BarQueue, Change_Patrons_Counter));
            Task.Run(() => bartender.Bartender_Work(speed, Add_To_Listbox_Bartender, Check_Bar_Queue, Check_Clean_Glasses, Take_Clean_Glass));
            Task.Run(() => waitress.WaitressWork(speed, fastWaitress, Add_To_Listbox_Waitress, Take_Dirty_Glass, Get_Patrons_Count, Place_Clean_Glass, Check_Dirty_Glasses));
            Task.Run(() => BarChart());
        }

        public event Action Change_Barqueue;
        public event Action Change_ChairQueue;
        public event Action Close_Pub;

        #region ListBox outpu
        public void Change_Patrons_Counter(int value) => PatronsCounter += value;
        public bool Get_Patrons_Count() => PatronsCounter > 0;

        public void Add_To_Listbox_Patrons(String str)
        {
            Dispatcher.Invoke(() =>
            {
                Listbox_Patrons.Items.Insert(0, $"{counter} {str}");
            });
            counter++;
        }

        public void Add_To_Listbox_Bartender(String str)
        {
            Dispatcher.Invoke(() =>
            {
                Listbox_Bartender.Items.Insert(0, $"{counter} {str}");
            });
            counter++;
        }

        public void Add_To_Listbox_Waitress(String str)
        {
            Dispatcher.Invoke(() =>
            {
                Listbox_Waitress.Items.Insert(0, $"{counter} {str}");
            });
            counter++;
        }
        #endregion
        public bool Check_Bar_Queue()
        {
            return !barQueue.IsEmpty;
        }

        public void Add_Patron_To_BarQueue(Patron patron)
        {
            barQueue.Enqueue(patron);
            Change_Barqueue();
        }

        public string Get_First_Patron_Name_In_BarQueue()
        {
            return barQueue.First().Name;
        }

        public void Change_PatronsInBarQueue()
        {
            PatronsInBarQueue = barQueue.Count();
        }

        public void Change_PatronsInChairQueue()
        {
            PatronsInChairQueue = chairQueue.Count();
        }

        public bool Is_First_In_ChairQueue(Patron patron)
        {
            return chairQueue.First() == patron;
        }

        public bool Check_Clean_Glasses()
        {
            return CleanGlassStackCount > 0;
        }

        public Glass Take_Clean_Glass()
        {
            cleanGlassStack.TryPop(out Glass glass);
            CleanGlassStackCount = cleanGlassStack.Count();
            return glass;
        }

        public void Place_Clean_Glass(Glass glass)
        {
            cleanGlassStack.Push(glass);
            CleanGlassStackCount = cleanGlassStack.Count();
        }

        public bool Check_Dirty_Glasses()
        {
            return dirtyGlassQueue.Count() == 0;
        }

        public Glass Take_Dirty_Glass()
        {
            dirtyGlassQueue.TryDequeue(out Glass glass);
            if (dirtyGlassQueue.Count() == 0)
            {
                DirtyGlassQueueCount = 0;
            }
            return glass;
        }

        public Chair Take_Chair()
        {
            chairQueue.TryDequeue(out Patron patron);
            Change_ChairQueue();
            chairStack.TryPop(out Chair chair);
            ChairStackCount = chairStack.Count();
            return chair;
        }

        public void On_Drink_Served(Glass glass)
        {
            Dispatcher.Invoke(() => Listbox_Patrons.Items.Insert(0, $"{counter} {barQueue.First().Name} grabs the beer and looks for a chair"));
            Task.Run(() =>
            {
                counter++;
                barQueue.TryDequeue(out Patron patron);
                Change_Barqueue();
                patron.Glass = glass;
                chairQueue.Enqueue(patron);
                Change_ChairQueue();
                double manipulator = Math.Round((1000d * speed), 0);
                int intManipulator = (int)manipulator;
                Thread.Sleep(4 * intManipulator);
                patron.Drink(guestStayLonger, speed, Add_To_Listbox_Patrons, Is_First_In_ChairQueue, Take_Chair, Patron_Go_Home, CheckForAvailableChairs);
            });
        }

        public bool CheckForAvailableChairs()
        {
            return chairStack.Count != 0;
        }

        public void Patron_Go_Home(Chair chair, Glass glass)
        {
            chairStack.Push(chair);
            ChairStackCount = chairStack.Count();
            dirtyGlassQueue.Enqueue(glass);
            DirtyGlassQueueCount = dirtyGlassQueue.Count();
            PatronsCounter--;
        }

        public void BarChart()
        {
            double barChartBar1 = 0;
            double barChartBar2 = 0;
            double barChartBar3 = 0;
            double barChartBar4 = 0;
            double barChartBar5 = 0;
            double maxPatrons = 0;
            double average50sec;

            int startTime = timer;
            while (PatronsCounter > 0 || pubOpen)
            {
                Thread.Sleep(5010);
                if (PatronsCounter > maxPatrons)
                {
                    maxPatrons = PatronsCounter;
                }

                barChartBar1 = barChartBar2;
                barChartBar2 = barChartBar3;
                barChartBar3 = barChartBar4;
                barChartBar4 = barChartBar5;
                barChartBar5 = patronsCounter;

                Dispatcher.Invoke(() =>
                {
                    ChartBar_Label1.Content = barChartBar1;
                    ChartBar_Label2.Content = barChartBar2;
                    ChartBar_Label3.Content = barChartBar3;
                    ChartBar_Label4.Content = barChartBar4;
                    ChartBar_Label5.Content = barChartBar5;

                    average50sec = (barChartBar5 + barChartBar4 + barChartBar3 + barChartBar2 + barChartBar1) / 5;
                    AverageLine.Height = (average50sec / maxPatrons / 2) * barChartMaxHeight;
                    AverageTextBlock.Height = (average50sec / maxPatrons / 2) * barChartMaxHeight + 14;
                    AverageNumberTextBlock.Height = (average50sec / maxPatrons / 2) * barChartMaxHeight + 14;
                    AverageNumberTextBlock.Text = "" + average50sec;
                    if (average50sec != 0)
                    {
                        AverageTextBlock.Visibility = System.Windows.Visibility.Visible;
                        AverageNumberTextBlock.Visibility = System.Windows.Visibility.Visible;
                    }

                    ChartBar5.Height = (barChartBar5 / maxPatrons / 2) * barChartMaxHeight;
                    ChartBar4.Height = (barChartBar4 / maxPatrons / 2) * barChartMaxHeight;
                    ChartBar3.Height = (barChartBar3 / maxPatrons / 2) * barChartMaxHeight;
                    ChartBar2.Height = (barChartBar2 / maxPatrons / 2) * barChartMaxHeight;
                    ChartBar1.Height = (barChartBar1 / maxPatrons / 2) * barChartMaxHeight;
                });
            }

        }

        public void Start_Timer()
        {
            while (timer > 0)
            {
                Thread.Sleep(1000);
                Timer--;

            }
            Close_Pub();
            pubOpen = false;
        }

        private void Button_Close_Pub_Click(object sender, RoutedEventArgs e)
        {
            Close_Pub();
            pubOpen = false;
        }

    }
}
