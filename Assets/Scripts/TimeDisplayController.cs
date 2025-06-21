using System;
using UnityEngine;
using Zenject;

public class TimeDisplayController : MonoBehaviour
{
    [SerializeField] private TimeSynchronizer _timeSynchronizer;
    [SerializeField] private ManualTimeSetter _manualTimeSetter;
    [SerializeField] private TimeManager _timeManager;

    private void OnEnable()
    {
        _timeSynchronizer.OnTimeFetched += OnTimeFetchedHandler;
        _manualTimeSetter.SetCurrentTimeCallback += OnManualTimeSet;
    }

    private void OnDisable()
    {
        _timeSynchronizer.OnTimeFetched -= OnTimeFetchedHandler;
        _manualTimeSetter.SetCurrentTimeCallback -= OnManualTimeSet;
    }

    private void OnTimeFetchedHandler(DateTime localTime)
    {
        if (!_manualTimeSetter.IsManualTimeSet) 
        {
            _timeManager.SetTime(localTime);
        }
    }

    private void OnManualTimeSet(DateTime manualTime)
    {
        _timeManager.SetTime(manualTime); 
    }
}
