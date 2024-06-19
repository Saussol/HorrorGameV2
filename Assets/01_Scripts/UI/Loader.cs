using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loader : MonoBehaviour
{
	private void Update()
	{
		transform.eulerAngles -= Vector3.forward;
	}
}
