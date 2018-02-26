using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotgunBullet : Projectile {

	public float lifetime = 0.2f;
	
	// Update is called once per frame
	public override void Update () {
		transform.Translate (-Vector2.up * speed * Time.deltaTime);
		lifetime -= Time.deltaTime;
		if (lifetime < 0) {
			Destroy(gameObject);
		}
	}
}
