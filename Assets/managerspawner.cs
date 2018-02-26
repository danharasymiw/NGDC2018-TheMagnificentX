using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class managerspawner : MonoBehaviour {

	// Use this for initialization
	void Awake ()
	{
		GameObject.Instantiate(Resources.Load("Manager"));
	}
	
}
