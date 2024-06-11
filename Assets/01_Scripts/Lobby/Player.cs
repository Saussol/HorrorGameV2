using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Vous pouvez ajouter d'autres informations du joueur ici

    // Méthode statique pour récupérer tous les joueurs dans la scène
    public static List<Player> GetAllPlayers()
    {
        List<Player> players = new List<Player>();

        // Trouve tous les GameObjects avec le composant Player attaché
        Player[] allPlayers = FindObjectsOfType<Player>();

        foreach (Player player in allPlayers)
        {
            players.Add(player);
        }

        return players;
    }
}
