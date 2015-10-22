using UnityEngine;
using System.Collections;

public class GiveDamageToPlayer : MonoBehaviour {

	public int DamageToGive = 10;

	private Vector2
		_lastposition,
		_velocity; //if an objecvt goes back and forth repeatedly we need to track the velocity of the object in order to determine how much damage the Player can incur.

	public void LateUpdate(){ // Perform after all the other translations (Update method is used to perform all the translations)

		_velocity = (_lastposition - (Vector2)transform.position) / Time.deltaTime;
		_lastposition = transform.position;

	}

	public void OnTriggerEnter2D(Collider2D other){

		var player = other.GetComponent<Player> ();
		if (player == null)
			return;

		player.TakeDamage (DamageToGive,gameObject);
		var controller = player.GetComponent<CharacterController2D1> ();
		var totalVelocity = controller.Velocity + _velocity;

		controller.SetForce(new Vector2(
			-1 * Mathf.Sign(totalVelocity.x)* Mathf.Clamp(Mathf.Abs(totalVelocity.x)*5,10,20),
			-1 * Mathf.Sign(totalVelocity.y)*Mathf.Clamp(Mathf.Abs(totalVelocity.y)*2,0,15)));

	}

}
