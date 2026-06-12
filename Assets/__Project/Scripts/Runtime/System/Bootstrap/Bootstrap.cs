using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using System.Collections.Generic;

public static class Bootstrap
{
    [RuntimeInitializeOnLoadMethod]
    public static void Boot()
    {
        Addressables.LoadAssetsAsync<GameObject>("manager").Completed += OnLoadedManagers;
    }

    private static void OnLoadedManagers(AsyncOperationHandle<IList<GameObject>> handle)
    {
        if (handle.Status is not AsyncOperationStatus.Succeeded)
        {
            Debug.LogError("Loading Error");
            return;
        }

        foreach (GameObject mem in handle.Result)
        {
            GameObject.Instantiate(mem);
        }
    }
}
