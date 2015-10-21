using UnityEngine;
using System.Collections;

public class Seeker : Player {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		//base.Update ();
		if (Input.GetMouseButtonDown (0)) {
			Debug.Log ("Tag Attempt");
			Tag ();
		}
	}

	void Tag () {
		GameObject hider = GameObject.Find ("Hider");
		float distance = Vector3.Distance (hider.transform.position, gameObject.transform.position);

		if (distance < 2.0f) {
			Debug.Log ("Tagged");
			hider.GetComponent<CharacterController>().enabled = false;
		} else {
			Debug.Log("Not Tagged");
		}
	}
}
