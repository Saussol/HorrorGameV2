using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class GameManager : NetworkBehaviour
{
    public override void OnNetworkSpawn()
    {
        // R�cup�re tous les joueurs dans la sc�ne
        List<Player> players = Player.GetAllPlayers();

        // Maintenant, vous pouvez faire ce que vous voulez avec cette liste de joueurs
        foreach (Player player in players)
        {
            Debug.Log("Player found: " + player.name);
            // Vous pouvez acc�der aux autres composants ou donn�es du joueur ici

            player.GetComponent<PlayerSeter>().ReSetPlayerComponents();
        }
    }
}
