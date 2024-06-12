using Steamworks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Steamworks.Data;
using System;
using Unity.Netcode;
using Netcode.Transports.Facepunch;
using UnityEngine.SceneManagement;

public class SteamManager : MonoBehaviour
{

    [SerializeField]
    private TMP_InputField LobbyIDInputField;

    [SerializeField]
    private TextMeshProUGUI LobbyID;

    [SerializeField]
    private GameObject MainMenu;

    [SerializeField]
    private GameObject InLobbyMenu;

    [SerializeField]
    private GameObject[] playerprefTag;

    private void OnEnable()
    {
        SteamMatchmaking.OnLobbyCreated += LobbyCreated;
        SteamMatchmaking.OnLobbyEntered += LobbyEntered;
        SteamMatchmaking.OnLobbyMemberJoined += LobbyMemberJoined;
        SteamFriends.OnGameLobbyJoinRequested += GameLobbyJoinReques;
    }



    private void OnDisable()
    {
        SteamMatchmaking.OnLobbyCreated -= LobbyCreated;
        SteamMatchmaking.OnLobbyEntered -= LobbyEntered;
        SteamMatchmaking.OnLobbyMemberJoined -= LobbyMemberJoined;
        SteamFriends.OnGameLobbyJoinRequested -= GameLobbyJoinReques;
    }

    private async void GameLobbyJoinReques(Lobby lobby, SteamId id)
    {
        await lobby.Join();
    }

    private void LobbyEntered(Lobby lobby)
    {
        LobbySaver.instance.currentLobby = lobby;
        LobbyID.text = lobby.Id.ToString();
        MainMenu.SetActive(false);
        InLobbyMenu.SetActive(true);

        Debug.Log("We entered");

        // Instancier les joueurs déjà présents dans le lobby
        foreach (var member in lobby.Members)
        {
            AddPlayerToLobby(member.Id);
        }

        if (NetworkManager.Singleton.IsHost) return;
        NetworkManager.Singleton.gameObject.GetComponent<FacepunchTransport>().targetSteamId = lobby.Owner.Id;
        NetworkManager.Singleton.StartClient();


    }

    private void LobbyCreated(Result result, Lobby lobby)
    {
        if (result == Result.OK)
        {
            lobby.SetPublic();
            lobby.SetJoinable(true);
            NetworkManager.Singleton.StartHost();

            // Ajouter le créateur du lobby
            AddPlayerToLobby(SteamClient.SteamId);
        }
    }

    public async void HostLobby()
    {
        await SteamMatchmaking.CreateLobbyAsync(4);
    }

    [ContextMenu("JoinLobbyWithID")]
    public async void JoinLobbyWithID()
    {
        Debug.Log("try joining lobby");
		try
		{
            Debug.Log("Joining Lobby 0/5 - Starting");

            ulong ID;
            if (!ulong.TryParse(LobbyIDInputField.text, out ID))
                return;

            Debug.Log("Joining Lobby 1/4 - Parsed lobby Id");

            Lobby[] lobbies = await SteamMatchmaking.LobbyList.WithSlotsAvailable(4).RequestAsync();

            Debug.Log("Joining Lobby 2/4 - Found lobby list");

            foreach (Lobby lobby in lobbies)
            {
                if (lobby.Id == ID)
                {
                    Debug.Log("Joining Lobby 3/4 - Found lobby, start joining");

                    await lobby.Join();

                    Debug.Log("Joining Lobby 4/4 - Joined successfully");

                    return;
                }
            }

            Debug.Log("Joining Lobby ERROR - Fail to found lobby");
        }
		catch (Exception err)
		{
            Debug.Log(err.Message);
			throw;
		}

    }

    public void CopyID()
    {
        TextEditor textEditor = new TextEditor();
        textEditor.text = LobbyID.text;
        textEditor.SelectAll();
        textEditor.Copy();
    }

    public void LeaveLobby()
    {
        // LobbySaver.instance.currentLobby?.Leave();
        // LobbySaver.instance.currentLobby = null;
        //  NetworkManager.Singleton.Shutdown();

    }

    public void StartGameServer()
    {
        if (NetworkManager.Singleton.IsHost)
        {


            NetworkManager.Singleton.SceneManager.LoadScene("GameScene", LoadSceneMode.Single);
        }
    }

    private void LobbyMemberJoined(Lobby lobby, Friend friend)
    {
        AddPlayerToLobby(friend.Id);
    }

    private void AddPlayerToLobby(SteamId steamId)
    {
        Debug.Log("Un de plus ");
        /*//var playerInstance = Instantiate(playerPrefab);
        playerprefTag[0].SetActive(true); // Activer le prefab

        var playerController = playerprefTag[0].GetComponent<LobbyPlayer>();
        if (playerController != null)
        {
            playerController.Initialize(SteamFriends.GetName(steamId));
        }

        playerInstances.Add(playerInstance);*/
    }

}