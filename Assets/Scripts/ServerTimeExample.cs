using System;
using System.Collections;
using UnityEngine;

public class ServerTimeExample : MonoBehaviour
{
    [SerializeField] private TimeFetcher _timeFetcher;
    private TimeManager timeManager;

    private void OnEnable()
    {
       _timeFetcher.OnTimeFetched += UpdateTimeDisplay;
    }

    void Start()
    {
        timeManager = FindObjectOfType<TimeManager>();
    }

    private void UpdateTimeDisplay(DateTime localTime)
    {
        Debug.Log("���������� �����: " + localTime);
        DateTime serverTime = localTime;
        timeManager.SetServerTime(serverTime);
    }

    private void OnDestroy() 
    {
        _timeFetcher.OnTimeFetched -= UpdateTimeDisplay; 
    }
}
