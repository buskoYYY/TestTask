using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using Zenject;

public class ManualTimeSetter : MonoBehaviour
{
    [SerializeField] private TMP_InputField _hourInput;
    [SerializeField] private TMP_InputField _minuteInput;
    [SerializeField] private TMP_InputField _dayInput;
    [SerializeField] private TMP_InputField _monthInput;
    [SerializeField] private TMP_InputField _yearInput;
    [SerializeField] private Effects _effect;
    [SerializeField] private Button _setTimeButton;

    private bool _isManualTimeSet = false;
    private DateTime _currentTime;

    public event Action<DateTime> SetCurrentTimeCallback;
    public event Action SetValidDate;

    public bool IsManualTimeSet { get { return _isManualTimeSet; } }

    [Inject]
    public void Initialize()
    {
        _setTimeButton.onClick.AddListener(OnSetTimeClicked);
    }

    private void OnSetTimeClicked()
    {
        int hour, minute, day, month, year;

        if (!int.TryParse(_hourInput.text, out hour) || hour < 0 || hour > 23)
        {
            Debug.LogWarning("Please enter a valid hour (0-23).");
            SetValidDate?.Invoke();
            return;
        }

        if (!int.TryParse(_minuteInput.text, out minute) || minute < 0 || minute > 59)
        {
            Debug.LogWarning("Please enter a valid minute (0-59).");
            SetValidDate?.Invoke();
            return;
        }

        if (!int.TryParse(_dayInput.text, out day) || day <= 0 || day > 31)
        {
            Debug.LogWarning("Please enter a valid day (1-31).");
            SetValidDate?.Invoke();
            return;
        }

        if (!int.TryParse(_monthInput.text, out month) || month <= 0 || month > 12)
        {
            Debug.LogWarning("Please enter a valid month (1-12).");
            SetValidDate?.Invoke();
            return;
        }

        if (!int.TryParse(_yearInput.text, out year) || year <= 0 || year > 9999)
        {
            Debug.LogWarning("Please enter a valid year (1-9999).");
            SetValidDate?.Invoke();
            return;
        }

        try
        {
            _currentTime = new DateTime(year, month, day, hour, minute, 0);
        }
        catch (ArgumentOutOfRangeException)
        {
            Debug.LogWarning("The date entered is not valid.");
            SetValidDate?.Invoke();
            return;
        }

        _isManualTimeSet = true;
        SetCurrentTimeCallback?.Invoke(_currentTime);
    }
}
