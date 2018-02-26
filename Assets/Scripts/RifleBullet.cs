using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RifleBullet : Projectile {

	public override void OnCollisionEnter2D(Collision2D coll) {
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
	}
}
