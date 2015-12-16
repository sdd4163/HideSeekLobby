using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class PlayerNetStartup : NetworkBehaviour
{

	public Camera FPSCam;
	public AudioListener audioListener;

	[SyncVar]
	public bool isHider = false;
	
	void Start ()
	{
		/*
		 * We turned off the character controller, firstperson controller, 
		 * the main camera and its audiolistener, because only OUR OWN character should respond to 
		 * keyboard events and control the camer, not all the other clients.
		 * 
		 * So here we turn them all back on IF IT'S US. 
		 * */
		GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
		bool hiderMade = false;
		foreach (GameObject p in players){
			if (p.GetComponent<PlayerNetStartup>().isHider){
				hiderMade = true;
			}
		}

		if (isLocalPlayer) {
			GameObject.Find ("Scene Camera").SetActive (false);
			GetComponent<CharacterController> ().enabled = true;
			if (hiderMade){
				GetComponent<Seeker>().enabled = true;
				Destroy(GetComponent<Hider>());
				gameObject.name = "Seeker";
				gameObject.tag = "Seeker";
				foreach (MeshRenderer mr in gameObject.GetComponentsInChildren<MeshRenderer>()){
					mr.material.color = Color.red;
				}
			}
			else{
				isHider = true;
				GetComponent<Hider>().enabled = true;
				Destroy(GetComponent<Seeker>());
				gameObject.name = "Hider";
				foreach (MeshRenderer mr in gameObject.GetComponentsInChildren<MeshRenderer>()){
					mr.material.color = Color.blue;
				}
				//gameObject.tag = "Hider";
			}
			GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController> ().enabled = true;
			FPSCam.enabled = true;
			audioListener.enabled = true;
		}
		else if (isHider){
			GetComponent<Hider>().enabled = true;
			Destroy(GetComponent<Seeker>());
			gameObject.name = "Hider";
			foreach (MeshRenderer mr in gameObject.GetComponentsInChildren<MeshRenderer>()){
				mr.material.color = Color.blue;
			}
			//gameObject.tag = "Hider";
		}
		else{
			GetComponent<Seeker>().enabled = true;
			Destroy(GetComponent<Hider>());
			gameObject.name = "Seeker";
			gameObject.tag = "Seeker";
			foreach (MeshRenderer mr in gameObject.GetComponentsInChildren<MeshRenderer>()){
				mr.material.color = Color.red;
			}
		}
	}
}