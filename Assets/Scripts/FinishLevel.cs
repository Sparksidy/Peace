﻿using UnityEngine;


public class FinishLevel : MonoBehaviour {

	public string Levelname;

	public void OnTriggerEnter2D(Collider2D other){
		if (other.GetComponent<Player> () == null)
			return;

		LevelManager.Instance.GoToNextLevel (Levelname);
	}
}
