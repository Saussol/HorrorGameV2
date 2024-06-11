using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class GameManager : NetworkBehaviour
{
    public override void OnNetworkSpawn()
    {
        // Récupère tous les joueurs dans la scène
        List<Player> players = Player.GetAllPlayers();

        // Maintenant, vous pouvez faire ce que vous voulez avec cette liste de joueurs
        foreach (Player player in players)
        {
            Debug.Log("Player found: " + player.name);
            // Vous pouvez accéder aux autres composants ou données du joueur ici

            player.GetComponent<PlayerSeter>().ReSetPlayerComponents();
        }
    }
}
