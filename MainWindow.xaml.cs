using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
using System.IO;
using System.Windows.Threading;

namespace jaildays
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 

    public partial class MainWindow : Window
    {
        private DateTime initDate = new DateTime(2022, 03, 30);
        private DateTime currentDate = new DateTime(2022, 4, 1); //reset datye
        const string dateFilePath = ".\\DayCounter.cfg";

        public MainWindow()
        {
            InitializeComponent();
            if (File.Exists(dateFilePath))
            {
                //Read file
                string[] resetDate = File.ReadAllLines(dateFilePath);
                currentDate = new DateTime(int.Parse(resetDate[0]), int.Parse(resetDate[1]), int.Parse(resetDate[2]));
            }
            else
            {
                //Create file
                string now = DateTime.Now.ToShortDateString();
                string[] comps = now.Split('/');
                currentDate = new DateTime(int.Parse(comps[0]), int.Parse(comps[1]), int.Parse(comps[2]));
                using (StreamWriter sw = File.CreateText(dateFilePath))
                {
                    sw.WriteLine(comps[0]);
                    sw.WriteLine(comps[1]);
                    sw.WriteLine(comps[2]);
                }
            }

            dateDefer(null, null);

            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMinutes(1);
            timer.Tick += dateDefer;
            timer.Start();
        }

        public void dateDefer(object sender, EventArgs e)
        {
            string now = DateTime.Now.ToShortDateString();
            string[] comps = now.Split('/');
            DateTime temp = new DateTime(int.Parse(comps[0]), int.Parse(comps[1]), int.Parse(comps[2]));
            int dist = (int)(temp - currentDate).TotalDays;
            daysRemainingLabel.Content = 14 - dist;
            daysCountLabel.Content = (int)(temp - initDate).TotalDays;
        }

        private void resetBtn_Click(object sender, RoutedEventArgs e)
        {
            //Reset
            File.Delete(dateFilePath);
            string now = DateTime.Now.ToShortDateString();
            string[] comps = now.Split('/');
            currentDate = new DateTime(int.Parse(comps[0]), int.Parse(comps[1]), int.Parse(comps[2]));
            using (StreamWriter sw = File.CreateText(dateFilePath))
            {
                sw.WriteLine(comps[0]);
                sw.WriteLine(comps[1]);
                sw.WriteLine(comps[2]);
            }

            dateDefer(null, null);
        }
    }
}
