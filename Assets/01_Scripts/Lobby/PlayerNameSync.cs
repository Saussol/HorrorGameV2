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
            name = SteamClient.Name;

            // Mettre à jour le pseudonyme sur le serveur
            UpdateDisplayNameServerRpc(steamName);
        }

        // Mettre à jour l'affichage du pseudonyme lorsque le joueur rejoint la partie
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
        // Vérifier si le joueur a les droits nécessaires pour changer de pseudonyme (à implémenter)
        // ...

        // Mettre à jour le pseudonyme sur le serveur
        displayName.Value = newName;
        name = newName;
        UpdateDisplayNameClientRpc(newName);
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
        displayName.Value = playerName;
        textName.text = playerName;
    }
}