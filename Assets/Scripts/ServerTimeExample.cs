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


/*        // Пример получения времени с сервера

        DateTime serverTime = _timeFetcher.GetLocalDateTime(); // Замените на полученное время с сервера

        Debug.Log("Servertime " + serverTime);

        timeManager.SetServerTime(serverTime);*/

    }

    private void UpdateTimeDisplay(DateTime localTime)

    {

        Debug.Log("Полученное время: " + localTime);
        DateTime serverTime = localTime;
        timeManager.SetServerTime(serverTime);
        // Здесь можете обновить UI или выполнить другие действия с временем

    }


    private void OnDestroy() // Чтобы избежать утечек памяти

    {

        _timeFetcher.OnTimeFetched -= UpdateTimeDisplay; // Отписка от события

    }

}
