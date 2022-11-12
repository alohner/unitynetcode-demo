using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class TutoNetworkVariable : NetworkBehaviour
{

    public NetworkVariable<int> random = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        random.OnValueChanged += (int prev, int next) => {
            Debug.Log("New value is "+next);//random.Value
        };
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I)){
            random.Value = Random.Range(0,100);
        }
    }
}
