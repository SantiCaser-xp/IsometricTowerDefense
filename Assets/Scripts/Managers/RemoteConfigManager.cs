using System.Threading.Tasks;
using Unity.Services.RemoteConfig;
using Unity.Services.Authentication;
using Unity.Services.Core;
using UnityEngine;
using System.Collections;

public class RemoteConfigManager : SingltonBase<RemoteConfigManager>
{ 
    public struct userAttributes { }
    public struct appAttributes { }

    async Task InitializeRemoteConfigAsync()
    {
        // initialize handlers for unity game services
        await UnityServices.InitializeAsync();

        // remote config requires authentication for managing environment information
        if (!AuthenticationService.Instance.IsSignedIn)
        {
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
        }
    }

    async void Start()
    {
        if (Utilities.CheckForInternetConnection())
        {
            await InitializeRemoteConfigAsync();
            RemoteConfigService.Instance.FetchCompleted += ApplyRemoteSettings;
            StartCoroutine(ApplyNewData());
        }
    }

    IEnumerator ApplyNewData()
    {
        while (true)
        {
            RemoteConfigService.Instance.FetchConfigs(new userAttributes(), new appAttributes());
            yield return new WaitForSeconds(5);
        }
    }

    void ApplyRemoteSettings(ConfigResponse configResponse)
    {
        Debug.Log("RemoteConfigService.Instance.appConfig fetched: " + RemoteConfigService.Instance.appConfig.config.ToString());
    }
}