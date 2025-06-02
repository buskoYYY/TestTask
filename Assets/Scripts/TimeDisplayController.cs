using System;
using UnityEngine;

public class TimeDisplayController : MonoBehaviour
{
    [SerializeField] private TimeSynchronizer _timeSynchronizer;
    [SerializeField] private TimeManager _timeManager;

    private void OnEnable()
    {
        _timeSynchronizer.OnTimeFetched += OnTimeFetchedHandler;
    }

    private void OnTimeFetchedHandler(DateTime localTime)
    {
        DateTime serverTime = localTime;
        _timeManager.SetTime(serverTime);
    }

    private void OnDestroy()
    {
        _timeSynchronizer.OnTimeFetched -= OnTimeFetchedHandler;
    }
}
