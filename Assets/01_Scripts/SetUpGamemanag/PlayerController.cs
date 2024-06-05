using Unity.Netcode;
using UnityEngine;

public class PlayerController : NetworkBehaviour
{
    //private NetworkVariable<int> randomNumber = new NetworkVariable<int>(1);// que le host peux changer
    private NetworkVariable<int> randomNumberV2 = new NetworkVariable<int>(1, NetworkVariableReadPermission.Everyone , NetworkVariableWritePermission.Owner);// tt le monde 


    public override void OnNetworkSpawn()
    {
        randomNumberV2.OnValueChanged += (int preiousValue, int newValue) =>
        {
            Debug.Log(OwnerClientId + "; ranfomNumberv2; " + randomNumberV2.Value);
        };
    }

    void Update()
    {
        
        if (IsOwner)
        {
            HandleMovement();

            if (Input.GetKeyDown(KeyCode.T))
            {
                randomNumberV2.Value = Random.Range(0, 100);
            }

           
        }
    }

    private void HandleMovement()
    {
        float move = Input.GetAxis("Vertical");
        float strafe = Input.GetAxis("Horizontal");
        transform.Translate(new Vector3(strafe, 0, move) * Time.deltaTime * 5f);
        //Debug.Log($"Player moved to position: {transform.position}");
    }

    
}
