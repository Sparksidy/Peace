using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
	 
	private bool _isFacingRight;
	private CharacterController2D1 _controller;
	private float normalizedHorizontalSpeed;
	// It is 0,1, or -1 depending on whether the player is moving right or left

	public float MaxSpeed = 8f;
	public float speedAccelerationOnGround = 10f;
	public float speedAccelerationInAir = 5f;
	public int MaxHealth ;
	public GameObject OuchEffect;
	public Projectile Projectile;
	public float FireRate;
	public Transform ProjectileFireLocation;
	public AudioClip PlayerHitSound;
	public AudioClip PlayerShootSound;
	public AudioClip PlayerHealthSound;







	public int Health{ get; private set;}
	public bool isDead{ get; private set;}

	private float _canFireIn;


	public void Awake(){
		// Collider2D collider = GetComponent<Collider2D>();

		_controller = GetComponent<CharacterController2D1> ();
		_isFacingRight = transform.localScale.x > 0;

		Health = MaxHealth;
		Debug.Log (Health);
	}

	public void Update(){

		_canFireIn -= Time.deltaTime;

		//if(!isDead)
		HandleInput();


		var movementFactor = _controller.State.IsGrounded ? speedAccelerationOnGround : speedAccelerationInAir;

		if (isDead)
			_controller.SetHorizontalForce (0);
		else
			_controller.SetHorizontalForce (Mathf.Lerp (_controller.Velocity.x, normalizedHorizontalSpeed * MaxSpeed, Time.deltaTime * movementFactor)); //Lerping speed of the character



	} 
	public void FinishLevel(){
		enabled = false;
		_controller.enabled = false;
	
	}


	public void Kill(){

		_controller.HandleCollisions = false;

		GetComponent<Collider2D>().enabled = false;

		isDead = true;
		Health = 0;


		_controller.SetForce (new Vector2 (0, 10));
	}
	public void RespawnAt(Transform spawnPoint){
		if (!_isFacingRight)
			Flip ();
		isDead = false;

		GetComponent<Collider2D>().enabled = true;
		_controller.HandleCollisions = true;
		Health = MaxHealth;

		transform.position = spawnPoint.position;


	}


	public void TakeDamage(int damage){
		AudioSource.PlayClipAtPoint (PlayerHitSound, transform.position);

		FloatingText.Show (string.Format ("-{0}", damage), "PlayerTakeDamageText", new FromWorldPointTextPositioner (Camera.main, transform.position, 2f, 60));

		Instantiate (OuchEffect, transform.position, transform.rotation);
		Health -= damage;

		if (Health <= 0)
			LevelManager.Instance.KillPlayer ();
	}


	public void GiveHealth(int health,GameObject instigator){
		AudioSource.PlayClipAtPoint (PlayerHealthSound, transform.position);
		FloatingText.Show (string.Format ("+{0}!",health), "PlayerGotHealthText", new FromWorldPointTextPositioner (Camera.main, transform.position, 2f, 60));
		Health = Mathf.Min (Health + health, MaxHealth);


	}


	private void HandleInput(){
		if (Input.GetKey (KeyCode.RightArrow)) {
			normalizedHorizontalSpeed = 1;
			if (!_isFacingRight) {
				Flip ();
			}
		} else if (Input.GetKey (KeyCode.LeftArrow)) {
			normalizedHorizontalSpeed = -1;
			if (_isFacingRight) {
				Flip ();
			}
		} else {
			normalizedHorizontalSpeed = 0;

		}

		if (_controller.CanJump && Input.GetKey (KeyCode.Space)) {
			_controller.Jump();
		}

		if (Input.GetKey(KeyCode.LeftControl))
			FireProjectile ();

	}

	private void FireProjectile (){
		if (_canFireIn > 0)
			return;

		var direction = _isFacingRight ? Vector2.right : -Vector2.right;

		var projectile = (Projectile)Instantiate (Projectile, ProjectileFireLocation.position, ProjectileFireLocation.rotation);
		projectile.Initialise (gameObject, direction, _controller.Velocity);

		//projectile.transform.localScale = new Vector3 (_isFacingRight ? 1 : -1, 1, 1);

		_canFireIn = FireRate;

		AudioSource.PlayClipAtPoint (PlayerShootSound, transform.position);
	}


	private void Flip(){
		transform.localScale = new Vector3 (-transform.localScale.x, transform.localScale.y, transform.localScale.z);
		_isFacingRight = transform.localScale.x > 0;

	}

}
