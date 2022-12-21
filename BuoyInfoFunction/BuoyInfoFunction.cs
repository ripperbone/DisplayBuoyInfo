using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using ScottPlot;
using SendGrid.Helpers.Mail;


namespace BuoyInfoFunction
{
    public static class BuoyInfoFunction
    {

        public static float MAX_FLOAT_VALUE = 10000;
        public static int MAX_INT_VALUE = 10000;

        [FunctionName("BuoyInfoFunction")]
        public static void Run([TimerTrigger("0 0 2 * * *")]TimerInfo myTimer,
            [SendGrid(ApiKey="SendGridAPIKey")] out SendGridMessage sendGridMessage,
            ILogger log)
        {
          

            log.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");

            string buoyId = "45007";
            DownloadFileAsync(buoyId, log).Wait();

            List<BuoyInfo> buoyInfo = GetBuoyInfo(buoyId, log);

            log.LogInformation($"Buoy info contains {buoyInfo.Count} items");

            sendGridMessage = new SendGridMessage();

            string ToAddress = System.Environment.GetEnvironmentVariable("EMAIL_TO_ADDRESS");
            string FromAddress = System.Environment.GetEnvironmentVariable("EMAIL_FROM_ADDRESS");
            string FromName = System.Environment.GetEnvironmentVariable("EMAIL_FROM_NAME");

            if (ToAddress == null || FromAddress == null || FromName == null)
            {
                log.LogInformation("Missing email configuration");
                return; // do not continue if unable to get configuration
            }

            sendGridMessage.SetFrom(new EmailAddress(FromAddress, FromName));
            sendGridMessage.AddTo(ToAddress);
            sendGridMessage.SetSubject("Buoy Info");

            string fileName = String.Format("Wind_Speed_Graph_{0}.png", buoyId);
            string fileLocation = Path.Combine(Path.GetTempPath(), fileName);

            List<BuoyInfo> buoyInfoToGraph = buoyInfo.Where(item => item.GetDate() > DateTime.UtcNow.AddDays(-1))
                .Where(item => item.GetWindSpeed() < MAX_FLOAT_VALUE && item.GetGust() < MAX_FLOAT_VALUE).ToList();

            if (buoyInfoToGraph.Count > 0)
            {
                log.LogInformation($"Saving graph to file: {fileLocation}");
                GraphBuoyInfo(buoyInfoToGraph, fileLocation);
                log.LogInformation("Done graphing");

                sendGridMessage.AddContent("text/plain", "Attachment: wind speed graph");
                byte[] bytes = File.ReadAllBytes(fileLocation);
                sendGridMessage.AddAttachment(fileName, Convert.ToBase64String(bytes));
            } else
            {
                sendGridMessage.AddContent("text/plain", "There is no data to graph.");
            }
        }

        private static void GraphBuoyInfo(List<BuoyInfo> buoyInfo, string fileLocation)
        {
            TimeZoneInfo centralTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Central Standard Time");

            double[] dataX = buoyInfo.Select(item => TimeZoneInfo.ConvertTimeFromUtc(item.GetDate(), centralTimeZone).ToOADate()).ToArray();

            double[] dataY_wind = buoyInfo.Select(item => Convert.ToDouble(MetersPerSecondToKnots(item.GetWindSpeed()))).ToArray();

            double[] dataY_gust = buoyInfo.Select(item => Convert.ToDouble(MetersPerSecondToKnots(item.GetGust()))).ToArray();

            Plot plot = new Plot(1000, 500);
            plot.AddScatter(dataX, dataY_wind);
            plot.AddScatter(dataX, dataY_gust);
            plot.XAxis.DateTimeFormat(true);
            plot.YAxis.Label("Knots");
            plot.SaveFig(fileLocation);
        }

        private static List<BuoyInfo> GetBuoyInfo(string buoyId, ILogger log)
        {

            List<BuoyInfo> buoyDataList = new List<BuoyInfo>();

            StreamReader stream;
            try
            {
                stream = new StreamReader(GetTempFilePath(buoyId));


            }
            catch (FileNotFoundException)
            {
                log.LogInformation("File could not be read.");
                return buoyDataList;
            }

            // check data in file is in expected order
            string[] header = Regex.Replace(stream.ReadLine().Substring(1), @"\s+", ",").Split(',');

            List<string> fields = new List<string>() {
                "YY", "MM", "DD", "hh", "mm", "WDIR", "WSPD", "GST", "WVHT", "DPD", "APD", "MWD", "PRES", "ATMP", "WTMP", "DEWP", "VIS", "PTDY", "TIDE"};

            if (header.Length != fields.Count)
            {
                log.LogError($"Unexpected number of fields in the file: {header.Length}");
                return buoyDataList;
            }

            for (int i = 0; i < fields.Count; i++)
            {
                if (!fields[i].Equals(header[i]))
                {
                    log.LogError($"Unexpected field: {header[i]}");
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




        public static Task DownloadFileAsync(string buoyId, ILogger log)
        {
            if (buoyId.Length != 5) throw new InvalidDataException();


            string url = String.Format("http://www.ndbc.noaa.gov/data/5day2/{0}_5day.txt", buoyId);

            return Task.Run(async () =>
            {
                byte[] fileBytes = new byte[0];
                HttpClient httpClient = new HttpClient();
                try
                {
                    fileBytes = await httpClient.GetByteArrayAsync(url);
                }
                catch (HttpRequestException e)
                {
                    log.LogInformation("Could not download the file. Exception: {0}", e.Message);
                }

                if (fileBytes.Length > 0)
                {
                    File.WriteAllBytes(GetTempFilePath(buoyId), fileBytes);
                }
                else if (fileBytes.Length == 0 && File.Exists(GetTempFilePath(buoyId)))
                {
                    log.LogInformation("There was no data. Cleaning up the old file...");
                    File.Delete(GetTempFilePath(buoyId));
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
