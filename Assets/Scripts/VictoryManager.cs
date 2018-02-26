using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class VictoryManager : MonoBehaviour {

	Manager manager;
	public Text playerText;
	// Use this for initialization
	void Start () {
		manager = GameObject.Find ("Manager").GetComponent<Manager> ();
		playerText.text = GetWinningPlayerName() + " wins!";

	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKey (KeyCode.Space))
		{
			manager.playerScores = new Dictionary<string, int>();
			SceneManager.LoadScene ("level1", LoadSceneMode.Single);
		}
	}

	string GetWinningPlayerName() {
		int highestScore = 0;
		string highestNetworkId = "";

		foreach (KeyValuePair<string, int> entry in manager.playerScores) {
			if (entry.Value > highestScore) {
				highestScore = entry.Value;
				highestNetworkId = entry.Key;
			}
		}

		GameObject winnerGO = null;

		foreach (GameObject player in manager.players) {
			if (player.GetComponent<Player> ().networkId == highestNetworkId) {
				return player.GetComponent<HFTGamepad> ().playerName;
			}
		}

		return "No Name";
	}
}
