using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Checkpoint : MonoBehaviour {

	private List<IPlayerRespawnListener> _listeners;
	public void Awake(){
		_listeners = new List<IPlayerRespawnListener> ();
	}
	public void PlayerHitCheckPoint(){
		StartCoroutine (PlayerHitCheckPointCo (LevelManager.Instance.CurrentTimeBonus));
	}

	private IEnumerator PlayerHitCheckPointCo(int bonus){
		FloatingText.Show ("Checkpoint!", "CheckpointText", new CenteredTextPositioner (.5f));
		yield return new WaitForSeconds (.5f);
		FloatingText.Show (string.Format ("+{0} time bonus!",bonus), "CheckpointText", new CenteredTextPositioner (.5f));

	}
	public void PlayerLeftCheckPoint(){

	}
	public void SpawnPlayer(Player player){
		player.RespawnAt (transform);


		foreach (var listener in _listeners) {
			listener.OnPlayerRespawnInThisCheckPoint(this,player);
		}
	}

	public void AssignObjectToCheckPoint(IPlayerRespawnListener listener){
		_listeners.Add (listener);

	}


}
