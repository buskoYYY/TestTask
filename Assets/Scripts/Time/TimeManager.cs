using System.Collections;
using UnityEngine;
using System;

public class TimeManager : MonoBehaviour
{
    private const int TIME_UNIT = 1;

    private DateTime _currentTime;
    private Coroutine _incrementTimeCoroutine;

    public event Action<DateTime> OnTimeSet;

    public void SetTime(DateTime serverTime)
    {
        _currentTime = serverTime;

        if (_incrementTimeCoroutine != null)
        {
            StopCoroutine(_incrementTimeCoroutine);
        }

        _incrementTimeCoroutine = StartCoroutine(IncrementTime());
    }

    private IEnumerator IncrementTime()
    {
        while (true)
        {
            _currentTime = _currentTime.AddSeconds(TIME_UNIT);
            OnTimeSet?.Invoke(_currentTime);
            yield return new WaitForSeconds(TIME_UNIT);
        }
    }
}
