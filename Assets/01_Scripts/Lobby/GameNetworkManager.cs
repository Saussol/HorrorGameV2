using Netcode.Transports.Facepunch;
using Steamworks;
using Steamworks.Data;
using System;
using Unity.Netcode;
using UnityEngine;

public class GameNetworkManager : MonoBehaviour
{
	public static GameNetworkManager Instance { get; private set; } = null;

	public Lobby? CurrentLobby { get; private set; } = null;

	private FacepunchTransport transport = null;

	[SerializeField] MainMenuUI mainMenuUI;

	private void Awake()
	{
		if (Instance == null)
			Instance = this;
		else
		{
			Destroy(gameObject);
			return;
		}

		DontDestroyOnLoad(gameObject);
	}

	private void Start()
	{
		transport = GetComponent<FacepunchTransport>();

		SteamMatchmaking.OnLobbyCreated += OnLobbyCreated;
		SteamMatchmaking.OnLobbyEntered += OnLobbyEntered;
		SteamMatchmaking.OnLobbyMemberJoined += OnLobbyMemberJoined;
		SteamMatchmaking.OnLobbyMemberLeave += OnLobbyMemberLeave;
		SteamMatchmaking.OnLobbyInvite += OnLobbyInvite;
		SteamMatchmaking.OnLobbyGameCreated += OnLobbyGameCreated;
		SteamFriends.OnGameLobbyJoinRequested += OnGameLobbyJoinRequested;
	}

	private void OnDestroy()
	{
		SteamMatchmaking.OnLobbyCreated -= OnLobbyCreated;
		SteamMatchmaking.OnLobbyEntered -= OnLobbyEntered;
		SteamMatchmaking.OnLobbyMemberJoined -= OnLobbyMemberJoined;
		SteamMatchmaking.OnLobbyMemberLeave -= OnLobbyMemberLeave;
		SteamMatchmaking.OnLobbyInvite -= OnLobbyInvite;
		SteamMatchmaking.OnLobbyGameCreated -= OnLobbyGameCreated;
		SteamFriends.OnGameLobbyJoinRequested -= OnGameLobbyJoinRequested;

		if (NetworkManager.Singleton == null) return;

		NetworkManager.Singleton.OnServerStarted -= OnServerStarted;
		NetworkManager.Singleton.OnClientConnectedCallback -= OnClientConnectedCallback;
		NetworkManager.Singleton.OnClientDisconnectCallback -= OnClientDisconnectCallback;
	}

	private void OnApplicationQuit() => Disconnect();

	public async void StartHost(int maxMembers = 100)
	{
		NetworkManager.Singleton.OnServerStarted += OnServerStarted;

		NetworkManager.Singleton.StartHost();

		CurrentLobby = await SteamMatchmaking.CreateLobbyAsync(maxMembers);
	}

	public void StartClient(SteamId id)
	{
		NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnectedCallback;
		NetworkManager.Singleton.OnClientDisconnectCallback += OnClientDisconnectCallback;

		transport.targetSteamId = id;

		if (NetworkManager.Singleton.StartClient())
		{
			Debug.Log("Client has joined", this);
			mainMenuUI.JoinGame();
		}
	}

	public void Disconnect()
	{
		CurrentLobby?.Leave();

		if (NetworkManager.Singleton == null) return;

		NetworkManager.Singleton.Shutdown();
	}

	#region Network Callbacks

	private void OnClientDisconnectCallback(ulong clientId)
	{
		Debug.Log($"Client disconnected, clientId={clientId}");

		NetworkManager.Singleton.OnClientConnectedCallback -= OnClientConnectedCallback;
		NetworkManager.Singleton.OnClientDisconnectCallback -= OnClientDisconnectCallback;
	}

	private void OnClientConnectedCallback(ulong clientId) => Debug.Log($"Client connected, clientId={clientId}");

	private void OnServerStarted() => Debug.Log("Server has started", this);

	#endregion

	#region Steam Callbacks

	private void OnGameLobbyJoinRequested(Lobby lobby, SteamId id) => StartClient(id);

	private void OnLobbyGameCreated(Lobby lobby, uint ip, ushort port, SteamId id) { }

	private void OnLobbyInvite(Friend friend, Lobby lobby) => Debug.Log($"You got a invite from {friend.Name}", this);

	private void OnLobbyMemberLeave(Lobby lobby, Friend friend) { }

	private void OnLobbyMemberJoined(Lobby lobby, Friend friend) { }

	private void OnLobbyEntered(Lobby lobby)
	{
		if (NetworkManager.Singleton.IsHost) return;

		StartClient(lobby.Id);
	}

	private void OnLobbyCreated(Result result, Lobby lobby)
	{
		if(result != Result.OK)
		{
			Debug.LogError($"Lobby couldn't be created, {result}", this);
			return;
		}

		lobby.SetFriendsOnly();
		lobby.SetData("name", "Cool Lobby");
		lobby.SetJoinable(true);

		SteamFriends.OpenOverlay("friends");

		Debug.Log("Lobby has been created!", this);
	}

	#endregion
}
