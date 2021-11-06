using Microsoft.VisualStudio.TestTools.UnitTesting;
using DisplayBuoyInfo;
using System;
using System.Collections.Generic;


namespace DisplayBuoyInfoTest
{
    [TestClass]
    public class BuoyInfoTest
    {
        [TestMethod]
        public void TestToString()
        {
            DateTime now = DateTime.Now;
            int windDirection = 270;
            float windSpeed = 1.1f;
            float gust = 2.2f;
            float waveHeight = 3.3f;
            float airTemp = 4.4f;
            float seaSurfaceTemp = 5.5f;
            BuoyInfo buoyInfo = new BuoyInfo(now, windDirection, windSpeed, gust, waveHeight, airTemp, seaSurfaceTemp);

            Assert.AreEqual(
                String.Format("Date: {0} Wind Direction: {1} Wind Speed: {2} Gust: {3} Wave Height: {4} Air Temp: {5} Sea Surface Temp: {6}",
                now.ToShortDateString(),
                windDirection,
                windSpeed,
                gust,
                waveHeight,
                airTemp,
                seaSurfaceTemp), buoyInfo.ToString());

        }
    }
}
