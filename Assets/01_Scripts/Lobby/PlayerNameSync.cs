using Steamworks;
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
        if (IsOwner)
        {
            string steamName = SteamClient.Name;
            name = steamName;

            // Mettre à jour le pseudonyme sur le serveur
            UpdateDisplayNameServerRpc(steamName);
        }

        // Mettre à jour l'affichage du pseudonyme lorsque la variable change
        displayName.OnValueChanged += OnDisplayNameChanged;

        // Mettre à jour l'affichage initial
        OnDisplayNameChanged(default, displayName.Value);

        if (IsServer)
        {
            // Informer le nouveau client des pseudonymes des joueurs déjà connectés
            UpdateAllClientsServerRpc();
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
        displayName.Value = newName; // Cette ligne peut encore causer l'erreur si elle est exécutée sur le client
        name = newName;
        textName.text = newName;
        LobbyPlayers.Instance.RefontName();
    }

    private void OnDisplayNameChanged(FixedString32Bytes oldName, FixedString32Bytes newName)
    {
        Debug.Log("Display Name Changed: " + newName);
        textName.text = newName.ToString();
    }

    [ServerRpc]
    private void UpdateAllClientsServerRpc(ServerRpcParams rpcParams = default)
    {
        foreach (var player in FindObjectsOfType<PlayerNameSync>())
        {
            if (player != this)
            {
                UpdateSingleClientClientRpc(player.displayName.Value.ToString());
                
            }
        }
    }

    [ClientRpc]
    private void UpdateSingleClientClientRpc(string playerName)
    {
        // Mettre à jour le pseudo des joueurs déjà connectés pour le nouveau client
        if (!IsOwner) // Ajoutez cette vérification pour éviter que le propriétaire ne réécrive sa propre variable
        {
            displayName.Value = playerName;
        }
        textName.text = playerName;
        name = playerName;

        LobbyPlayers.Instance.RefontName();
    }

    [ServerRpc]
    public void ChangeDisplayNameServerRpc(string newName)
    {
        // Vérifier si le joueur a les droits nécessaires pour changer de pseudonyme (à implémenter)
        // ...

        // Mettre à jour le pseudonyme sur le serveur
        displayName.Value = newName;
        name = newName;
        UpdateDisplayNameClientRpc(newName);
    }
}