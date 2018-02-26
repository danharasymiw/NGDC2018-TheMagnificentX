using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Manager : MonoBehaviour {

	public List<GameObject> players = new List<GameObject>();
	List<GameObject> alivePlayers = new List<GameObject>();
	public Dictionary<string, int> playerScores = new Dictionary<string, int>();
	
	public bool gameStarted = false;

	// Use this for initialization
	void Start () {
		DontDestroyOnLoad (this.gameObject);
		alivePlayers = new List<GameObject> ();
		//Debug.Log ("Start finished");

		SceneManager.sceneLoaded += OnSceneLoaded;
	}
	
	// Update is called once per frame
	void Update () {
		//Debug.Log (alivePlayers.Count);
		if (gameStarted && alivePlayers.Count <= 1) {
			if (alivePlayers.Count > 0) {
				var id = alivePlayers [0].GetComponent<Player> ().networkId;
				PlayerWon(id);
			}

			//Debug.Log ("load new level!");
			gameStarted = false;
			SceneManager.LoadScene ("level" + (int) Random.Range(1, 3), LoadSceneMode.Single);
		}
	}

	public void addPlayer(GameObject player) {
		players.Add (player);
		alivePlayers.Add (player);
	}

	public void RemovePlayer(GameObject player)
	{
		players.Remove(player);
		alivePlayers.Remove(player);
		Destroy(player);
	}

	public void startNewRound() {
		
		alivePlayers = new List<GameObject> ();
		// re-add the players to the alive list
		foreach (GameObject player in players) {
			player.GetComponent<Player>().resetToDefaultWeapon();
			alivePlayers.Add (player);
			spawnPlayer (player);
		}

		StartCoroutine (DelayRoundStart());
	}

	public void spawnPlayer(GameObject player) {
		// move the player somewhere
		Vector2 spawnPos = new Vector2(Random.Range(0, Screen.width), Random.Range(0, Screen.height));

		player.transform.position = Camera.main.ScreenToWorldPoint (new Vector3 (spawnPos.x, spawnPos.y, 100));

	}

	public void playerDied(GameObject player) {
		alivePlayers.Remove (player);
	}

	void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
		//Debug.Log ("loaded scene");
		if (scene.name.StartsWith ("level")) {
			startNewRound ();
			gameStarted = true;
		}
	}

	public void PlayerWon(string networkId)
	{
		int score;
		if (playerScores.TryGetValue(networkId, out score))
		{
			playerScores[networkId] = score + 1;
		}
		else
		{
			playerScores.Add(networkId, 1);
		}
	}

	IEnumerator DelayRoundStart() {
		foreach (GameObject player in players) {
			player.SetActive (true);
			player.GetComponent<Player> ().isActive = false;
			player.GetComponent<DisplayName> ().timer = 5f;
			player.GetComponent<DisplayName> ().enabled = true;
			player.GetComponent<Rigidbody2D> ().velocity = new Vector2 (0, 0);
			player.transform.rotation = new Quaternion();
		}

		yield return new WaitForSeconds (5);

		foreach (GameObject player in players) {
			player.GetComponent<Player> ().isActive = true;
		}
	}
}
