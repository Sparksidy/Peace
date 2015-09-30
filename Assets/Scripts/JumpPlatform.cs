using UnityEngine;
using System.Collections;

public class JumpPlatform : MonoBehaviour {
	public float JumpMagnitude = 20f;

	public void ControllerEnter2D(CharacterController2D1 controller){
		controller.SetVerticalForce (JumpMagnitude);
	}

}
