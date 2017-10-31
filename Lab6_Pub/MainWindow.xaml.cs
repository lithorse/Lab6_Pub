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
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public MainWindow()
        {
            DataContext = this;
            InitializeComponent();
            Change_Barqueue += ChangeBarQueueData;
            Change_ChairQueue += ChangeChairQueueData;
        }
        ConcurrentQueue<Patron> barQueue = new ConcurrentQueue<Patron>();
        ConcurrentQueue<Patron> chairQueue = new ConcurrentQueue<Patron>();
        ConcurrentStack<Chair> chairStack = new ConcurrentStack<Chair>();
        Random random = new Random();
        int counter;
        int patronsCounter;
        public bool pubOpen;

        int chairs = 9;
        public int glasses = 8;
        int barQueueData = 0;
        int chairQueueData = 0;

        public int BarQueueData
        {
            get { return barQueueData; }
            set
            {
                barQueueData = value;
                OnPropertyChanged();
            }
        }

        public int Chairs
        {
            get { return chairs; }
            set
            {
                chairs = value;
                OnPropertyChanged();
            }
        }

        public int ChairStackCount
        {
            get { return chairStack.Count; }
            set
            {
                chairs = value;
                OnPropertyChanged();
            }
        }

        public int ChairQueueData
        {
            get { return chairQueueData; }
            set
            {
                chairQueueData = value;
                OnPropertyChanged();
            }
        }

        public int Glasses
        {
            get { return glasses; }
            set
            {
                glasses = value;
                OnPropertyChanged();
            }
        }


        Bouncer bouncer;
        Waitress waitress;
        Bartender bartender;

        private void Button_Start_Click(object sender, RoutedEventArgs e)
        {
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

            bouncer = new Bouncer();
            waitress = new Waitress();
            bartender = new Bartender();

            bartender.Drink_Served += On_Drink_Served;
            Close_Pub += bouncer.On_Close;


            counter = 1;
            pubOpen = true;
            Task.Run(() =>
            {
                for (int i = 0; i < chairs; i++)
                {
                    chairStack.Push(new Chair());
                };
                ChairStackCount = chairStack.Count();
                OnPropertyChanged();
            });
            Task.Run(() => bouncer.Bouncer_Work(Add_To_Listbox_Patrons, Add_Patron_To_BarQueue, Change_Patrons_Counter));
            Task.Run(() => bartender.Bartender_Work(Add_To_Listbox_Bartender, Check_Bar_Queue, Check_Glasses, Change_Glasses));
        }

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

        public bool Check_Bar_Queue()
        {
            return !barQueue.IsEmpty;
        }

        public bool Check_Glasses()
        {
            return glasses > 0;
        }

        public string Get_First_Patron()
        {
            return barQueue.First().Name;
        }

        public void Change_Glasses(int value)
        {
            Glasses += value;
            OnPropertyChanged();
        }

        public void Change_Patrons_Counter(int value) => patronsCounter += value;


        public void ChangeBarQueueData()
        {
            BarQueueData = barQueue.Count();
        }
        public void ChangeChairQueueData()
        {
            ChairQueueData = chairQueue.Count();
        }

        public event Action Change_Barqueue;
        public event Action Change_ChairQueue;



        public void Add_Patron_To_BarQueue(Patron patron)
        {
            barQueue.Enqueue(patron);
            Change_Barqueue();
        }

        public event Action Close_Pub;
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void Button_Close_Pub_Click(object sender, RoutedEventArgs e)
        {
            Close_Pub();
        }

        public void On_Drink_Served()
        {
            Dispatcher.Invoke(() => Listbox_Patrons.Items.Insert(0, $"{counter} {barQueue.First().Name} grabs the beer and looks for a chair"));
            counter++;
            barQueue.TryDequeue(out Patron patron);
            Change_Barqueue();
            Thread.Sleep(4000);
            chairQueue.Enqueue(patron);
            Change_ChairQueue();
            Task.Run(() => patron.Drink(Add_To_Listbox_Patrons, FirstChairQueue));
        }
        public bool FirstChairQueue(Patron patron)
        {
            return chairQueue.First() == patron;
        }

        public Chair TakeChair()
        {
            chairStack.TryPop(out Chair chair);
            return chair;
        }

    }
}
