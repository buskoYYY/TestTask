using System.Collections;
using UnityEngine;
using System;

public class TimeManager : MonoBehaviour
{
    private const int TIME_UNIT = 1;

    private DateTime _currentTime;

    public event Action<DateTime> OnTimeSet;

    public void SetTime(DateTime serverTime)
    {
        _currentTime = serverTime;
        StartCoroutine(IncrementTime());
    }

    private IEnumerator IncrementTime()
    {
        while (true)
        {
            _currentTime = _currentTime.AddSeconds(TIME_UNIT);
            DateTime time = _currentTime;
            OnTimeSet?.Invoke(_currentTime);

            yield return new WaitForSeconds(TIME_UNIT);
        }
    }
}
