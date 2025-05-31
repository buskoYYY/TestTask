using System.Collections;
using UnityEngine;
using System;


public class TimeManager : MonoBehaviour
{
    private DateTime currentTime;

    public void SetServerTime(DateTime serverTime)
    {
        currentTime = serverTime;
        StartCoroutine(UpdateTime());
    }

    private IEnumerator UpdateTime()
    {
        while (true)
        {
            currentTime = currentTime.AddSeconds(1);
            DateTime date = currentTime.Date;
            TimeSpan time = currentTime.TimeOfDay;
            Debug.Log("������� ����: " + date.ToString("yyyy-MM-dd"));
            Debug.Log("������� �����: " + time.ToString(@"hh\:mm\:ss"));
            yield return new WaitForSeconds(1);
        }
    }
}
