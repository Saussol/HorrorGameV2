using UnityEngine;

public class TestMovePit : MonoBehaviour
{
    public float moveSpeed = 6f; // Vitesse de d�placement du personnage

    void Update()
    {
        // D�placement horizontal
        float horizontalInput = Input.GetAxis("Horizontal");
        transform.Translate(Vector3.right * horizontalInput * moveSpeed * Time.deltaTime);

        // D�placement vertical
        float verticalInput = Input.GetAxis("Vertical");
        transform.Translate(Vector3.forward * verticalInput * moveSpeed * Time.deltaTime);
    }
}