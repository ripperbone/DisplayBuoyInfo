using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DisplayBuoyInfo
{
    class BuoyInfoAccessor
    {

        private List<BuoyInfo> _buoyDataList;
        private int _currentDataItem;
        private static string _tempFilePath;

        public string METERS_PER_SECOND = "m/s";
        public string METERS = "meters";
        public char CELSIUS = 'C';
        public char FAHRENHEIT = 'F';
        public string KNOTS = "knots";
        public float MAX_FLOAT_VALUE = 10000;
        public int MAX_INT_VALUE = 10000;

        public BuoyInfoAccessor()
        {
            _tempFilePath = Path.Combine(Path.GetTempPath(), "Buoy_Data.txt");
            FetchData();

        }
        public void FetchData()
        {
            // try to get the buoy info if we have it saved in temp, otherwise hitting get data will 
            // download the file
            _buoyDataList = getBuoyInfo();
            _currentDataItem = 0;
        }

        public bool NextDataItem()
        {
            // Do not increment if no data has been retrieved.

            if (!HasData) return false;

            if (_currentDataItem + 1 < _buoyDataList.Count) {
                _currentDataItem++;
                return true;
            }
            else
            {
                return false;
            }

        }

        public bool PreviousDataItem()
        {
            if (!HasData) return false;

            if (_currentDataItem - 1 > -1)
            {
                // Decrement current data item
                _currentDataItem--;
                return true;
            }
            else
            {
                return false;
            }
        }


        public bool HasData
        {
            get { return (_buoyDataList == null ? false : (_buoyDataList.Count > 0)); }
        }

        private BuoyInfo CurrentInfo
        {
            get { return (_buoyDataList[_currentDataItem]);  }
        }



        public String TimeZoneName()
        {
            return TimeZone.CurrentTimeZone.StandardName;
        }

        public String Date()
        {
            // translate datetime into user's current timezone
            return CurrentInfo.getDate().ToLocalTime().ToString();
        }
        
        public String WindDirection()
        {
            // Return ? if the value was not provided in the data

            int windDirection = CurrentInfo.getWindDirection();
            if (windDirection < MAX_INT_VALUE) {
                
                return String.Format("{0}°{1}", windDirection, determineCompassDirection(windDirection));
            } else {
                return "?";
            }
        }

        public String WindSpeed(String unit)
        {
            float windSpeed = CurrentInfo.getWindSpeed();
            if (windSpeed < MAX_FLOAT_VALUE) {
                if (unit.Equals(KNOTS)){
                   return String.Format("{0} {1}", metersPerSecondToKnots(windSpeed), unit);
                } else if (unit.Equals(METERS_PER_SECOND)) {
                    return String.Format("{0} {1}", windSpeed, unit);
                } else {
                    throw new NotImplementedException();
                }
            } else {
                return "?";
            }
        }

        public String Gust(String unit)
        {
            float gust = CurrentInfo.getGust();
            if (gust < MAX_FLOAT_VALUE)
            {
                if (unit.Equals(KNOTS))
                {
                    return String.Format("{0} {1}", metersPerSecondToKnots(gust), unit);
                }
                else if (unit.Equals(METERS_PER_SECOND))
                {
                    return String.Format("{0} {1}", gust, unit);
                }
                else
                {
                    throw new NotImplementedException();
                }
            }
            else
            {
                return "?";
            }
        }

        public String WaveHeight(String unit)
        {
            float waveHeight = CurrentInfo.getWaveHeight();
            if (waveHeight < MAX_FLOAT_VALUE)
            {
                if (!unit.Equals(METERS))
                {
                    throw new NotImplementedException();
                }
                else
                {
                    return String.Format("{0} {1}", waveHeight, unit);
                }
            }
            else
            {
                return "?";
            }

        }


        public String AirTemp(char unit)
        {
            float airTemp = CurrentInfo.getAirTemp();
            if (airTemp < MAX_FLOAT_VALUE)
            {
                if (unit.Equals(FAHRENHEIT)) {
                    return String.Format("{0}°{1}", celsiusToFahrenheit(airTemp), unit);
                } else if (unit.Equals(CELSIUS)) {
                    return String.Format("{0}°{1}", airTemp, unit);
                } else {
                    throw new NotImplementedException();
                }
            } else {
                return "?";
            }
        }

        public String SeaSurfaceTemp(char unit)
        {
            float seaSurfaceTemp = CurrentInfo.getSeaSurfaceTemp();
            if (seaSurfaceTemp < MAX_FLOAT_VALUE)
            {
                if (unit.Equals(FAHRENHEIT)) {
                    return String.Format("{0}°{1}", celsiusToFahrenheit(seaSurfaceTemp), unit);
                } else if (unit.Equals(CELSIUS)) {
                    return String.Format("{0}°{1}", seaSurfaceTemp, unit);
                } else {
                    throw new NotImplementedException();
                }
            } else {
                return "?";
            }
        }

        private String determineCompassDirection(int windDirection)
        {
            // wind direction is in degrees clockwise from North

            if (windDirection >= 45 && windDirection <= 135)
            {
                return "E";
            }
            else if (windDirection > 135 && windDirection < 215)
            {
                return "S";
            }
            else if (windDirection >= 225 && windDirection <= 315)
            {
               return "W";
            }
            else
            {
                return "N";
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

            

        private List<BuoyInfo> getBuoyInfo()
        {

            List<BuoyInfo> buoyDataList = new List<BuoyInfo>();

            StreamReader stream;
            try
            {
                stream = new StreamReader(_tempFilePath);


            }
            catch (FileNotFoundException)
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
                            data[count] = MAX_FLOAT_VALUE.ToString();
                        }
                        else
                        {
                            data[count] = MAX_INT_VALUE.ToString();
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
    

   

        public Task DownloadFileAsync(string buoyId)
        {
            if (buoyId.Length != 5) throw new InvalidDataException();

  
            string url = string.Format("http://www.ndbc.noaa.gov/data/5day2/{0}_5day.txt", buoyId);
             
            return Task.Run(() =>
            {
                try
                {              
                    WebClient web = new WebClient();
                    web.DownloadFile(url, _tempFilePath);
                }
                catch (WebException ex)
                {
                    Console.WriteLine(ex.ToString());
                    return;
                }
            });
        }
    }
}
