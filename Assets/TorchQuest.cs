using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TorchQuest : MonoBehaviour
{
    private bool isPlayerNearby = false;
    public GameObject fireVFX;
    public Transform vfxSpawnPoint;
    private CampFire campFire;

    void Start()
    {
        campFire = FindObjectOfType<CampFire>();

        if (campFire == null)
        {
            Debug.LogError("No CampFire component found in the scene!");
        }
    }

    void Update()
    {
        if (isPlayerNearby && Input.GetKeyDown(KeyCode.E) && campFire.hasFire == true)
        {
            Interact();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = true;
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = false;
        }
    }

    void Interact()
    {
        Instantiate(fireVFX, vfxSpawnPoint.position, vfxSpawnPoint.rotation);
    }
}
