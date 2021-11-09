using System;
using System.Text;

namespace DisplayBuoyInfo {
    public class BuoyInfo
    {
        DateTime _date;
        int _windDirection;
        float _windSpeed;
        float _gust;
        float _waveHeight;
        float _airTemp;
        float _seaSurfaceTemp;

        public BuoyInfo(DateTime date, int windDirection, float windSpeed, float gust, float waveHeight, float airTemp, float seaSurfaceTemp)
        {
            _date = date;
            _windDirection = windDirection;
            _windSpeed = windSpeed;
            _gust = gust;
            _waveHeight = waveHeight;
            _airTemp = airTemp;
            _seaSurfaceTemp = seaSurfaceTemp;

        }

        public BuoyInfo()
        {
        }

        public BuoyInfo WithDate(DateTime date)
        {
            _date = date;
            return this;
        }

        public BuoyInfo WithWindDirection(int windDirection)
        {
            _windDirection = windDirection;
            return this;
        }

        public BuoyInfo WithWindSpeed(float windSpeed)
        {
            _windSpeed = windSpeed;
            return this;
        }

        public BuoyInfo WithGust(float gust)
        {
            _gust = gust;
            return this;
        }

        public BuoyInfo WithWaveHeight(float waveHeight)
        {
            _waveHeight = waveHeight;
            return this;
        }

        public BuoyInfo WithAirTemp(float airTemp)
        {
            _airTemp = airTemp;
            return this;
        }

        public BuoyInfo WithSeaSurfaceTemp(float seaSurfaceTemp)
        {
            _seaSurfaceTemp = seaSurfaceTemp;
            return this;
        }


        public DateTime GetDate()
        {
            return _date;
        }


        public int GetWindDirection()
        {
            return _windDirection;
        }

        public float GetWindSpeed()
        {
            return _windSpeed;
        }

        public float GetGust()
        {
            return _gust;
        }


        public float GetWaveHeight()
        {
            return _waveHeight;
        }


        public float GetAirTemp()
        {
            return _airTemp;
        }

        public float GetSeaSurfaceTemp()
        {
            return _seaSurfaceTemp;
        }


        override public string ToString()
        {
            StringBuilder strBuild = new StringBuilder();

            strBuild.Append("Date: ");
            strBuild.Append(_date.ToShortDateString());
            strBuild.Append(" Wind Direction: ");
            strBuild.Append(_windDirection);
            strBuild.Append(" Wind Speed: ");
            strBuild.Append(_windSpeed);
            strBuild.Append(" Gust: ");
            strBuild.Append(_gust);
            strBuild.Append(" Wave Height: ");
            strBuild.Append(_waveHeight);
            strBuild.Append(" Air Temp: ");
            strBuild.Append(_airTemp);
            strBuild.Append(" Sea Surface Temp: ");
            strBuild.Append(_seaSurfaceTemp);



            return strBuild.ToString();
        }

    }
}
