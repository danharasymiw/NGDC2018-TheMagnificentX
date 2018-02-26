using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayName : MonoBehaviour {

	public GameObject textMesh;
	public float timer = 9999999999f;

	Player player;

	void Start() {
		player = GetComponent<Player> ();
		Debug.Log ("wat");
		textMesh.GetComponent<Renderer> ().sortingOrder = -99999999;
	}

	// Update is called once per frame
	void Update () {
		Debug.Log ("Setting name...");
		Debug.Log (GetComponent<HFTGamepad> ().playerName);
		string score = player.GetScore ().ToString ();
		Debug.Log ("score is: " + score);
		textMesh.GetComponent<TextMesh> ().text = GetComponent<HFTGamepad> ().playerName + (score != "0" ? ": " + score : "");

		timer -= Time.deltaTime;

		if (timer < 0) {
			textMesh.GetComponent<TextMesh> ().text = "";
			this.enabled = false;
		}
	}
}
