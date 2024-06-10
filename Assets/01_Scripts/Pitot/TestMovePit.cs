using UnityEngine;

public class TestMovePit : MonoBehaviour
{
    public float moveSpeed = 6f; // Vitesse de déplacement du personnage

    void Update()
    {
        // Déplacement horizontal
        float horizontalInput = Input.GetAxis("Horizontal");
        transform.Translate(Vector3.right * horizontalInput * moveSpeed * Time.deltaTime);

        // Déplacement vertical
        float verticalInput = Input.GetAxis("Vertical");
        transform.Translate(Vector3.forward * verticalInput * moveSpeed * Time.deltaTime);
    }
}