using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Collections;
using Unity.Netcode;
using UnityEditor.PackageManager;
using UnityEngine;

public class LobbyPlayer : NetworkBehaviour
{
    [SerializeField]
    private TextMeshPro playerNameText;

    private NetworkVariable<FixedString32Bytes> playerName = new NetworkVariable<FixedString32Bytes>();

    public override void OnNetworkSpawn()
    {
        if (IsClient)
        {
            playerName.OnValueChanged += OnPlayerNameChanged;
        }

        if (IsServer)
        {
            string steamName = Steamworks.SteamClient.Name;
            playerName.Value = new FixedString32Bytes(steamName);
        }
    }

    private void OnPlayerNameChanged(FixedString32Bytes oldName, FixedString32Bytes newName)
    {
        playerNameText.text = newName.ToString();
    }

    [ClientRpc]
    public void SetPlayerNameClientRpc(string name, ClientRpcParams clientRpcParams = default)
    {
        playerName.Value = new FixedString32Bytes(name);
    }

    public void Initialize(string name)
    {
        if (IsServer)
        {
            playerName.Value = new FixedString32Bytes(name);
        }
        else
        {
            SetPlayerNameServerRpc(name);
        }
    }

    [ServerRpc]
    private void SetPlayerNameServerRpc(string name)
    {
        playerName.Value = new FixedString32Bytes(name);
    }
}
