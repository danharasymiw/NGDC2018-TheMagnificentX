using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using UnityEngine.SceneManagement;

public class StagingAreaManager : MonoBehaviour {

	public List<Transform> spawnPositions;
	int playerCount = 0;
	List<GameObject> players = new List<GameObject> ();

	private AudioSource introMusic;
	
	public GameObject text;

	public float musicTimer = 20;

	bool textChanged = false;

	public void addPlayer(GameObject player) {
		player.GetComponent<Player> ().isActive = false;
		//player.transform.position = spawnPositions [playerCount++].position;
		GameObject.Find ("Manager").GetComponent<Manager> ().spawnPlayer(player);
		playerCount++;
		players.Add (player);

		if (players.Count > 1 && !textChanged) {
			text.GetComponent<Text>().text = "Press Space to Start!";
		}
	}

	public void RemovePlayer(GameObject player)
	{
		players.Remove(player);
		Destroy(player);
		playerCount--;
	}

	void Start() {
		introMusic = GetComponent<AudioSource>();
	}

	void Update()
	{
		musicTimer -= Time.deltaTime;

		if (musicTimer < 0)
		{
			introMusic.Play();
			musicTimer = Random.Range(20, 30);
		}
		
		if (Input.GetKey (KeyCode.Space) && playerCount >= 2) {
			foreach (GameObject player in players) {
				player.GetComponent<Player> ().isActive = true;
			}

			GameObject.Find ("Manager").GetComponent<Manager> ().gameStarted = true;
			SceneManager.LoadScene ("level" + (int) Random.Range(1, 3));
		}
	}
}
