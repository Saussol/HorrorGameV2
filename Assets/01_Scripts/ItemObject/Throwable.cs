using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Throwable : ItemObject
{
	private Rigidbody rb;
	private bool thrown;

	private void Awake()
	{
		rb = GetComponent<Rigidbody>();
	}

	public override void Use(Vector3 throwDirection, Vector3 velocity)
	{
		rb.AddForce(throwDirection + velocity * .75f, ForceMode.Impulse);
		thrown = true;
	}

	private void OnCollisionEnter(Collision collision)
	{
		if (thrown)
		{
			if (collision.transform.CompareTag("Player"))
			{
				collision.transform.GetComponent<CharacterMovement>().StunPlayer();
			}
			thrown = false;
		}
	}
}
