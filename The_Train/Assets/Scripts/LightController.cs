using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public enum ActionType { Flicker, Change };

public class Lights : MonoBehaviour
{
	[SerializeField] public ActionType type;
	[Header("Flicker")]
	[SerializeField] public GameObject lightObject;
	[Header("Change")]
	[SerializeField] public GameObject lightOff;
	[SerializeField] public GameObject lightOn;

	private bool safeModeOn;
	private Light2D flickerlight;

	private void Start()
	{
		safeModeOn = PlayerPrefs.GetInt("SafeModeOn", 0) == 1;
		
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.name == "Player")
		{
			switch (type)
			{
				case ActionType.Change:
					{
						StartCoroutine(changeLight());
						break;
					}
				case ActionType.Flicker:
					{
						StartCoroutine(flickerLight());
						break;

					}
			}
		}
	}

	IEnumerator flickerLight(bool leaveOn = true)
	{
		if(!safeModeOn)
		{
			flickerlight = lightObject.GetComponent<Light2D>();
			float seconds = 0.2f;
			flickerlight.intensity = 0;
			yield return new WaitForSeconds(seconds);
			for (int i = 0; i < 2; i++)
			{
				flickerlight.intensity = 0.9f;
				yield return new WaitForSeconds(seconds);
				flickerlight.intensity = 0;
				yield return new WaitForSeconds(seconds);
			}
			if(leaveOn)
				flickerlight.intensity = 0.9f;
		}
	}

	IEnumerator changeLight()
	{
		StartCoroutine(flickerLight(false));
		yield return new WaitForSeconds(2f);
		Light2D light1 = lightOff.GetComponent<Light2D>();
		Light2D light2 = lightOn.GetComponent<Light2D>();
		light1.enabled = false;
		light2.enabled = true;
		
	}
}
