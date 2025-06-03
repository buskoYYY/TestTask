using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class ManualTimeSetter : MonoBehaviour
{
    [SerializeField] private TMP_InputField _hourInput;
    [SerializeField] public TMP_InputField _minuteInput;
    [SerializeField] public Button _setTimeButton;

    private bool _isManualTimeSet = false;
    private DateTime _currentTime;

    public event Action<DateTime> SetCurrentTimeCallback;

    public bool IsManualTimeSet { get { return _isManualTimeSet; } }


    private void Start()
    {
        _setTimeButton.onClick.AddListener(OnSetTimeClicked);
    }

    public void SetCurrentTime(DateTime time)
    {
        if (_isManualTimeSet)
        {
            _currentTime = time;
            Debug.Log("Current Time: " + _currentTime);
        }
    }

    private void OnSetTimeClicked()
    {
        int hour, minute;

        if (!int.TryParse(_hourInput.text, out hour) || hour < 0 || hour > 23)
        {
            Debug.LogWarning("Please enter a valid hour (0-23).");
            return;
        }

        if (!int.TryParse(_minuteInput.text, out minute) || minute < 0 || minute > 59)
        {
            Debug.LogWarning("Please enter a valid minute (0-59).");
            return;
        }

        _currentTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, hour, minute, 0);

        _isManualTimeSet = true;

        SetCurrentTimeCallback?.Invoke(_currentTime);
    }

}
