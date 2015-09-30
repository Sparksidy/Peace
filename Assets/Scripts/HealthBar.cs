﻿using UnityEngine;
using System.Collections;

public class HealthBar : MonoBehaviour {

	public Player Player;
	public Transform ForeGroundSprite;
	public SpriteRenderer ForeGroundRenderer;
	public Color MaxHealthColor = new Color (255 / 255f, 63 / 255f, 63 / 255f);
	public Color MinHealthColor = new Color (64 / 255f, 137 / 255f, 255 / 255f);

	public void Update(){
		var HealthPercent = Player.Health / (float)Player.MaxHealth;

		ForeGroundSprite.localScale = new Vector3 (HealthPercent, 1, 1);
		ForeGroundRenderer.color = Color.Lerp (MaxHealthColor, MinHealthColor, HealthPercent);

	}


}