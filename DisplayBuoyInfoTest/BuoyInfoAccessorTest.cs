using Microsoft.VisualStudio.TestTools.UnitTesting;
using DisplayBuoyInfo;
using System.Collections.Generic;
using System;

namespace DisplayBuoyInfoTest
{
    [TestClass]
    public class BuoyInfoAccessorTest
    {
        [TestMethod]
        public void TestHasData()
        {

            BuoyInfoAccessor buoyInfoAccessor = new BuoyInfoAccessor();
            Assert.IsFalse(buoyInfoAccessor.HasData);

            buoyInfoAccessor.SetBuoyData(new List<BuoyInfo>() { new BuoyInfo() }) ;
            Assert.IsTrue(buoyInfoAccessor.HasData);

            buoyInfoAccessor.SetBuoyData(null);
            Assert.IsFalse(buoyInfoAccessor.HasData);
        }

        [TestMethod]
        public void TestNextDataItem()
        {
            BuoyInfoAccessor buoyInfoAccessor = new BuoyInfoAccessor();
            Assert.IsFalse(buoyInfoAccessor.NextDataItem());

            buoyInfoAccessor.SetBuoyData(new List<BuoyInfo>() { new BuoyInfo() });
            Assert.IsFalse(buoyInfoAccessor.NextDataItem());

        }

        [TestMethod]
        public void TestPreviousDataItem()
        {
            BuoyInfoAccessor buoyInfoAccessor = new BuoyInfoAccessor();
            Assert.IsFalse(buoyInfoAccessor.PreviousDataItem());

            buoyInfoAccessor.SetBuoyData(new List<BuoyInfo>() { new BuoyInfo(), new BuoyInfo() });
            Assert.IsTrue(buoyInfoAccessor.NextDataItem());
            Assert.IsFalse(buoyInfoAccessor.NextDataItem()); // no more next items in list of data
            Assert.IsTrue(buoyInfoAccessor.PreviousDataItem());
            Assert.IsFalse(buoyInfoAccessor.PreviousDataItem()); // no more previous items in list of data
        }

        [TestMethod]
        public void TestDate()
        {
            DateTime expectedDate = new DateTime(2021, 10, 26, 15, 51, 0);
            BuoyInfoAccessor buoyInfoAccessor = new BuoyInfoAccessor();
            buoyInfoAccessor.SetBuoyData(new List<BuoyInfo>() { new BuoyInfo().WithDate(expectedDate) });
            Assert.AreEqual(expectedDate.ToLocalTime().ToString(), buoyInfoAccessor.Date());
        }

        [TestMethod]
        public void TestWindDirection()
        {
            BuoyInfoAccessor buoyInfoAccessor = new BuoyInfoAccessor();
            buoyInfoAccessor.SetBuoyData(new List<BuoyInfo>() {
                new BuoyInfo().WithWindDirection(0),
                new BuoyInfo().WithWindDirection(44),
                new BuoyInfo().WithWindDirection(45),
                new BuoyInfo().WithWindDirection(135),
                new BuoyInfo().WithWindDirection(136),
                new BuoyInfo().WithWindDirection(224),
                new BuoyInfo().WithWindDirection(225),
                new BuoyInfo().WithWindDirection(315),
                new BuoyInfo().WithWindDirection(316),
                new BuoyInfo().WithWindDirection(360),
                new BuoyInfo().WithWindDirection(361),
                new BuoyInfo().WithWindDirection(-1)
            });
            Assert.AreEqual("0°N", buoyInfoAccessor.WindDirection());
            buoyInfoAccessor.NextDataItem();
            Assert.AreEqual("44°N", buoyInfoAccessor.WindDirection());
            buoyInfoAccessor.NextDataItem();
            Assert.AreEqual("45°E", buoyInfoAccessor.WindDirection());
            buoyInfoAccessor.NextDataItem();
            Assert.AreEqual("135°E", buoyInfoAccessor.WindDirection());
            buoyInfoAccessor.NextDataItem();
            Assert.AreEqual("136°S", buoyInfoAccessor.WindDirection());
            buoyInfoAccessor.NextDataItem();
            Assert.AreEqual("224°S", buoyInfoAccessor.WindDirection());
            buoyInfoAccessor.NextDataItem();
            Assert.AreEqual("225°W", buoyInfoAccessor.WindDirection());
            buoyInfoAccessor.NextDataItem();
            Assert.AreEqual("315°W", buoyInfoAccessor.WindDirection());
            buoyInfoAccessor.NextDataItem();
            Assert.AreEqual("316°N", buoyInfoAccessor.WindDirection());
            buoyInfoAccessor.NextDataItem();
            Assert.AreEqual("360°N", buoyInfoAccessor.WindDirection());
            buoyInfoAccessor.NextDataItem();
            Assert.AreEqual("?", buoyInfoAccessor.WindDirection());
            buoyInfoAccessor.NextDataItem();
            Assert.AreEqual("?", buoyInfoAccessor.WindDirection());


        }

        [TestMethod]
        public void TestWindSpeed()
        {
            BuoyInfoAccessor buoyInfoAccessor = new BuoyInfoAccessor();
            buoyInfoAccessor.SetBuoyData(new List<BuoyInfo>()
            {
                new BuoyInfo().WithWindSpeed(10),
                new BuoyInfo().WithWindSpeed(10.1f),
                new BuoyInfo().WithWindSpeed(10001)
            });
            Assert.AreEqual("10 m/s", buoyInfoAccessor.WindSpeed(BuoyInfoAccessor.METERS_PER_SECOND));
            buoyInfoAccessor.NextDataItem();
            Assert.AreEqual("10.1 m/s", buoyInfoAccessor.WindSpeed(BuoyInfoAccessor.METERS_PER_SECOND));
            Assert.AreEqual("19.6 knots", buoyInfoAccessor.WindSpeed(BuoyInfoAccessor.KNOTS));
            Assert.ThrowsException<NotImplementedException>(() => buoyInfoAccessor.WindSpeed("rods/hour"));
            buoyInfoAccessor.NextDataItem();
            Assert.AreEqual("?", buoyInfoAccessor.WindSpeed(BuoyInfoAccessor.METERS_PER_SECOND));
        }

        [TestMethod]
        public void TestGust()
        {
            BuoyInfoAccessor buoyInfoAccessor = new BuoyInfoAccessor();
            buoyInfoAccessor.SetBuoyData(new List<BuoyInfo>()
            {
                new BuoyInfo().WithGust(10),
                new BuoyInfo().WithGust(10.1f),
                new BuoyInfo().WithGust(10001)
            });
            Assert.AreEqual("10 m/s", buoyInfoAccessor.Gust(BuoyInfoAccessor.METERS_PER_SECOND));
            buoyInfoAccessor.NextDataItem();
            Assert.AreEqual("10.1 m/s", buoyInfoAccessor.Gust(BuoyInfoAccessor.METERS_PER_SECOND));
            Assert.AreEqual("19.6 knots", buoyInfoAccessor.Gust(BuoyInfoAccessor.KNOTS));
            Assert.ThrowsException<NotImplementedException>(() => buoyInfoAccessor.Gust("rods/hour"));
            buoyInfoAccessor.NextDataItem();
            Assert.AreEqual("?", buoyInfoAccessor.Gust(BuoyInfoAccessor.METERS_PER_SECOND));
        }

        [TestMethod]
        public void TestWaveHeight()
        {
            BuoyInfoAccessor buoyInfoAccessor = new BuoyInfoAccessor();
            buoyInfoAccessor.SetBuoyData(new List<BuoyInfo>()
            {
                new BuoyInfo().WithWaveHeight(1),
                new BuoyInfo().WithWaveHeight(10001)
            });
            Assert.AreEqual("1 meters", buoyInfoAccessor.WaveHeight(BuoyInfoAccessor.METERS));
            Assert.ThrowsException<NotImplementedException>(() => buoyInfoAccessor.WaveHeight("rods"));
            buoyInfoAccessor.NextDataItem();
            Assert.AreEqual("?", buoyInfoAccessor.WaveHeight(BuoyInfoAccessor.METERS));
        }

        [TestMethod]
        public void TestAirTemp()
        {
            BuoyInfoAccessor buoyInfoAccessor = new BuoyInfoAccessor();
            buoyInfoAccessor.SetBuoyData(new List<BuoyInfo>()
            {
                new BuoyInfo().WithAirTemp(20),
                new BuoyInfo().WithAirTemp(-1),
                new BuoyInfo().WithAirTemp(10001)
            });
            Assert.AreEqual("20°C", buoyInfoAccessor.AirTemp(BuoyInfoAccessor.CELSIUS));
            Assert.AreEqual("68°F", buoyInfoAccessor.AirTemp(BuoyInfoAccessor.FAHRENHEIT));
            buoyInfoAccessor.NextDataItem();
            Assert.AreEqual("-1°C", buoyInfoAccessor.AirTemp(BuoyInfoAccessor.CELSIUS));
            Assert.AreEqual("30.2°F", buoyInfoAccessor.AirTemp(BuoyInfoAccessor.FAHRENHEIT));
            Assert.ThrowsException<NotImplementedException>(() => buoyInfoAccessor.AirTemp('K'));
            buoyInfoAccessor.NextDataItem();
            Assert.AreEqual("?", buoyInfoAccessor.AirTemp(BuoyInfoAccessor.CELSIUS));

        }

        [TestMethod]
        public void TestSeaSurfaceTemp()
        {
            BuoyInfoAccessor buoyInfoAccessor = new BuoyInfoAccessor();
            buoyInfoAccessor.SetBuoyData(new List<BuoyInfo>()
            {
                new BuoyInfo().WithSeaSurfaceTemp(20),
                new BuoyInfo().WithSeaSurfaceTemp(-1),
                new BuoyInfo().WithSeaSurfaceTemp(10001)
            });
            Assert.AreEqual("20°C", buoyInfoAccessor.SeaSurfaceTemp(BuoyInfoAccessor.CELSIUS));
            Assert.AreEqual("68°F", buoyInfoAccessor.SeaSurfaceTemp(BuoyInfoAccessor.FAHRENHEIT));
            buoyInfoAccessor.NextDataItem();
            Assert.AreEqual("-1°C", buoyInfoAccessor.SeaSurfaceTemp(BuoyInfoAccessor.CELSIUS));
            Assert.AreEqual("30.2°F", buoyInfoAccessor.SeaSurfaceTemp(BuoyInfoAccessor.FAHRENHEIT));
            Assert.ThrowsException<NotImplementedException>(() => buoyInfoAccessor.SeaSurfaceTemp('K'));
            buoyInfoAccessor.NextDataItem();
            Assert.AreEqual("?", buoyInfoAccessor.SeaSurfaceTemp(BuoyInfoAccessor.CELSIUS));

        }
    }
}
