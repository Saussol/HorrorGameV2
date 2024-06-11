using System.Collections;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.SceneManagement;

public class SceneChangeManager : NetworkBehaviour
{
    // Nom de la scène à charger
    [SerializeField] private string sceneToLoad = "MultiScene";

    // Vérifie si tous les joueurs sont prêts
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

    // Coroutine pour attendre que tous les joueurs soient prêts
    private IEnumerator WaitForAllPlayers()
    {
        while (!AreAllPlayersReady())
        {
            yield return null;
        }

        // Tous les joueurs sont prêts, changer de scène
        ChangeScene();
    }

    // Changer de scène
    private void ChangeScene()
    {
        SceneManager.LoadScene(sceneToLoad);
    }

    // Méthode appelée lorsque la scène est chargée
    public override void OnNetworkSpawn()
    {
        // Vérifier si le gestionnaire de scène est dans la scène de jeu
        if (SceneManager.GetActiveScene().name == sceneToLoad)
        {
            // Attendre que tous les joueurs soient prêts
            StartCoroutine(WaitForAllPlayers());
        }
    }
}