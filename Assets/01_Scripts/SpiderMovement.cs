using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderMovement : MonoBehaviour
{
    public float speed = 5;

    // Update is called once per frame
    void Update()
    {
		transform.position += transform.forward * Time.deltaTime * speed;
		transform.eulerAngles += transform.up * Time.deltaTime * speed * 2;
	}
}
