using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;

public class StagingAreaManager : MonoBehaviour {

	public List<Transform> spawnPositions;
	int playerCount = 0;
	List<GameObject> players = new List<GameObject> ();

	public void addPlayer(GameObject player) {
		player.GetComponent<Player> ().isActive = false;
		player.transform.position = spawnPositions [playerCount++].position;
		players.Add (player);
	}

	void Update() {
		if (Input.GetKey (KeyCode.Space) && playerCount >= 2) {
			foreach (GameObject player in players) {
				player.GetComponent<Player> ().isActive = true;
			}

			GameObject.Find ("Manager").GetComponent<Manager> ().gameStarted = true;
			SceneManager.LoadScene ("level1");
		}
	}
}
