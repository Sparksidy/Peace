using UnityEngine;
using System;
using System.Collections;
using System.Linq;
using System.Collections.Generic;

public class LevelManager : MonoBehaviour {
	public static LevelManager Instance{ get; private set;} //  Retreive the current level manager we are looking on

	public Player Player { get; private set;}
	public CameraController Camera{ get; private set;}
	public TimeSpan RunningTime{get{return DateTime.UtcNow - _started;}}

	public int CurrentTimeBonus{
		get{
			var secondDifference = (int)(BonusCutOffSeconds - RunningTime.TotalSeconds);
			return Mathf.Max(0,secondDifference) * BonusSecondsMultiplier;
		}

	}

	private List<Checkpoint> _checkpoints;
	private int _currentCheckpointIndex;
	private DateTime _started;
	private int _savedPoints;


	public Checkpoint DebugSpawn;
	public int BonusCutOffSeconds;//Max amount of time player can go while still receiving bonus points
	public int BonusSecondsMultiplier; // Seconds left over multiplied by how many points you should recieve
	

	public void Awake(){
		_savedPoints = GameManager.Instance.Points;

		Instance = this;

	}
	public void Start(){
		_checkpoints = FindObjectsOfType<Checkpoint> ().OrderBy (t => t.transform.position.x).ToList ();
		_currentCheckpointIndex = _checkpoints.Count > 0 ? 0 : -1;

		Player = FindObjectOfType<Player> ();
		Camera = FindObjectOfType<CameraController> ();

		_started = DateTime.UtcNow;

		// To group each star under a certain checkpoint we find the stars by finding objects of type monobehaviour that implements the IPlayerRespawnListener Interface
		var listeners = FindObjectsOfType<MonoBehaviour> ().OfType<IPlayerRespawnListener> ();
		//Then we loop through each object or in this case the stars
		foreach (var listener in listeners) {
			//We loop backwards starting from checkpoint3 then 2 then 1 and check whether the distance of the object from the checkpoint is  > 0 or  < 0 . If it is positive distance then we assign
			// that star to that checkpoint
			for(var i=_checkpoints.Count -1;i>=0;i--){ 
				var distance = ((MonoBehaviour)listener).transform.position.x - _checkpoints[i].transform.position.x;
				if(distance < 0)
					continue;

				_checkpoints[i].AssignObjectToCheckPoint(listener);
				break;
			}
		}



#if UNITY_EDITOR
		if (DebugSpawn != null)
			DebugSpawn.SpawnPlayer (Player);
		else if (_currentCheckpointIndex != -1)
			_checkpoints [_currentCheckpointIndex].SpawnPlayer (Player);
#else
		if(_currentCheckpointIndex !=-1)
			_checkpoints [_currentCheckpointIndex].SpawnPlayer (Player);
#endif
	}

	public void Update(){



		var isAtLastCheckpoint = _currentCheckpointIndex + 1 >= _checkpoints.Count;
		if (isAtLastCheckpoint)
			return;

		var distanceToNextCheckPoint = _checkpoints [_currentCheckpointIndex + 1].transform.position.x - Player.transform.position.x;
		if (distanceToNextCheckPoint >= 0)
			return;


		_checkpoints [_currentCheckpointIndex].PlayerLeftCheckPoint ();
		_currentCheckpointIndex++;
		_checkpoints [_currentCheckpointIndex].PlayerHitCheckPoint ();


		GameManager.Instance.AddPoints (CurrentTimeBonus);
		_savedPoints = GameManager.Instance.Points;
		_started = DateTime.UtcNow;

	

	}
	public void KillPlayer(){
		StartCoroutine (KillPlayerCo());
	}

	public void GoToNextLevel(string levelname){
		StartCoroutine (GotoNextLevelCo(levelname));
	}

	private IEnumerator GotoNextLevelCo(string levelname){
		Player.FinishLevel ();
		GameManager.Instance.AddPoints (CurrentTimeBonus);

		FloatingText.Show ( " Level Complete !", "CheckpointText", new CenteredTextPositioner (.2f));
		yield return new WaitForSeconds(1f);

		FloatingText.Show (string.Format ("{0} Pointss Biaatch !", GameManager.Instance.Points), "CheckpointText", new CenteredTextPositioner (.1f));
		yield return new WaitForSeconds(5f);

		if (string.IsNullOrEmpty (levelname))
			Application.LoadLevel ("StartScreen");
		else
			Application.LoadLevel (levelname);
	}

	private IEnumerator KillPlayerCo(){
		Player.Kill ();
		Camera.isFollowing = false;
		yield return new WaitForSeconds(2f);

		Camera.isFollowing = true;

		if(_currentCheckpointIndex != -1)
			_checkpoints[_currentCheckpointIndex].SpawnPlayer(Player);


		_started = DateTime.UtcNow;
		GameManager.Instance.ResetPoints (_savedPoints);

	}
}
