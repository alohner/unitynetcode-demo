using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Services.Core;
using Unity.Services.Authentication;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using UnityEngine;
using System;

public class TutorialManager : MonoBehaviour
{

    private UnityTransport transport;
    public TMPro.TMP_InputField joinCodeInputField;

    async void Awake(){
        await Authenticate();
    }

    private static async Task Authenticate(){
        await UnityServices.InitializeAsync();
        await AuthenticationService.Instance.SignInAnonymouslyAsync();
    }


    void Start(){
        transport = FindObjectOfType<UnityTransport>();
    }

    public async void CreateMultiplayerRelay(){
        Allocation a = await RelayService.Instance.CreateAllocationAsync(2);
        joinCodeInputField.text = await RelayService.Instance.GetJoinCodeAsync(a.AllocationId);

        transport.SetRelayServerData(a.RelayServer.IpV4, (ushort) a.RelayServer.Port, a.AllocationIdBytes, a.Key, a.ConnectionData);

        NetworkManager.Singleton.StartHost();
    }

    public async void JoinSession(){
        try {
            JoinAllocation a = await RelayService.Instance.JoinAllocationAsync(joinCodeInputField.text);
        
            transport.SetClientRelayData(a.RelayServer.IpV4, (ushort) a.RelayServer.Port, a.AllocationIdBytes, a.Key, a.ConnectionData, a.HostConnectionData);
            
            NetworkManager.Singleton.StartClient();
        }catch(Exception e){
            Debug.LogError("Error when trying to join multiplayer lobby "+e.Message);
        }
    }

    public void StartClient(){
        NetworkManager.Singleton.StartClient();
    }

    public void StartHost(){
        NetworkManager.Singleton.StartHost();
    }
}
