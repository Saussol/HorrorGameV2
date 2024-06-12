using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Netcode;
using UnityEngine;

public class QuestGestion : NetworkBehaviour
{
    private NetworkVariable<int> m_Variable = new NetworkVariable<int>(1, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);


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
        if (!IsLocalPlayer) return;


        if (Input.GetKeyDown(KeyCode.T))
        {
            m_Variable.Value += 1;           
        }


    }
}
