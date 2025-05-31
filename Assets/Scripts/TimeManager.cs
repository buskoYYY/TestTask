using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class TimeManager : MonoBehaviour

{

    private DateTime currentTime;


    // ����� ��� ��������� ������� � �������

    public void SetServerTime(DateTime serverTime)

    {

        currentTime = serverTime;

        StartCoroutine(UpdateTime());

    }


    // ������� ��� ���������� ������� ������ �������

    private IEnumerator UpdateTime()

    {

        while (true)

        {

            // ������ ������� ����������� ������� �����

            currentTime = currentTime.AddSeconds(1);

            Debug.Log("������� �����: " + currentTime);

            yield return new WaitForSeconds(1);

        }

    }

}
