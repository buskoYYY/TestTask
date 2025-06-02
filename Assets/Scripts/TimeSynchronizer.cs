using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class TimeSynchronizer : MonoBehaviour
{
    private const string url = "https://yandex.com/time/sync.json";

    public event Action<DateTime> OnTimeFetched;

    private void Start()
    {
        StartCoroutine(GetTimeFromServer());
    }

    private IEnumerator GetTimeFromServer()
    {
        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Error: " + request.error);
                yield break; // Прерываем выполнение в случае ошибки
            }
            try
            {
                string jsonResponse = request.downloadHandler.text;
                ServerData timeResponse = JsonUtility.FromJson<ServerData>(jsonResponse);
                DateTime dateTime = DateTimeOffset.FromUnixTimeMilliseconds(timeResponse.time).UtcDateTime;
                DateTime localDateTime = TimeZoneInfo.ConvertTimeFromUtc(dateTime, TimeZoneInfo.Local);
                OnTimeFetched?.Invoke(localDateTime);
            }
            catch (Exception ex)
            {
                Debug.LogError("JSON Parsing Error: " + ex.Message);
            }
        }
    }
}
