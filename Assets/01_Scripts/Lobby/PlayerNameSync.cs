using Steamworks;
using TMPro;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

public class PlayerNameSync : NetworkBehaviour
{
    public NetworkVariable<FixedString32Bytes> displayName = new NetworkVariable<FixedString32Bytes>();
    public NetworkVariable<FixedString32Bytes> sharedText = new NetworkVariable<FixedString32Bytes>(); // Variable pour le texte partag�
    public TextMeshPro textName;
    public TextMeshPro sharedTextDisplay; // UI element to display the shared text
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
        displayName.OnValueChanged += OnDisplayNameChanged;

        // Mettre � jour l'affichage initial
        OnDisplayNameChanged(default, displayName.Value);

        if (IsServer)
        {
            // Informer le nouveau client des pseudonymes des joueurs d�j� connect�s
            UpdateAllClientsServerRpc();

            // Envoyer le texte partag� au nouveau client
            UpdateSharedTextServerRpc(sharedText.Value.ToString());
        }

        // Mettre � jour l'affichage du texte partag� lorsque la variable change
        sharedText.OnValueChanged += OnSharedTextChanged;

        // Mettre � jour l'affichage initial du texte partag�
        OnSharedTextChanged(default, sharedText.Value);
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
        if (IsOwner) return; // Ne pas essayer de modifier la NetworkVariable c�t� client
        textName.text = newName;
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
        textName.text = playerName;
    }

    [ServerRpc]
    public void ChangeDisplayNameServerRpc(string newName)
    {
        displayName.Value = newName;
        name = newName;
        UpdateDisplayNameClientRpc(newName);
    }

    [ServerRpc]
    public void UpdateSharedTextServerRpc(string newText, ServerRpcParams rpcParams = default)
    {
        sharedText.Value = newText;
        UpdateSharedTextClientRpc(newText);
    }

    [ClientRpc]
    public void UpdateSharedTextClientRpc(string newText)
    {
        if (IsOwner) return; // Ne pas essayer de modifier la NetworkVariable c�t� client
        sharedTextDisplay.text = newText;
    }

    private void OnSharedTextChanged(FixedString32Bytes oldText, FixedString32Bytes newText)
    {
        sharedTextDisplay.text = newText.ToString();
    }
}