using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
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
            InitializeComponent();
        }

        ConcurrentQueue<Patron> barQueue = new ConcurrentQueue<Patron>();
        ConcurrentQueue<Patron> chairQueue = new ConcurrentQueue<Patron>();
        Random random = new Random();
        int counter;
        int patronsCounter;
        bool pubOpen;

        int glasses = 8;
        int chairs = 9;

        Bouncer bouncer;
        Waitress waitress;
        Bartender bartender;

        private void Button_Start_Click(object sender, RoutedEventArgs e)
        {
            bouncer = new Bouncer();
            waitress = new Waitress();
            bartender = new Bartender();

            counter = 1;
            pubOpen = true;

            Task.Run(() => Bouncer_Work());
            Task.Run(() => Bartender_Work());

            //queue.TryDequeue(out Patron activePatron);
            //Listbox_Patrons.Items.Insert(0, activePatron.Name);
        }

        private void Bouncer_Work()
        {
            while (pubOpen)
            {
                Thread.Sleep(random.Next(3000, 10000));
                if (pubOpen)
                {
                    Patron latestPatron = bouncer.LetInPatron();
                    patronsCounter++;
                    Dispatcher.Invoke(() =>
                    {
                        Listbox_Patrons.Items.Insert(0, $"{counter} {latestPatron.Name} entered the bar");
                    });
                    Task.Run(() =>
                    {
                        Thread.Sleep(1000);
                        barQueue.Enqueue(latestPatron);
                    /*Dispatcher.Invoke(() =>
                    {
                        Listbox_Patrons.Items.Insert(0, $"{counter} {barQueue.Last().Name} stands in queue at the bar");
                    });
                    counter++;*/
                    });
                    counter++;
                }
            }
        }

        private void Bartender_Work()
        {
            Dispatcher.Invoke(() =>
            {
                Listbox_Bartender.Items.Insert(0, $"{counter} The bartender waits for visitors");
            });
            counter++;
            while (pubOpen || patronsCounter > 0)
            {
                if (!barQueue.IsEmpty && glasses > 0)
                {
                    Dispatcher.Invoke(() =>
                    {
                        Listbox_Bartender.Items.Insert(0, $"{counter} The bartender gets a glass");
                    });
                    counter++;
                    Thread.Sleep(3000);
                    glasses--;
                    Dispatcher.Invoke(() =>
                    {
                        Listbox_Bartender.Items.Insert(0, $"{counter} The bartender pours a beer");
                    });
                    counter++;
                    Thread.Sleep(3000);
                    Dispatcher.Invoke(() =>
                    {
                        Listbox_Patrons.Items.Insert(0, $"{counter} {barQueue.First().Name} grabs the beer and looks for a chair");
                    });
                    counter++;
                    barQueue.TryDequeue(out Patron patron);
                    chairQueue.Enqueue(patron);
                }
            }
        }

        private void Button_Pause_Click(object sender, RoutedEventArgs e)
        {
            pubOpen = false;
        }

        public void Add_To_Listbox_Bartender(String str)
        {
            counter++;
        }
    }
}
