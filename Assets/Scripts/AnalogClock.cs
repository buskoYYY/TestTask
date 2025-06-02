
using System;
using UnityEngine;

public class AnalogClock : MonoBehaviour
{
    private const int FULL_CURCLE_HOURS = 12;
    private const int MINUTES_IN_HOUR = 60;
    private const int SECONDS_IN_MINUTE = 60;
    private const int HOUR_ANGLE = 30;
    private const int MINUTE_ANGLE = 6;
    private const int SECOND_ANGLE = 6;

    [SerializeField] private TimeManager _timeManager;
    [SerializeField] private Transform _hourHand;
    [SerializeField] private Transform _minuteHand;
    [SerializeField] private Transform _secondHand;

    private DateTime _time;

    private void OnEnable()
    {
        _timeManager.OnTimeSet += OnTimeSetHandler;
    }

    private void OnDisable()
    {
        _timeManager.OnTimeSet -= OnTimeSetHandler;
    }
    private void Update()
    {
        DateTime currentTime = _time; 

        float hourAngle = (currentTime.Hour % FULL_CURCLE_HOURS + currentTime.Minute / MINUTES_IN_HOUR) * HOUR_ANGLE; 
        float minuteAngle = (currentTime.Minute + currentTime.Second / SECONDS_IN_MINUTE) * MINUTE_ANGLE; 
        float secondAngle = currentTime.Second * SECOND_ANGLE; 
        _hourHand.localRotation = Quaternion.Euler(0, 0, -hourAngle);
        _minuteHand.localRotation = Quaternion.Euler(0, 0, -minuteAngle);
        _secondHand.localRotation = Quaternion.Euler(0, 0, -secondAngle);
    }

    private void OnTimeSetHandler(DateTime time)
    {
        _time = time;
    }
}
