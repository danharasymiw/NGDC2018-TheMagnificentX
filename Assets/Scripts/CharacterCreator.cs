using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterCreator : MonoBehaviour {
	public GameObject maleBody;
	public List<GameObject> maleHeads = new List<GameObject>();

	public GameObject femaleBody;
	public List<GameObject> femaleHeads = new List<GameObject>();

	public void createCharacter(Transform body, Transform head, Color color) {

		// male character
		GameObject playerBody;
		GameObject playerHead;

		if (Random.Range (0, 2) > 0.5f) {
			playerBody = maleBody;
			playerHead = maleHeads [(int)Random.Range (0, maleHeads.Count)];
		}
		//female character
		else {
			playerBody = femaleBody;
			playerHead = femaleHeads [(int)Random.Range (0, femaleHeads.Count)];
		}

		// playerBody.GetComponent<SpriteRenderer>().sortingOrder = 150;
		// playerHead.GetComponent<SpriteRenderer>().sortingOrder = 151;
		// playerHead.GetComponentInChildren<SpriteRenderer>(true).sortingOrder = 152;

		GameObject spawnedBody = GameObject.Instantiate (playerBody, body.transform.position, body.transform.rotation);
		GameObject spawnedHead = GameObject.Instantiate (playerHead, head.transform.position, head.transform.rotation);

		spawnedBody.transform.parent = body.transform;
		spawnedHead.transform.parent = head.transform;

		spawnedHead.GetComponent<SpriteRenderer> ().color = color;
	}
}
