using System;

[Serializable]

public class ServerData

{
    public int year;
    public int month;
    public int day;
    public int hour;
    public int minute;
    public int seconds;
    public int milliSeconds;
    public string dateTime;
    public string date;
    public string time;
    public string timeZone;
    public string dayOfWeek;
    public bool dstActive;
    public long unixTimeStamp;
}
