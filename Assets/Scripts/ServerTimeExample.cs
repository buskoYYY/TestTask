using System;
using System.Collections;
using System.Collections.Generic;
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


/*        // ������ ��������� ������� � �������

        DateTime serverTime = _timeFetcher.GetLocalDateTime(); // �������� �� ���������� ����� � �������

        Debug.Log("Servertime " + serverTime);

        timeManager.SetServerTime(serverTime);*/

    }

    private void UpdateTimeDisplay(DateTime localTime)

    {

        Debug.Log("���������� �����: " + localTime);
        DateTime serverTime = localTime;
        timeManager.SetServerTime(serverTime);
        // ����� ������ �������� UI ��� ��������� ������ �������� � ��������

    }


    private void OnDestroy() // ����� �������� ������ ������

    {

        _timeFetcher.OnTimeFetched -= UpdateTimeDisplay; // ������� �� �������

    }

}
