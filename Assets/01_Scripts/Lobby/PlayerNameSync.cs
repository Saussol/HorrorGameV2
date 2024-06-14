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
    //private CSteamID steamID;
    public TextMeshPro textName;
    public string name;

    public override void OnNetworkSpawn()
    {
        if (IsOwner)
        {
            //steamID = SteamUser.();
            string steamName = SteamClient.Name;
            name = SteamClient.Name;

            // Mettre à jour le pseudonyme sur le serveur
            UpdateDisplayNameServerRpc(steamName);
        }
        else
        {
            // Mettre à jour l'affichage du pseudonyme lorsque le joueur rejoint la partie
            displayName.OnValueChanged += OnDisplayNameChanged;
        }

        // Mettre à jour l'affichage initial
        OnDisplayNameChanged(default, displayName.Value);
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
}
