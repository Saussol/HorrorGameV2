using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
//using Unity.Netcode.Editor;
using UnityEngine;

public class QuestLeader : NetworkBehaviour
{
    
    public TextMeshProUGUI textMeshProUGUIA;
    
    public TextMeshProUGUI textMeshProUGUIB;

    public static QuestLeader instance;

    public void Awake()
    {
         instance = this;

    }
}
