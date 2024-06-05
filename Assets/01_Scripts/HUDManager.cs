using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HUDManager : MonoBehaviour
{
	public static HUDManager Instance { get; private set; }

	[SerializeField] private GameObject indication;
	private TMP_Text indicationText;

	private void Awake()
	{
		if (Instance != null && Instance != this)
		{
			Destroy(this);
		}
		else
		{
			Instance = this;
		}
	}

	private void Start()
	{
		indicationText = indication.GetComponent<TMP_Text>();
		HideIndication();
	}

	public void DiplayIndication(string action)
	{
		if (indicationText.text != $"Press E to {action}")
		{
			indication.SetActive(true);
			indicationText.text = $"Press E to {action}";
		}
	}

	public void HideIndication()
	{
		if (indication.activeSelf)
		{
			indication.SetActive(false);
			if (indicationText == null) indicationText = indication.GetComponent<TMP_Text>();
			indicationText.text = "";
		}
	}
}
