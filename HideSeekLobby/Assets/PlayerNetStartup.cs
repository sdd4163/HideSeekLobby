using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class PlayerNetStartup : NetworkBehaviour
{

	public Camera FPSCam;
	public AudioListener audioListener;
	public bool isHider;
	
	void Start ()
	{
		/*
		 * We turned off the character controller, firstperson controller, 
		 * the main camera and its audiolistener, because only OUR OWN character should respond to 
		 * keyboard events and control the camer, not all the other clients.
		 * 
		 * So here we turn them all back on IF IT'S US. 
		 * */

		if (isLocalPlayer) {
			GameObject.Find ("Scene Camera").SetActive (false);
			GetComponent<CharacterController> ().enabled = true;
			GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController> ().enabled = true;
			FPSCam.enabled = true;
			audioListener.enabled = true;
		}
	}
	void OnLevelWasLoaded(int level)
	{
		if (level == 1) 
		{
			if (isLocalPlayer) {
				GameObject.Find ("Scene Camera").SetActive (false);
				GetComponent<CharacterController> ().enabled = true;
				GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController> ().enabled = true;
				FPSCam.enabled = true;
				audioListener.enabled = true;
			}
		}
	}


}
