using System;
using TMPro;
using UnityEngine;
using Zenject;

public class DigitalClock : MonoBehaviour
{
    [Inject] TimeManager _timeManager;
    [SerializeField] private TextMeshProUGUI _hourText;
    [SerializeField] private TextMeshProUGUI _minuteText;
    [SerializeField] private TextMeshProUGUI _dayText;
    [SerializeField] private TextMeshProUGUI _monthText;
    [SerializeField] private TextMeshProUGUI _yearText;

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
