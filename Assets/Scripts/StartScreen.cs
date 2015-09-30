using UnityEngine;
using System.Collections;

public class StartScreen : MonoBehaviour {

	public string firstLevel;

	public void Update(){
		if (!Input.GetMouseButtonDown (0))
			return;

		Application.LoadLevel (firstLevel);
	}
}
