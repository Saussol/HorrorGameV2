using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LobbyPlayer : MonoBehaviour
{
    public TextMeshProUGUI playerNameText;

    public void Initialize(string playerName)
    {
        playerNameText.text = playerName;
    }
}
