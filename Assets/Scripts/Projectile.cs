using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

	public float damage;
	public float speed = 15f;

	public Vector2 dir;

	public Player creator;
	
	// Use this for initialization

	
	// Update is called once per frame
	public virtual void Update () {
		transform.Translate (-Vector2.up * speed * Time.deltaTime);
	}

	public virtual void OnCollisionEnter2D(Collision2D coll) {
		//Debug.Log ("collided!");
		if (coll.gameObject.CompareTag("Player")) {
			var player = coll.gameObject.GetComponent<Player> ();
			player.die();

			creator.onPlayerKill();
		}

		if (coll.gameObject.CompareTag("bullet")) {
			Physics2D.IgnoreCollision(coll.gameObject.GetComponent<Collider2D>(), GetComponent<Collider2D>());
			return;
		}
		if (coll.gameObject.CompareTag("wood"))
		{
			creator.onBulletHitWall("wood");
		}

		Destroy (gameObject);
	}

	void OnBecameInvisible() {
		Destroy (gameObject);
	}
}
