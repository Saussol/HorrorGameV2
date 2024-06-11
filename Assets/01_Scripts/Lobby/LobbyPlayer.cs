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

            if (IsOwner)
            {
                // Envoi du pseudo au serveur lorsqu'on se connecte
                SendPlayerNameToServer(Steamworks.SteamClient.Name);
            }
        }

        if (IsServer)
        {
            // Le serveur initialise son propre pseudo
            string steamName = Steamworks.SteamClient.Name;
            playerName.Value = new FixedString32Bytes(steamName);
        }
    }

    private void OnPlayerNameChanged(FixedString32Bytes oldName, FixedString32Bytes newName)
    {
        playerNameText.text = newName.ToString();
    }

    private void SendPlayerNameToServer(string name)
    {
        SetPlayerNameServerRpc(name);
    }

    [ServerRpc]
    private void SetPlayerNameServerRpc(string name, ServerRpcParams rpcParams = default)
    {
        playerName.Value = new FixedString32Bytes(name);

        // Rediffuser le pseudo à tous les clients
        SetPlayerNameClientRpc(name, new ClientRpcParams { Send = new ClientRpcSendParams { TargetClientIds = new List<ulong> { rpcParams.Receive.SenderClientId } } });
    }

    [ClientRpc]
    private void SetPlayerNameClientRpc(string name, ClientRpcParams clientRpcParams = default)
    {
        playerName.Value = new FixedString32Bytes(name);
    }
}
