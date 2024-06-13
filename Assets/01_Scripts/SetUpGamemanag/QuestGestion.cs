using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Netcode;
using UnityEngine;

public class QuestGestion : NetworkBehaviour
{
    //private NetworkVariable<int> m_Variable = new NetworkVariable<int>(1, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);

    [SerializeField] private GameObject m_SpawnGameObject;
    private GameObject SpawnObject;

    /*public override void OnNetworkSpawn()
    {
        m_Variable.OnValueChanged += (int previousValue, int newValue) =>
        {
            Debug.Log(OwnerClientId + " mon num " + m_Variable.Value);
        };
    }*/

    // Update is called once per frame
    void Update()
    {
        if (!IsOwner) return;

        if (Input.GetKeyDown(KeyCode.T))
        {
            //SpawnObject = Instantiate(m_SpawnGameObject);
            //SpawnObject.GetComponent<NetworkObject>().Spawn(true);

            RequestSpawnObjectServerRpc();

            Debug.Log("spawn");
            //ChangeVariable2ServerRpc();
        }

        if (Input.GetKeyDown(KeyCode.Y))
        {
            Destroy(SpawnObject);
        }
    }

    [ServerRpc]
    private void RequestSpawnObjectServerRpc(ServerRpcParams rpcParams = default)
    {
        GameObject newSpawnedObject = Instantiate(m_SpawnGameObject);
        NetworkObject networkObject = newSpawnedObject.GetComponent<NetworkObject>();
        networkObject.Spawn(true);

        SpawnObject = newSpawnedObject;

        // Optionally, you can keep track of which object was spawned by which player
        ulong clientId = rpcParams.Receive.SenderClientId;
        Debug.Log($"Object spawned by client {clientId}");

        // If you want to make sure each client can only spawn one object at a time, you might need to handle this logic.
        // This could include keeping a dictionary of client IDs to spawned objects and ensuring only one per client.
    }

    /*[ServerRpc(RequireOwnership = false)]
    private void ChangeVariable2ServerRpc()
	{
        Debug.Log($"Hello server, from {OwnerClientId}");
        m_Variable.Value += 1;
    }*/
}
