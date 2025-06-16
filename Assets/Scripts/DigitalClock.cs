using System;
using TMPro;
using UnityEngine;

public class DigitalClock : MonoBehaviour
{
    [SerializeField] TimeManager _timeManager;
    [SerializeField] TextMeshProUGUI _hourText;
    [SerializeField] TextMeshProUGUI _minuteText;
    [SerializeField] TextMeshProUGUI _dayText;
    [SerializeField] TextMeshProUGUI _monthText;
    [SerializeField] TextMeshProUGUI _yearText;

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
        _hourText.text = _time.Hour.ToString();
        _minuteText.text = _time.Minute.ToString();
        _dayText.text = _time.Day.ToString();
        _monthText.text = _time.Month.ToString();
        _yearText.text = _time.Year.ToString();
    }

    private void OnTimeSetHandler(DateTime time)
    {
        _time = time;
    }
}
