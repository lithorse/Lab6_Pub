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
        }
        ConcurrentQueue<Patron> barQueue = new ConcurrentQueue<Patron>();
        ConcurrentQueue<Patron> chairQueue = new ConcurrentQueue<Patron>();
        Random random = new Random();
        int counter;
        int patronsCounter;
        public bool pubOpen;

        int chairs = 9;
        public int glasses = 8;

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
            bouncer = new Bouncer();
            waitress = new Waitress();
            bartender = new Bartender();

            Close_Pub += bouncer.On_Close;

            counter = 1;
            pubOpen = true;

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

        public void Add_Patron_To_BarQueue(Patron patron)
        {
            bartender.Drink_Served += patron.On_Drink_Served/*(add_To_Listbox_Patron, get_First_Patron_Name)*/;
            barQueue.Enqueue(patron);
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
    }
}
