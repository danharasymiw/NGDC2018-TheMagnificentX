using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bull : MonoBehaviour {
	public float damage;
	public float speed = 15f;

	public float dustSpawnRate = 0.5f;
	public float dustSpawnRateVariance = 0.1f;

	public float dustLocationVariance = 1f;

	public float dustSpawnTimer;

	public List<GameObject> dustSprites;

	void Start () {
		dustSpawnTimer = dustSpawnRate + Random.Range(-dustSpawnRateVariance, dustSpawnRateVariance);
	}

	void Update () {
		transform.Translate (Vector2.right * speed * Time.deltaTime);
		dustSpawnTimer -= Time.deltaTime;
		
		if (dustSpawnTimer < 0) {
			GameObject.Instantiate(dustSprites[Random.Range(0, (int)(dustSprites.Count))], 
				new Vector2(transform.position.x + Random.Range(-dustLocationVariance, dustLocationVariance), transform.position.y), transform.rotation);
			dustSpawnTimer = dustSpawnRate;
		}
	}

	public void OnCollisionEnter2D(Collision2D coll) {
		if (coll.gameObject.CompareTag("Player"))
		{
			var player = coll.gameObject.GetComponent<Player>();
			player.die();
		}
	}

	void OnBecameInvisible() {
		Destroy(gameObject);
	}
}
