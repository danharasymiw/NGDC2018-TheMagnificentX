using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stampede : MonoBehaviour {
	
	public float stampedeTimerRangeMin = 10f;
	public float stampedeTimerRangeMax = 30f;

	public float stampedeTimerRangeMaxDecrementValue = 2f;
	public float stampedeTimer;

	public bool stampedeStarted = false;

	public float indicatorFlashTime = 2f;

	public GameObject bull;

	public float bullOffsetX = 1f;
	public float bullOffsetY = 0.5f;

	public GameObject indicator_words;
	public GameObject indicator;

	public float flashRate = 0.5f;
	public float flashTimer;
	
	private AudioSource source;

	void Start () {
		stampedeTimer = Random.Range(stampedeTimerRangeMin, stampedeTimerRangeMax);
		flashTimer = flashRate;

		source = GetComponent<AudioSource>();
	}

	// Update is called once per frame
	void Update () {
		stampedeTimer -= Time.deltaTime;
		

		

		if (stampedeTimer < 0 && !stampedeStarted) {
			 stampedeStarted = true;
			 indicator_words.SetActive(false);
			 indicator.SetActive(false);
		}

		if (stampedeTimer < indicatorFlashTime && stampedeTimer > 0) {
			flashTimer -= Time.deltaTime;

			if (flashTimer < 0) {
				indicator.SetActive(!indicator.active);
				indicator_words.SetActive(!indicator_words.active);
				flashTimer = flashRate;
			}
			
		}

		if (stampedeStarted) {
			source.Play();
			Instantiate(bull, new Vector2(transform.position.x - bullOffsetX, transform.position.y + bullOffsetY), transform.rotation);
			Instantiate(bull, transform.position, transform.rotation);
			Instantiate(bull, new Vector2(transform.position.x + bullOffsetX, transform.position.y + bullOffsetY + 0.1f), transform.rotation);

			stampedeTimer = Random.Range(stampedeTimerRangeMin, stampedeTimerRangeMax);
			stampedeTimerRangeMax -= stampedeTimerRangeMaxDecrementValue;
			if (stampedeTimerRangeMax < stampedeTimerRangeMin) {
				stampedeTimerRangeMax = stampedeTimerRangeMin;
			}
			stampedeStarted = false;
		}
	}
}
