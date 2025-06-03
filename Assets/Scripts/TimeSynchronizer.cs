using System;
using UnityEngine.Networking;
using UnityEngine;
using System.Collections;

public class TimeSynchronizer : MonoBehaviour
{
    private const string url = "https://yandex.com/time/sync.json";

    private bool _isManualTimeSet = false;

    public event Action<DateTime> OnTimeFetched;

    private void Start()
    {
        StartCoroutine(GetTimeFromServer());
    }

    public void SetManualTimeStatus(bool status)
    {
        _isManualTimeSet = status;
    }

    private IEnumerator GetTimeFromServer()
    {
        if (_isManualTimeSet)
        {
            yield break;
        }

        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Error: " + request.error);

                yield break;
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
