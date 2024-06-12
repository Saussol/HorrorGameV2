using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Netcode;
using UnityEngine;

public class QuestGestion : NetworkBehaviour
{
    private NetworkVariable<int> m_Variable = new NetworkVariable<int>(1, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);


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
        if (IsOwner)
		{
            if (Input.GetKeyDown(KeyCode.T))
            {
                ChangeVariableServerRpc();
            }
        }
		else
		{
            if (Input.GetKeyDown(KeyCode.T))
            {
                ChangeVariable2ServerRpc();
            }
        }
    }

    [ServerRpc]
    private void ChangeVariableServerRpc()
	{
        Debug.Log($"Hello server, from {OwnerClientId}");
        m_Variable.Value += 1;
    }

    [ServerRpc(RequireOwnership = false)]
    private void ChangeVariable2ServerRpc()
	{
        Debug.Log($"Hello server, from {OwnerClientId}");
        m_Variable.Value += 1;
    }
}
