using System;
using UnityEngine;

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
        if (!_manualTimeSetter.IsManualTimeSet) // Проверяем, установлено ли ручное время
        {
            _timeManager.SetTime(localTime);
        }
    }

    private void OnManualTimeSet(DateTime manualTime)
    {
        _timeManager.SetTime(manualTime); // Устанавливаем время вручную
    }
}

