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
        ConcurrentQueue<Glass> dirtyGlassQueue = new ConcurrentQueue<Glass>();
        ConcurrentStack<Glass> cleanGlassStack = new ConcurrentStack<Glass>();

        Random random = new Random();
        int counter;
        int patronsCounter;
        public bool pubOpen;

        int availableChairs = 0;
        int chairs = 9;
        int glasses = 8;
        int cleanGlasses;
        int dirtyGlasses;

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
                availableChairs = value;
                OnPropertyChanged();
            }

        }

        public int CleanGlassStackCount
        {
            get { return cleanGlassStack.Count; }
            set
            {
                cleanGlasses = value;
                OnPropertyChanged();
            }
        }

        public int DirtyGlassQueueCount
        {
            get { return dirtyGlassQueue.Count; }
            set
            {
                dirtyGlasses= value;
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
                OnPropertyChanged();
            });
            Task.Run(() => bouncer.Bouncer_Work(Add_To_Listbox_Patrons, Add_Patron_To_BarQueue, Change_Patrons_Counter));
            Task.Run(() => bartender.Bartender_Work(Add_To_Listbox_Bartender, Check_Bar_Queue, Check_Glasses, Take_Clean_Glass));
            Task.Run(() => waitress.WaitressWork(Add_To_Listbox_Waitress, Take_Dirty_Glass, GetPatronsCount, Place_Clean_Glass));
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

        public void Add_To_Listbox_Waitress(String str)
        {
            Dispatcher.Invoke(() =>
            {
                Listbox_Waitress.Items.Insert(0, $"{counter} {str}");
            });
            counter++;
        }

        public bool Check_Bar_Queue()
        {
            return !barQueue.IsEmpty;
        }

        public bool Check_Glasses()
        {
            return CleanGlassStackCount > 0;
        }

        public string Get_First_Patron()
        {
            return barQueue.First().Name;
        }

        public Glass Take_Clean_Glass()
        {
            cleanGlassStack.TryPop(out Glass glass);
            CleanGlassStackCount = cleanGlassStack.Count();
            OnPropertyChanged();
            return glass;
        }

        public Glass Take_Dirty_Glass()
        {
            dirtyGlassQueue.TryDequeue(out Glass glass);
            DirtyGlassQueueCount = dirtyGlassQueue.Count();
            OnPropertyChanged();
            return glass;
        }

        public void Place_Clean_Glass(Glass glass)
        {
            cleanGlassStack.Push(glass);
            CleanGlassStackCount = cleanGlassStack.Count();
            OnPropertyChanged();
        }

        public void Change_Patrons_Counter(int value) => patronsCounter += value;

        public bool GetPatronsCount() => patronsCounter > 0;

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

        public void On_Drink_Served(Glass glass)
        {
            Dispatcher.Invoke(() => Listbox_Patrons.Items.Insert(0, $"{counter} {barQueue.First().Name} grabs the beer and looks for a chair"));
            counter++;
            barQueue.TryDequeue(out Patron patron);
            Change_Barqueue();
            patron.Glass = glass;
            chairQueue.Enqueue(patron);
            Change_ChairQueue();
            Thread.Sleep(4000);
            Task.Run(() => patron.Drink(Add_To_Listbox_Patrons, FirstChairQueue, TakeChair, Go_Home));
        }
        public bool FirstChairQueue(Patron patron)
        {
            return chairQueue.First() == patron;
        }

        public Chair TakeChair()
        {
            chairQueue.TryDequeue(out Patron patron);
            Change_ChairQueue();
            chairStack.TryPop(out Chair chair);
            ChairStackCount = chairStack.Count();
            OnPropertyChanged();
            return chair;
        }

        public void Go_Home(Chair chair, Glass glass)
        {
            chairStack.Push(chair);
            ChairStackCount = chairStack.Count();
            dirtyGlassQueue.Enqueue(glass);
            DirtyGlassQueueCount = dirtyGlassQueue.Count();
            OnPropertyChanged();
            patronsCounter--;
        }
    }
}
