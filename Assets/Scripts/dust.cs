using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class dust : MonoBehaviour {

	public float fadeRate = 0.5f;
	public float spinRate = 10.0f;
	public float spinRateVariance = 5f;

	SpriteRenderer spriteRenderer;

	void Start () {
		spriteRenderer = this.GetComponent<SpriteRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
		spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, spriteRenderer.color.a - (fadeRate * Time.deltaTime));
		transform.Rotate(new Vector3(0f, 0f, (spinRate - Random.Range(-spinRateVariance, spinRateVariance)) * Time.deltaTime));
		if (spriteRenderer.color.a <= 0f) {
			Destroy(this.gameObject);
		}
	}
}
