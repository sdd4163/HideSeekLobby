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
		//Check all players in scene, see if a hider has been made
		GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
		bool hiderMade = false;
		foreach (GameObject p in players){
			if (p.GetComponent<PlayerNetStartup>().isHider){
				hiderMade = true;
			}
		}

		//Only actual players in clients
		if (isLocalPlayer) {
			GameObject.Find ("Scene Camera").SetActive (false);
			GetComponent<CharacterController> ().enabled = true;
			if (hiderMade){
				//Creates player as a Seeker
				GetComponent<Seeker>().enabled = true;
				Destroy(GetComponent<Hider>());
				gameObject.name = "Seeker";
				gameObject.tag = "Seeker";
				foreach (MeshRenderer mr in gameObject.GetComponentsInChildren<MeshRenderer>()){
					mr.material.color = Color.red;
				}
			}
			else{
				//Creates player as a Hider
				isHider = true;
				GetComponent<Hider>().enabled = true;
				Destroy(GetComponent<Seeker>());
				gameObject.name = "Hider";
				foreach (MeshRenderer mr in gameObject.GetComponentsInChildren<MeshRenderer>()){
					mr.material.color = Color.blue;
				}
			}
			GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController> ().enabled = true;
			FPSCam.enabled = true;
			audioListener.enabled = true;
		}
		//Non-local player objects
		else if (isHider){
			GetComponent<Hider>().enabled = true;
			Destroy(GetComponent<Seeker>());
			gameObject.name = "Hider";
			foreach (MeshRenderer mr in gameObject.GetComponentsInChildren<MeshRenderer>()){
				mr.material.color = Color.blue;
			}
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