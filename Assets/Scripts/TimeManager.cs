using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class TimeManager : MonoBehaviour

{

    private DateTime currentTime;


    // Метод для установки времени с сервера

    public void SetServerTime(DateTime serverTime)

    {

        currentTime = serverTime;

        StartCoroutine(UpdateTime());

    }


    // Корутин для обновления времени каждую секунду

    private IEnumerator UpdateTime()

    {

        while (true)

        {

            // Каждую секунду увеличиваем текущее время

            currentTime = currentTime.AddSeconds(1);

            Debug.Log("Текущее время: " + currentTime);

            yield return new WaitForSeconds(1);

        }

    }

}
