using UnityEngine;
using System.Collections;

public class FollowObject : MonoBehaviour {

	public Vector2 Offset;
	public Transform following;

	public void Update(){
		transform.position = following.transform.position + (Vector3)Offset;
	}

}
