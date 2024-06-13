using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Netcode;
using UnityEngine;

public class QuestGestion : NetworkBehaviour
{
    //private NetworkVariable<int> m_Variable = new NetworkVariable<int>(1, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);



    /*public override void OnNetworkSpawn()
    {
        m_Variable.OnValueChanged += (int previousValue, int newValue) =>
        {
            Debug.Log(OwnerClientId + " mon num " + m_Variable.Value);
        };
    }*/

    // Update is called once per frame
    [SerializeField] private GameObject m_SpawnGameObject;
    private GameObject spawnObject;

    // Update is called once per frame
    void Update()
    {
        if (!IsOwner) return;

        if (Input.GetKeyDown(KeyCode.T))
        {
            RequestSpawnObjectServerRpc();
        }

        if (Input.GetKeyDown(KeyCode.Y))
        {
            RequestDestroyObjectServerRpc();
        }
    }

    [ServerRpc]
    private void RequestSpawnObjectServerRpc(ServerRpcParams rpcParams = default)
    {
        GameObject newSpawnedObject = Instantiate(m_SpawnGameObject);
        newSpawnedObject.GetComponent<NetworkObject>().Spawn(true); 

        spawnObject = newSpawnedObject;
        // Optionally, you can keep track of which object was spawned by which player
        ulong clientId = rpcParams.Receive.SenderClientId;
        Debug.Log($"Object spawned by client {clientId}");
    }

    [ServerRpc]
    private void RequestDestroyObjectServerRpc(ServerRpcParams rpcParams = default)
    {
        Destroy(spawnObject);
    }

    /*[ServerRpc(RequireOwnership = false)]
    private void ChangeVariable2ServerRpc()
	{
        Debug.Log($"Hello server, from {OwnerClientId}");
        m_Variable.Value += 1;
    }*/
}
