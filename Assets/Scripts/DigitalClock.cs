using System;
using TMPro;
using UnityEngine;

public class DigitalClock : MonoBehaviour
{
    [SerializeField] TimeManager _timeManager;
    [SerializeField] TextMeshProUGUI _dateText;
    [SerializeField] TextMeshProUGUI _timeText;

    private DateTime _time;

    private void OnEnable()
    {
        _timeManager.OnTimeSet += OnTimeSetHandler;
    }

    private void Update()
    {
        _dateText.text = _time.Date.ToString("yyyy-MM-dd");
        _timeText.text = _time.ToString(@"hh\:mm\:ss");
    }

    private void OnDisable()
    {
        _timeManager.OnTimeSet -= OnTimeSetHandler;
    }

    private void OnTimeSetHandler(DateTime time)
    {
        _time = time;
    }
}
