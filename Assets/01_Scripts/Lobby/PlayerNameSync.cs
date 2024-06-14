using Steamworks;
using System;
using TMPro;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

public class PlayerNameSync : NetworkBehaviour
{
    public NetworkVariable<string> PlayerName = new NetworkVariable<string>();
    //public NetworkVariable<FixedString32Bytes> displayName = new NetworkVariable<FixedString32Bytes>();
    public TextMeshPro textName;
    public string name;

    public override void OnNetworkSpawn()
    {
        if (IsOwner)
        {
            string steamName = SteamClient.Name;
            name = steamName;

            // Mettre � jour le pseudonyme sur le serveur
            UpdateDisplayNameServerRpc(steamName);
        }

        // Mettre � jour l'affichage du pseudonyme lorsque la variable change
        PlayerName.OnValueChanged += OnDisplayNameChanged;


        // Mettre � jour l'affichage initial
        OnDisplayNameChanged(default, PlayerName.Value);

        if (IsServer)
        {
            // Informer le nouveau client des pseudonymes des joueurs d�j� connect�s
            UpdateAllClientsServerRpc();
        }

        
        
    }

    private void OnDisplayNameChanged(string previousValue, string newValue)
    {
        Debug.Log("Display Name Changed: " + newValue);
        textName.text = newValue.ToString();
        name = newValue;
    }

   

    [ServerRpc]
    private void UpdateDisplayNameServerRpc(string newName, ServerRpcParams rpcParams = default)
    {
        PlayerName.Value = newName;
        name = newName;
        UpdateDisplayNameClientRpc(newName);
    }

    [ClientRpc]
    private void UpdateDisplayNameClientRpc(string newName)
    {
        PlayerName.Value = newName; // Cette ligne peut encore causer l'erreur si elle est ex�cut�e sur le client
        name = newName;
        textName.text = newName;
        LobbyPlayers.Instance.RefontName();
    }

   

    [ServerRpc]
    private void UpdateAllClientsServerRpc(ServerRpcParams rpcParams = default)
    {
        foreach (var player in FindObjectsOfType<PlayerNameSync>())
        {
            if (player != this)
            {
                UpdateSingleClientClientRpc(player.PlayerName.ToString());
                
            }
        }
    }

    [ClientRpc]
    private void UpdateSingleClientClientRpc(string playerName)
    {
        // Mettre � jour le pseudo des joueurs d�j� connect�s pour le nouveau client
        if (!IsOwner) // Ajoutez cette v�rification pour �viter que le propri�taire ne r��crive sa propre variable
        {
            PlayerName.Value = playerName;
        }
        textName.text = playerName;
        name = playerName;

        LobbyPlayers.Instance.RefontName();
    }

    [ServerRpc]
    public void ChangeDisplayNameServerRpc(string newName)
    {
        // V�rifier si le joueur a les droits n�cessaires pour changer de pseudonyme (� impl�menter)
        // ...

        // Mettre � jour le pseudonyme sur le serveur
        PlayerName.Value = newName;
        name = newName;
        UpdateDisplayNameClientRpc(newName);
    }
}