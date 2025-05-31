using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

[Serializable]
public class TimeResponse
{
    public long time;
}

public class TimeFetcher : MonoBehaviour
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

            if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("Error: " + request.error);
            }
            else
            {
                string jsonResponse = request.downloadHandler.text;
                TimeResponse timeResponse = JsonUtility.FromJson<TimeResponse>(jsonResponse);
                DateTime dateTime = DateTimeOffset.FromUnixTimeMilliseconds(timeResponse.time).UtcDateTime;
                DateTime localDateTime = TimeZoneInfo.ConvertTimeFromUtc(dateTime, TimeZoneInfo.Local);
                Debug.Log("Local Time: " + localDateTime);
                OnTimeFetched?.Invoke(localDateTime); 
            }
        }
    }
}
