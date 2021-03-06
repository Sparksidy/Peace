﻿using UnityEngine;
using System.Collections;

public class SimpleEnemyAI : MonoBehaviour,ITakeDamage,IPlayerRespawnListener {

	public float Speed;
	public float FireRate = 1;
	public Projectile Projectile;
	public GameObject DestroyEffect;
	public AudioClip ShootSound;

	private CharacterController2D1 _controller;
	private Vector2 _direction;
	private Vector2 _startPosition;
	private float _canFireIn;

	public void Start(){
		_controller = GetComponent<CharacterController2D1> ();
		_direction = new Vector2 (-1, 0);
		_startPosition = transform.position;
	}

	public void Update(){
		_controller.SetHorizontalForce (_direction.x * Speed);
		if (((_direction.x) < 0 && _controller.State.IsCollidingLeft) || (_direction.x >0 && _controller.State.IsCollidingRight)) {
			_direction = -_direction;
			transform.localScale = new Vector3(-transform.localScale.x,transform.localScale.y,transform.localScale.z);
		}
		if((_canFireIn -= Time.deltaTime) >0)
			return;

		var raycast = Physics2D.Raycast(transform.position,_direction, 20, 1 << LayerMask.NameToLayer("Player"));
		if(!raycast)
			return;

		var projectile = (Projectile) Instantiate(Projectile,transform.position,transform.rotation);
		projectile.Initialise(gameObject,_direction,_controller.Velocity);
		_canFireIn = FireRate;


		if (ShootSound != null)
			AudioSource.PlayClipAtPoint (ShootSound, transform.position);
	}


	public void TakeDamage(int damage,GameObject instigator){
		Instantiate (DestroyEffect,transform.position,transform.rotation);
		gameObject.SetActive(false);

	}

	public void OnPlayerRespawnInThisCheckPoint(Checkpoint checkpoint,Player player){
		_direction = new Vector2 (-1, 0);
		transform.localScale = new Vector3 (1, 1, 1);
		transform.position = _startPosition;
		gameObject.SetActive(true);

	}






}
