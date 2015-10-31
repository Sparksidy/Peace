using UnityEngine;
using System.Collections;


/// <summary>
/// A bag to add data
/// </summary>
public class GameManager {
	private static GameManager _instance;
	public static GameManager Instance {get{return _instance ?? (_instance = new GameManager());}} // SingleTon Class




	public int Points{get; private set;}
	public int Lives{ get; private set;}

	//Sanity check
	//Empty cTor: nobody other than Game Manager can instance itself. The only instantiation of game Manager is in this ctor itself,declaring it to be private makes sure that nobody has priveleges to instantiate it.
	private GameManager(){

	}

	public void Reset(){
		Points = 0;
	}
	public void ResetPoints(int points){
		Points = points;
	}
	public void AddPoints(int pointsToAdd){
		Points += pointsToAdd;
	}
	public void ResetLives(int lives){
		Lives = lives;

	}

}
