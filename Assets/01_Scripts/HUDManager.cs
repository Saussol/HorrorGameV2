using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HUDManager : MonoBehaviour
{
	[SerializeField] private GameObject indication;
	private TMP_Text indicationText;

	private void Start()
	{
		indicationText = indication.GetComponent<TMP_Text>();
		HideIndication();
	}

	public void DiplayIndication(string action, bool customDisplay)
	{
		if (customDisplay)
		{
			if(indicationText.text != action)
			{
				indication.SetActive(true);
				indicationText.text = action;
			}
		}
		else
		{
			if (indicationText.text != $"Press E to {action}")
			{
				indication.SetActive(true);
				indicationText.text = $"Press E to {action}";
			}
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
