using System.Collections;
using System.Collections.Generic;
using HappyFunTimes;
using UnityEngine;
using UnityEngine.Experimental.Rendering;
using UnityEngine.UI;

public class Player : MonoBehaviour {
	public float speed = 10.0f;

	HFTInput m_hftInput;
	HFTSoundPlayer m_soundPlayer;

	public Weapon weapon;
	public Transform gunHolder;

	public int health = 100;

	public Vector2 playerDir;

	Manager manager;

	public int score = 0;

	public Transform body;
	public Transform head;

	public bool isActive = true;
	public bool setInactiveNextFrame = false;

	public string networkId;
	public GameObject defaultWeapon;

	// Use this for initialization

	IEnumerator wait()
	{
		yield return new WaitForSeconds(1);
	}
	
	void Start () {
		//Debug.Log ("player start");
		DontDestroyOnLoad (this.gameObject);
		m_hftInput = GetComponent<HFTInput>();
		m_soundPlayer = GetComponent<HFTSoundPlayer>();
		playerDir = Vector2.right;

		manager = GameObject.Find ("Manager").GetComponent<Manager> ();
		manager.addPlayer (this.gameObject);
		GameObject.Find("CharacterCreator").GetComponent<CharacterCreator>().createCharacter(body, head, GetComponent<HFTGamepad>().color);

		if (Application.loadedLevel == 0) {
			GameObject.Find ("StagingAreaManager").GetComponent<StagingAreaManager> ().addPlayer (this.gameObject);
		}

		transform.localScale = new Vector2(0.5f, 0.5f);
	}
	
	// Update is called once per frame
	void Update () {
		if (setInactiveNextFrame)
		{
			gameObject.SetActive(false);
			setInactiveNextFrame = false;
		}

		if (isActive) {
			float dx = (Input.GetAxis ("Horizontal") + m_hftInput.GetAxis("Horizontal")) * speed * Time.deltaTime;
			float dy = (Input.GetAxis ("Vertical") - m_hftInput.GetAxis ("Vertical")) * speed * Time.deltaTime;

			GetComponent<Rigidbody2D> ().velocity = new Vector2 (dx * speed, dy * speed);
			//GetComponent<Rigidbody2D> ().AddForce (new Vector2 (dx * 10, dy * 10));
			if (Input.GetKey (KeyCode.Space)) {
				weapon.fire (this);
			}


			var horizontalDirection = m_hftInput.GetAxis("Horizontal2");
			var verticalDirection = m_hftInput.GetAxis("Vertical2");

			if (horizontalDirection != 0 || verticalDirection != 0)
			{
				var angle = 
					horizontalDirection > 0 && verticalDirection == 0 ?  90
					: horizontalDirection < 0 && verticalDirection == 0 ? -90
					: horizontalDirection > 0 && verticalDirection < 0 ? 135
					: horizontalDirection < 0 && verticalDirection > 0 ? -35
					: horizontalDirection > 0 && verticalDirection > 0 ? 45
					: horizontalDirection < 0 && verticalDirection < 0 ? -135
					: horizontalDirection == 0 && verticalDirection > 0 ? 0
					: horizontalDirection == 0 && verticalDirection < 0 ? 180
					: 0f;

				transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
				if (weapon.fire(this))
				{
					m_soundPlayer.PlaySound(weapon.soundName);
				}
			}



			// keep the player in the bounds of the game
			Vector3 pos = Camera.main.WorldToViewportPoint (transform.position);
			pos.x = Mathf.Clamp01(pos.x);
			pos.y = Mathf.Clamp01(pos.y);
			transform.position = Camera.main.ViewportToWorldPoint(pos);
		}

	}

	public void resetToDefaultWeapon () {
		Vector2 old_scale = weapon.transform.localScale;
		Destroy (weapon.gameObject);
		GameObject wep = Instantiate (defaultWeapon, gunHolder.position, gunHolder.rotation);
		wep.transform.parent = transform;
		wep.transform.localScale = old_scale;
		weapon = wep.GetComponent<Weapon> ();
	}

	void OnTriggerEnter2D(Collider2D coll) {
		if (coll.CompareTag("weaponPickup")) {
			Vector2 old_scale = weapon.transform.localScale;
			Destroy (weapon.gameObject);
			GameObject wep = Instantiate (coll.GetComponent<WeaponPickup> ().weapon, gunHolder.position, gunHolder.rotation);
			wep.transform.parent = transform;
			wep.transform.localScale = old_scale;
			weapon = wep.GetComponent<Weapon> ();
			Destroy (coll.gameObject);
		}
	}

	public void die() {
		manager.playerDied (gameObject);
		//Debug.Log ("player died");
		m_soundPlayer.PlaySound("player_death");

		if (weapon.isDroppable) {
			GameObject.Instantiate (weapon.weaponPickup, transform.position, transform.rotation);
		}
		GetComponent<HFTGamepad>().controllerOptions.controllerType = HFTGamepad.ControllerType.c_dead;

		setInactiveNextFrame = true;
		

		
	}

	public void onRoundStart()
	{
		var gamepad = GetComponent<HFTGamepad>();
		gamepad.controllerOptions.controllerType = HFTGamepad.ControllerType.c_2dpad;
		
		
		resetToDefaultWeapon();
	}
	
	public void onPlayerKill()
	{
		//Debug.Log("Player Killed");
		m_soundPlayer.PlaySound("killed_other_player");
	}

	public void onBulletHitWall(string name)
	{
		m_soundPlayer.PlaySound("bullet_hit_wall_" + name);
	}

	public void onReload()
	{
		m_soundPlayer.PlaySound("reloading");
	}
	
	public void InitializeNetPlayer(SpawnInfo spawnInfo)
	{
		networkId = spawnInfo.netPlayer.GetSessionId();

		spawnInfo.netPlayer.OnDisconnect += (sender, args) =>
		{
			if (Application.loadedLevel == 0) {
				GameObject.Find ("StagingAreaManager").GetComponent<StagingAreaManager> ().RemovePlayer (gameObject);
			}
			else
			{
				GameObject.Find("Manager").GetComponent<Manager>().RemovePlayer(gameObject);
			}
			
		};
	}

	public int GetScore() {
		if (!manager.playerScores.TryGetValue (networkId, out score)) {
			score = 0;
			//Debug.Log ("Score is 0!");
		} 
		//Debug.Log ("Returning score!");
		return score;
	}

	void OnCollisionEnter2D(Collision2D coll) {
		if (!isActive) {
			manager.spawnPlayer (gameObject);
		}
	}
}
