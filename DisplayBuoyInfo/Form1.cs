using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DisplayBuoyInfo
{
    public partial class Form1 : Form
    {

      
        List<BuoyInfo> _buoyDataList;
        int _currentDataItem;
        string _path;
        private static char DEGREES= '°';
        private static char SPACE = ' ';
        private static string METERS_PER_SEC = "m/s";
        private static string METERS = "meters";
        private static char CELSIUS = 'C';
        private static char FAHRENHEIT = 'F';
        private static string KNOTS = "knots";


        public Form1()
        {
            InitializeComponent();
            _path = Path.Combine(Path.GetTempPath(), "Buoy_Data.txt");


            // try to get the buoy info if we have it saved in temp, otherwise hitting get data will 
            // download the file

            _buoyDataList = getBuoyInfo();

            if (_buoyDataList.Count > 0)
            {

                setDataOnForm();

            }
        }



        private void setDataOnForm()
        {


            BuoyInfo info = _buoyDataList[_currentDataItem];



            // translate datetime into user's current timezone
            TimeZone localTime = TimeZone.CurrentTimeZone;

            timeZoneLabel.Text = localTime.StandardName;

            dateBox.Text = info.getDate().ToLocalTime().ToString();


            // stored greatest possible value to represent missing data so check that data we get is reasonable

            int windDirection = info.getWindDirection();
            if (windDirection < 1000)
            {
                // wind direction is in degrees clockwise from North

                string compassDirection;

               
                if (windDirection >= 45 && windDirection <= 135)
                {
                    compassDirection = "E";
                } else if (windDirection > 135 && windDirection < 215)
                {
                    compassDirection = "S";
                }
                else if (windDirection >= 225 && windDirection <= 315)
                {
                    compassDirection = "W";
                }
                else
                {
                    compassDirection = "N";
                }

                windDirectionBox.Text = windDirection.ToString() + DEGREES + SPACE + compassDirection;
            }

            float windSpeed = info.getWindSpeed();
            if (windSpeed < 1000)
            {
                if (knotsCheckBox.Checked)
                {
                    windSpeedBox.Text = metersPerSecondToKnots(windSpeed).ToString() + SPACE + KNOTS;
                }
                else
                {
                    windSpeedBox.Text = windSpeed.ToString() + SPACE + METERS_PER_SEC;
                }
            }

            float gust = info.getGust();
            if (gust < 1000)
            {
                if (knotsCheckBox.Checked)
                {
                    gustBox.Text = metersPerSecondToKnots(gust).ToString() + SPACE + KNOTS;
                }
                else
                {
                    gustBox.Text = gust.ToString() + SPACE + METERS_PER_SEC;
                }
            }

            float waveHeight = info.getWaveHeight();
            if (waveHeight < 1000)
            {
                waveHeightBox.Text = waveHeight.ToString() + SPACE + METERS;
            }

            float airTemp = info.getAirTemp();
            if (airTemp < 1000)
            {
                if (fahrenheitCheckBox.Checked)
                {
                    AirTempBox.Text = celsiusToFahrenheit(airTemp).ToString() + SPACE + DEGREES + FAHRENHEIT;
                } else
                {
                    AirTempBox.Text = airTemp.ToString() + SPACE + DEGREES + CELSIUS;
                }
                
            }

            float seaSurfaceTemp = info.getSeaSurfaceTemp();
            if (seaSurfaceTemp < 1000)
            {
                if (fahrenheitCheckBox.Checked)
                {
                    seaSurfaceTempBox.Text = celsiusToFahrenheit(seaSurfaceTemp).ToString() + SPACE + DEGREES + FAHRENHEIT;
                }
                else
                {
                    seaSurfaceTempBox.Text = seaSurfaceTemp.ToString() + SPACE + DEGREES + CELSIUS;
                }
                
            }
        }




        private static Task DownloadFileAsync(string url, string path)
        {
            return Task.Run(() =>
            {
                try
                {              
                    WebClient web = new WebClient();
                    web.DownloadFile(url, path);
                }
                catch (WebException ex)
                {
                    MessageBox.Show(ex.ToString());
                    return;
                }
            });
        }

        private async void getDataButton_Click(object sender, EventArgs e)
        {

            string url = "http://www.ndbc.noaa.gov/data/5day2/45007_5day.txt";
            

            await DownloadFileAsync(url, _path);

            
            

            _buoyDataList = getBuoyInfo();

            if (_buoyDataList.Count > 0)
            {

                setDataOnForm();

            }
            else
            {
                MessageBox.Show("No information was retrieved. This could be because you are not connected to the internet or the information was not able to be downloaded.");
            }

           

          

        }





        


        private List<BuoyInfo> getBuoyInfo()
        {



            Console.Write("Getting data from...");
            Console.WriteLine(_path);


            List<BuoyInfo> buoyDataList = new List<BuoyInfo>();

            StreamReader stream;
            try
            {
                stream = new StreamReader(_path);

                
            } catch (FileNotFoundException)
            {
                Console.WriteLine("File could not be read.");
                return buoyDataList;
            }



            string header1 = stream.ReadLine();
            string header2 = stream.ReadLine();




            string dataString = stream.ReadLine();

            while (dataString != null)
            {

                // Replace any whitespace with a comma so that we can split it

                dataString = Regex.Replace(dataString, @"\s+", ",");

                string[] data = dataString.Split(',');




                int count = 0;

                foreach (string element in data)
                {

                    if (element.Equals("MM"))
                    {
                        if (count >= 6 && count <= 14)
                        {
                            data[count] = Single.MaxValue.ToString();
                        }
                        else
                        {
                            data[count] = Int32.MaxValue.ToString();
                        }
                    }

                    count++;
                }

                int month = Int32.Parse(data[1]);
                int day = Int32.Parse(data[2]);
                int hour = Int32.Parse(data[3]);
                int minute = Int32.Parse(data[4]);
                int windDirection = Int32.Parse(data[5]);
                float windSpeed = Single.Parse(data[6]);
                float gust = Single.Parse(data[7]);
                float waveHeight = Single.Parse(data[8]);
                float airTemp = Single.Parse(data[13]);
                float seaSurfaceTemp = Single.Parse(data[14]);

                // get current year
                DateTime now = DateTime.Now;
                int year = now.Year;


                DateTime date = new DateTime(year, month, day, hour, minute, 0);

                BuoyInfo info = new BuoyInfo(date, windDirection, windSpeed, gust, waveHeight, airTemp, seaSurfaceTemp);

                buoyDataList.Add(info);
                dataString = stream.ReadLine();

            }


            // we're done reading from the file

            stream.Close();


            return buoyDataList;
        }

        private void nextButton_Click(object sender, EventArgs e)
        {

            if (_buoyDataList != null && _buoyDataList.Count > 0)
            {

                if (_currentDataItem + 1 < _buoyDataList.Count)
                {
                    Console.WriteLine("Next!");

                    // Increment current data item

                    _currentDataItem++;

                    setDataOnForm();


                }
                else
                {
                    MessageBox.Show("Reached end of data. Starting back at first record");

                    _currentDataItem = 0;
                }
            } else
            {
               MessageBox.Show("No data has been retrieved yet");
            }
        }

        private void previousButton_Click(object sender, EventArgs e)
        {
            if (_buoyDataList != null && _buoyDataList.Count > 0)
            {

                if (_currentDataItem - 1 > -1)
                {
                    Console.WriteLine("Previous!");

                    // Decrement current data item

                    _currentDataItem--;

                    setDataOnForm();


                }
                else
                {
                    MessageBox.Show("Reached end of data. Starting back at first record");

                    _currentDataItem = 0;
                }
            }
            else
            {
                MessageBox.Show("No data has been retrieved yet");
            }
        }

        private float celsiusToFahrenheit(float celsius)
        {
            return ((celsius * 9 / 5) + 32);

        }

        private float metersPerSecondToKnots(float metersPerSecond)
        {
            int numOfDecimalPlaces = 1;
            return (System.Convert.ToSingle(Math.Round(System.Convert.ToDouble(metersPerSecond) * 1.9438, numOfDecimalPlaces)));
        }

        private void fahrenheitCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (_buoyDataList != null && _buoyDataList.Count > 0)
            {
                setDataOnForm();
            }
           
        }

        private void knotsCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (_buoyDataList != null && _buoyDataList.Count > 0)
            {
                setDataOnForm();
            }
        }
    }
}
