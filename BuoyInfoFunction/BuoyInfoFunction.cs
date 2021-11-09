using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using ScottPlot;

namespace BuoyInfoFunction
{
    public static class BuoyInfoFunction
    {

        public static float MAX_FLOAT_VALUE = 10000;
        public static int MAX_INT_VALUE = 10000;

        [FunctionName("BuoyInfoFunction")]
        public static void Run([TimerTrigger("0 0 2 * * *")]TimerInfo myTimer, ILogger log)
        {
          

            log.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");

            string buoyId = "45007";
            DownloadFileAsync(buoyId).Wait();

            List<BuoyInfo> buoyInfo = GetBuoyInfo(buoyId);

            log.LogInformation($"Buoy info contains {buoyInfo.Count} items");

            string fileLocation = Path.Combine(Path.GetTempPath(), String.Format("Wind_Speed_Graph_{0}.png", buoyId));

            log.LogInformation($"Saving graph to file: {fileLocation}");
            GraphBuoyInfo(buoyInfo, fileLocation);
            log.LogInformation("Done graphing");

            
        }

        private static void GraphBuoyInfo(List<BuoyInfo> buoyInfo, string fileLocation)
        {
            TimeZoneInfo centralTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Central Standard Time");

            double[] dataX = buoyInfo.Where(item => item.GetDate() > DateTime.UtcNow.AddDays(-1))
               .Select(item => TimeZoneInfo.ConvertTimeFromUtc(item.GetDate(), centralTimeZone).ToOADate()).ToArray();

            double[] dataY_wind = buoyInfo.Where(item => item.GetDate() > DateTime.UtcNow.AddDays(-1))
                .Select(item => Convert.ToDouble(MetersPerSecondToKnots(item.GetWindSpeed()))).ToArray();

            double[] dataY_gust = buoyInfo.Where(item => item.GetDate() > DateTime.UtcNow.AddDays(-1))
                .Select(item => Convert.ToDouble(MetersPerSecondToKnots(item.GetGust()))).ToArray();

            Plot plot = new Plot(1000, 500);
            plot.AddScatter(dataX, dataY_wind);
            plot.AddScatter(dataX, dataY_gust);
            plot.XAxis.DateTimeFormat(true);
            plot.YAxis.Label("Knots");
            plot.SaveFig(fileLocation);
        }

        private static List<BuoyInfo> GetBuoyInfo(string buoyId)
        {

            List<BuoyInfo> buoyDataList = new List<BuoyInfo>();

            StreamReader stream;
            try
            {
                stream = new StreamReader(GetTempFilePath(buoyId));


            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("File could not be read.");
                return buoyDataList;
            }


            // skip header lines
            _ = stream.ReadLine();
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




        public static Task DownloadFileAsync(string buoyId)
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

        private static string GetTempFilePath(string buoyId)
        {
            return Path.Combine(Path.GetTempPath(), String.Format("Buoy_Data_{0}.txt", buoyId));
        }


        private static float CelsiusToFahrenheit(float celsius)
        {
            return ((celsius * 9 / 5) + 32);

        }

        private static float MetersPerSecondToKnots(float metersPerSecond)
        {
            int numOfDecimalPlaces = 1;
            return (System.Convert.ToSingle(Math.Round(System.Convert.ToDouble(metersPerSecond) * 1.9438, numOfDecimalPlaces)));
        }
    }
}
