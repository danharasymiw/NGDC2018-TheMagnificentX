using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour {

	public float fireRate = 2f;

	public int maxAmmo = 6;
	public int ammo;
	public float reloadRate = 3f;
	public float spread = 3f;

	public GameObject bullet;
	public Transform bulletSpawnPosition;

	public float fireRateTimer;
	public float reloadTimer;

	public int numberOfBulletsPerShot = 1;

	public string soundName;

	public bool isDroppable = false;
	public GameObject weaponPickup;
	
	void Start() {
		ammo = maxAmmo;
		//Debug.Log ("WEAPON START");
		fireRateTimer = 0;
		reloadTimer = 0;
	}
	
	// Update is called once per frame
	void Update () {
		fireRateTimer -= Time.deltaTime;
		reloadTimer -= Time.deltaTime;

		if (ammo <= 0) {
			reloadTimer = reloadRate;
			ammo = maxAmmo;
		}

		Debug.DrawRay (transform.position, transform.right);
	}

	public bool fire(Player shotBy) {
		if (ammo > 0 && fireRateTimer < 0 && reloadTimer < 0) {

			fireBullets (shotBy);
			
			fireRateTimer = fireRate;
			ammo--;
			if (ammo <= 0)
			{
				shotBy.onReload();
			}
			return true;
		}
		return false;
	}

	public virtual void fireBullets(Player shotBy) {
		for (int i = 0; i < numberOfBulletsPerShot; i++) {
			var proj = Instantiate (bullet, bulletSpawnPosition.position, transform.rotation);
			proj.transform.Rotate (new Vector3 (0, 0, Random.Range(-spread, spread)));
			proj.GetComponent<Projectile>().creator = shotBy;
		}
	}		
}
