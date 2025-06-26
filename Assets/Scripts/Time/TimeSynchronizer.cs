using System;

using UnityEngine;


public class TimeSynchronizer : MonoBehaviour

{

    public event Action<DateTime> OnTimeFetched;


    public void SetTime(long time)

    {

        try

        {

            DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddMilliseconds(time);

            DateTime localDateTime = TimeZoneInfo.ConvertTimeFromUtc(dateTime, TimeZoneInfo.Local);


            OnTimeFetched?.Invoke(localDateTime);

        }

        catch (Exception ex)

        {

            Console.WriteLine("Ошибка: " + ex.Message);

        }

    }

}