using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Netcode;
using UnityEngine;

public class QuestGestion : NetworkBehaviour
{
    private NetworkVariable<int> m_Variable = new NetworkVariable<int>(1, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);

    [SerializeField] private GameObject m_SpawnGameObject;
    private GameObject SpawnObject;

    public override void OnNetworkSpawn()
    {
        m_Variable.OnValueChanged += (int previousValue, int newValue) =>
        {
            Debug.Log(OwnerClientId + " mon num " + m_Variable.Value);
        };
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsOwner) return;

        if (Input.GetKeyDown(KeyCode.T))
        {
            SpawnObject = Instantiate(m_SpawnGameObject);
            SpawnObject.GetComponent<NetworkObject>().Spawn(true);

            Debug.Log("spawn");
            //ChangeVariable2ServerRpc();
        }

        if (Input.GetKeyDown(KeyCode.Y))
        {
            Destroy(SpawnObject);
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void ChangeVariable2ServerRpc()
	{
        Debug.Log($"Hello server, from {OwnerClientId}");
        m_Variable.Value += 1;
    }
}
