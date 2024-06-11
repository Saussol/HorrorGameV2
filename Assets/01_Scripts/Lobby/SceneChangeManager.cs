using System.Collections;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.SceneManagement;

public class SceneChangeManager : NetworkBehaviour
{
    // Nom de la sc�ne � charger
    [SerializeField] private string sceneToLoad = "MultiScene";

    // V�rifie si tous les joueurs sont pr�ts
    private bool AreAllPlayersReady()
    {
        foreach (var connection in NetworkManager.Singleton.ConnectedClientsList)
        {
            if (!connection.PlayerObject.IsSpawned)
            {
                return false;
            }
        }
        return true;
    }

    // Coroutine pour attendre que tous les joueurs soient pr�ts
    private IEnumerator WaitForAllPlayers()
    {
        while (!AreAllPlayersReady())
        {
            yield return null;
        }

        // Tous les joueurs sont pr�ts, changer de sc�ne
        ChangeScene();
    }

    // Changer de sc�ne
    private void ChangeScene()
    {
        SceneManager.LoadScene(sceneToLoad);
    }

    // M�thode appel�e lorsque la sc�ne est charg�e
    public override void OnNetworkSpawn()
    {
        // V�rifier si le gestionnaire de sc�ne est dans la sc�ne de jeu
        if (SceneManager.GetActiveScene().name == sceneToLoad)
        {
            // Attendre que tous les joueurs soient pr�ts
            StartCoroutine(WaitForAllPlayers());
        }
    }
}