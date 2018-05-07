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
using System.Net;
using System.IO;
using System.Collections;

namespace DailyHoroscope
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private string[] zodiacs = { "Aries", "Taurus", "Gemini", "Cancer", "Leo", "Virgo", "Libra", "Scorpio", "Sagittarius", "Capricorn", "Aquarius", "Pisces"};
        Dictionary<String, String> horoscopes;

        public MainWindow()
        {
            InitializeComponent();
            Init();
        }

        private void Init()
        {
            this.dateLabel.Content = DateTime.Today.DayOfWeek + ", " + DateTime.Now.ToString("MMMM") + " " + DateTime.Today.Day.ToString();
            GetWebsiteData();
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to exit?", "Daily Horoscope", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                Application.Current.Shutdown();
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.horoscopeTextBlock.Text = GetHoroscope(sender.ToString().Split(' ')[1]);
        }

        private string GetHoroscope(string zodiac)
        {
             return zodiac + ": " + this.horoscopes[zodiac];
        }

        private void GetWebsiteData()
        {
            this.horoscopes = new Dictionary<string, string>();

            string data = "";
            string sURL = "http://www.starlightastrology.com/daily.htm";
            WebRequest wrGETURL = WebRequest.Create(sURL);
            Stream objStream = wrGETURL.GetResponse().GetResponseStream();

            StreamReader objReader = new StreamReader(objStream);

            string sLine = "";
            int zodiacNum = 0;

            while (sLine != null && zodiacNum < 12)
            {
                sLine = objReader.ReadLine();
                if (sLine != null && sLine.Contains("CHANGE " + this.zodiacs[zodiacNum].ToUpper() + " HERE"))
                {
                    sLine = objReader.ReadLine();
                    while (!sLine.Contains("END " + this.zodiacs[zodiacNum].ToUpper() + " HERE"))
                    {
                        data += sLine;
                        sLine = objReader.ReadLine();
                    }

                    horoscopes.Add(this.zodiacs[zodiacNum], data);
                    zodiacNum++;
                    data = "";
                }
            }
        }
    }
}
