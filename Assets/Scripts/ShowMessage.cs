using UnityEngine;
using System.Collections;

public class ShowMessage : MonoBehaviour {
	
	public GameObject Effect;

	public AudioClip HitStarSound;

	public string Message = "Press";
	
	
	
	
	public void OnTriggerEnter2D(Collider2D other){
		
		if (other.GetComponent<Player> () == null)
			return;
		
		if (HitStarSound != null)
			AudioSource.PlayClipAtPoint (HitStarSound, transform.position);
		

		Instantiate (Effect, transform.position, transform.rotation);
		FloatingText.Show (string.Format ("{0}!",Message ), "MissilePowerActivate", new FromWorldPointTextPositioner (Camera.main, transform.position, 1.5f, 50));
		
		
		gameObject.SetActive (false); //We do not want the stars to completely get destroyed ,instead we are just disabling them.
	}
	
	/*public void FinishAnimationEvent(){
		//gameObject.SetActive (false); //We do not want the stars to completely get destroyed ,instead we are just disabling them.
		Renderer.enabled = false;
		Animator.SetTrigger("Reset");

	}*/
	
	
	public void OnPlayerRespawnInThisCheckPoint(Checkpoint checkpoint,Player player){
		
		
		gameObject.SetActive (true);
	}
	
}
