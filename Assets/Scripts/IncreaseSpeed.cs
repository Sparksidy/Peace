using UnityEngine;
using System.Collections;

public class IncreaseSpeed : MonoBehaviour {
	

	public void OnTriggerEnter2D(Collider2D  other){
		var player = other.GetComponent<Player>();
		player.MaxSpeed = 8;
	}
}
