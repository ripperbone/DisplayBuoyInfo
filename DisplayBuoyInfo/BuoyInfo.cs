using System;
using System.Text;

class BuoyInfo
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


    public DateTime getDate()
    {
        return _date;
    }


    public int getWindDirection()
    {
        return _windDirection;
    }

    public float getWindSpeed()
    {
        return _windSpeed;
    }

    public float getGust()
    {
        return _gust;
    }


    public float getWaveHeight()
    {
        return _waveHeight;
    }


    public float getAirTemp()
    {
        return _airTemp;
    }

    public float getSeaSurfaceTemp()
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
