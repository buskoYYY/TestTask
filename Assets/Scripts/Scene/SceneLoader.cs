using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] private string _sceneAddress;

    public void LoadScene()
    {
        Addressables.LoadSceneAsync(_sceneAddress, LoadSceneMode.Single).Completed += OnSceneLoaded;
    }

    private void OnSceneLoaded(AsyncOperationHandle<SceneInstance> obj)
    {
        if (obj.Status == AsyncOperationStatus.Succeeded)
        {
            if (obj.Status == AsyncOperationStatus.Succeeded)
            {
                Debug.Log("The scene has been uploaded successfully.");
            }
            else
            {
                Debug.LogError("Scene loading error.");
            }
        }
    }
}
