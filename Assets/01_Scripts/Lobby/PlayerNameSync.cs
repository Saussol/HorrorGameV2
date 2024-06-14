using Steamworks;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

public class PlayerNameSync : NetworkBehaviour
{
    public NetworkVariable<FixedString32Bytes> displayName = new NetworkVariable<FixedString32Bytes>();
    public TextMeshPro textName;
    public string name;

    public override void OnNetworkSpawn()
    {
        NewPlayerRedy();
    }

    public void NewPlayerRedy()
    {
        if (IsOwner)
        {
            string steamName = SteamClient.Name;
            name = SteamClient.Name;

            // Mettre � jour le pseudonyme sur le serveur
            UpdateDisplayNameServerRpc(steamName);
        }
        else
        {
            // Mettre � jour l'affichage du pseudonyme lorsque le joueur rejoint la partie
            displayName.OnValueChanged += OnDisplayNameChanged;
        }

        // Mettre � jour l'affichage initial
        OnDisplayNameChanged(default, displayName.Value);

        if (IsServer)
        {
            // Informer le nouveau client des pseudonymes des joueurs d�j� connect�s
            UpdateNewClientServerRpc(displayName.Value.ToString());
        }
    }

    [ServerRpc]
    private void UpdateDisplayNameServerRpc(string newName, ServerRpcParams rpcParams = default)
    {
        displayName.Value = newName;
        name = newName;
        UpdateDisplayNameClientRpc(newName);
    }

    [ClientRpc]
    private void UpdateDisplayNameClientRpc(string newName)
    {
        displayName.Value = newName;
        name = newName;
        textName.text = newName;
    }

    private void OnDisplayNameChanged(FixedString32Bytes oldName, FixedString32Bytes newName)
    {
        Debug.Log("Display Name Changed: " + newName);
        textName.text = newName.ToString();
    }

    [ServerRpc]
    public void ChangeDisplayNameServerRpc(string newName)
    {
        // V�rifier si le joueur a les droits n�cessaires pour changer de pseudonyme (� impl�menter)
        // ...

        // Mettre � jour le pseudonyme sur le serveur
        displayName.Value = newName;
        name = newName;
        UpdateDisplayNameClientRpc(newName);
    }

    [ServerRpc]
    private void UpdateNewClientServerRpc(string playerName, ServerRpcParams rpcParams = default)
    {
        UpdateNewClientClientRpc(playerName);
    }

    [ClientRpc]
    private void UpdateNewClientClientRpc(string playerName)
    {
        // Mettre � jour le pseudo des joueurs d�j� connect�s pour le nouveau client
        displayName.Value = playerName;
        textName.text = playerName;
    }
}
