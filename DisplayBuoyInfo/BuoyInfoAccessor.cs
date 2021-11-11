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
    public class BuoyInfoAccessor
    {

        private List<BuoyInfo> _buoyDataList;
        private int _currentDataItem;

        public static string METERS_PER_SECOND = "m/s";
        public static string METERS = "meters";
        public static char CELSIUS = 'C';
        public static char FAHRENHEIT = 'F';
        public static string KNOTS = "knots";
        public static float MAX_FLOAT_VALUE = 10000;
        public static int MAX_INT_VALUE = 10000;

        public BuoyInfoAccessor()
        {
        }

        public void FetchData(string buoyId)
        {
            // try to get the buoy info if we have it saved in temp, otherwise hitting get data will 
            // download the file
            _buoyDataList = GetBuoyInfo(GetTempFilePath(buoyId));
            _currentDataItem = 0;
        }

        public void SetBuoyData(List<BuoyInfo> buoyDataList)
        {
            _buoyDataList = buoyDataList;
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
            get { return _buoyDataList != null && _buoyDataList.Count > 0; }
        }

        public string TimeZoneName()
        {
            return TimeZone.CurrentTimeZone.StandardName;
        }

        public string Date()
        {
            // translate datetime into user's current timezone
            return _buoyDataList[_currentDataItem].GetDate().ToLocalTime().ToString();
        }
        
        public string WindDirection()
        {
            // Return ? if the value was not provided in the data. Assume a value outside of 0...360 is missing.

            int windDirection = _buoyDataList[_currentDataItem].GetWindDirection();
            if (windDirection >= 0 && windDirection <= 360) {
                
                return String.Format("{0}°{1}", windDirection, DetermineCompassDirection(windDirection));
            } else {
                return "?";
            }
        }

        public string WindSpeed(string unit)
        {
            float windSpeed = _buoyDataList[_currentDataItem].GetWindSpeed();
            if (windSpeed < MAX_FLOAT_VALUE) {
                if (unit.Equals(KNOTS)){
                   return String.Format("{0} {1}", MetersPerSecondToKnots(windSpeed), unit);
                } else if (unit.Equals(METERS_PER_SECOND)) {
                    return String.Format("{0} {1}", windSpeed, unit);
                } else {
                    throw new NotImplementedException();
                }
            } else {
                return "?";
            }
        }

        public string Gust(string unit)
        {
            float gust = _buoyDataList[_currentDataItem].GetGust();
            if (gust < MAX_FLOAT_VALUE)
            {
                if (unit.Equals(KNOTS))
                {
                    return String.Format("{0} {1}", MetersPerSecondToKnots(gust), unit);
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

        public string WaveHeight(string unit)
        {
            float waveHeight = _buoyDataList[_currentDataItem].GetWaveHeight();
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


        public string AirTemp(char unit)
        {
            float airTemp = _buoyDataList[_currentDataItem].GetAirTemp();
            if (airTemp < MAX_FLOAT_VALUE)
            {
                if (unit.Equals(FAHRENHEIT)) {
                    return String.Format("{0}°{1}", CelsiusToFahrenheit(airTemp), unit);
                } else if (unit.Equals(CELSIUS)) {
                    return String.Format("{0}°{1}", airTemp, unit);
                } else {
                    throw new NotImplementedException();
                }
            } else {
                return "?";
            }
        }

        public string SeaSurfaceTemp(char unit)
        {
            float seaSurfaceTemp = _buoyDataList[_currentDataItem].GetSeaSurfaceTemp();
            if (seaSurfaceTemp < MAX_FLOAT_VALUE)
            {
                if (unit.Equals(FAHRENHEIT)) {
                    return String.Format("{0}°{1}", CelsiusToFahrenheit(seaSurfaceTemp), unit);
                } else if (unit.Equals(CELSIUS)) {
                    return String.Format("{0}°{1}", seaSurfaceTemp, unit);
                } else {
                    throw new NotImplementedException();
                }
            } else {
                return "?";
            }
        }

        private string DetermineCompassDirection(int windDirection)
        {
            // wind direction is in degrees clockwise from North

            if (windDirection >= 45 && windDirection <= 135)
            {
                return "E";
            }
            else if (windDirection > 135 && windDirection < 225)
            {
                return "S";
            }
            else if (windDirection >= 225 && windDirection <= 315)
            {
                return "W";
            }
            else
            {
                // 316...360 0...45
                return "N";
            }
        }

        private float CelsiusToFahrenheit(float celsius)
        {
            return ((celsius * 9 / 5) + 32);

        }

        private float MetersPerSecondToKnots(float metersPerSecond)
        {
            int numOfDecimalPlaces = 1;
            return (System.Convert.ToSingle(Math.Round(System.Convert.ToDouble(metersPerSecond) * 1.9438, numOfDecimalPlaces)));
        }

        public string GetTempFilePath(string buoyId)
        {
            return Path.Combine(Path.GetTempPath(), String.Format("Buoy_Data_{0}.txt", buoyId));
        }

        public List<BuoyInfo> GetBuoyInfo(string filePath)
        {
            List<BuoyInfo> buoyDataList = new List<BuoyInfo>();

            StreamReader stream;
            try
            {
                stream = new StreamReader(filePath);
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("File could not be read.");
                return buoyDataList;
            }

            // check data in file is in expected order
            string[] header = Regex.Replace(stream.ReadLine().Substring(1), @"\s+", ",").Split(',');

            List<string> fields = new List<string>() {
                "YY", "MM", "DD", "hh", "mm", "WDIR", "WSPD", "GST", "WVHT", "DPD", "APD", "MWD", "PRES", "ATMP", "WTMP", "DEWP", "VIS", "PTDY", "TIDE"};

            if (header.Length != fields.Count)
            {
                Console.WriteLine($"Unexpected number of fields in the file: {header.Length}");
                return buoyDataList;
            }

            for (int i = 0; i < fields.Count; i++)
            {
                if (!fields[i].Equals(header[i]))
                {
                    Console.WriteLine($"Unexpected field: {header[i]}");
                    return buoyDataList;
                }
            }

            // skip the next header line
            _ = stream.ReadLine();




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

                int month = int.Parse(data[1]);
                int day = int.Parse(data[2]);
                int hour = int.Parse(data[3]);
                int minute = int.Parse(data[4]);
                int windDirection = int.Parse(data[5]);
                float windSpeed = float.Parse(data[6]);
                float gust = float.Parse(data[7]);
                float waveHeight = float.Parse(data[8]);
                float airTemp = float.Parse(data[13]);
                float seaSurfaceTemp = float.Parse(data[14]);

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

  
            string url = String.Format("http://www.ndbc.noaa.gov/data/5day2/{0}_5day.txt", buoyId);

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            return Task.Run(() =>
            {
                try
                {              
                    WebClient web = new WebClient();
                    web.DownloadFile(url, GetTempFilePath(buoyId));
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
