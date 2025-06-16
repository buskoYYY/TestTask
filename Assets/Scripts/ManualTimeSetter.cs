using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class ManualTimeSetter : MonoBehaviour
{
    [SerializeField] private TMP_InputField _hourInput;
    [SerializeField] public TMP_InputField _minuteInput;
    [SerializeField] public TMP_InputField _dayInput;
    [SerializeField] public TMP_InputField _monthInput;
    [SerializeField] public TMP_InputField _yearInput;
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
        int hour, minute,day, month, year;

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

        if (!int.TryParse(_dayInput.text, out day) || day < 0 || day > 31)
        {
            Debug.LogWarning("Please enter a valid minute (0-11).");
            return;
        }

        if (!int.TryParse(_monthInput.text, out month) || month < 0 || month > 12)
        {
            Debug.LogWarning("Please enter a valid minute (0-11).");
            return;
        }

        if (!int.TryParse(_yearInput.text, out year) || year < 0 || year > 9999)
        {
            Debug.LogWarning("Please enter a valid minute (0-9999).");
            return;
        }



        _currentTime = new DateTime(year, month, day, hour, minute, 0);

        _isManualTimeSet = true;

        SetCurrentTimeCallback?.Invoke(_currentTime);
    }

}
