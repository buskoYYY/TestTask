using System;
using UnityEngine.Networking;
using UnityEngine;
using System.Collections;

public class TimeSynchronizer : MonoBehaviour
{
    private const string url = "https://timeapi.io/api/Time/current/zone?timeZone=UTC";

    [SerializeField] int maxRetries = 3;
    [SerializeField] private float _waitBeforeRetrying = 2;

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

        for (int attempt = 0; attempt < maxRetries; attempt++)
        {
            using (UnityWebRequest request = UnityWebRequest.Get(url))
            {
                yield return request.SendWebRequest();

                if (request.result != UnityWebRequest.Result.Success)
                {
                    Debug.LogError($"Attempt {attempt + 1}/{maxRetries}: Error: {request.error}");

                    if (attempt + 1 >= maxRetries)
                    {
                        Debug.LogWarning("Failed to get time from server after all attempts. Using local time.");
                        OnTimeFetched?.Invoke(DateTime.Now);

                        yield break;
                    }

                    yield return new WaitForSeconds(_waitBeforeRetrying);
                    continue;
                }

                try
                {
                    string jsonResponse = request.downloadHandler.text;
                    ServerData timeResponse = JsonUtility.FromJson<ServerData>(jsonResponse);
                    DateTime dateTime = DateTime.Parse(timeResponse.dateTime);
                    DateTime localDateTime = TimeZoneInfo.ConvertTimeFromUtc(dateTime, TimeZoneInfo.Local);
                    OnTimeFetched?.Invoke(localDateTime);

                    yield break;
                }
                catch (Exception ex)
                {
                    Debug.LogError("JSON Parsing Error: " + ex.Message);
                    Debug.LogError(ex.StackTrace);
                }
            }
        }
    }
}
