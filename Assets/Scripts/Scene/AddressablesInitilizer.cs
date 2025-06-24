using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class AddressablesInitilizer : MonoBehaviour
{
    [SerializeField] private SceneLoader _loader;
    private void Awake()
    {
        InitializeAddressables();
    }

    private void InitializeAddressables()
    {
        Addressables.InitializeAsync().Completed += handle =>
        {
            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                _loader.LoadScene();
                Debug.Log("Addressables successfully initialized.");
            }
            else
            {
                Debug.LogError("Addressables initialization error.");
            }
        };
    }
}
