using UnityEngine;
using System.Collections;

public class GameOver : MonoBehaviour {
	double timer = 0.0;

	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		timer += Time.deltaTime;
		if (timer > 5.0) {
			Application.LoadLevel("Scene1");
		}
	}
}
