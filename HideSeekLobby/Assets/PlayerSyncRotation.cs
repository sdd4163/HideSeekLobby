using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System.Collections.Generic;

public class PlayerSyncRotation: NetworkBehaviour
{
	
	[SyncVar (hook = "OnPlayerRotSynced")]
	private float
		syncPlayerRotation;
	
	[SyncVar (hook = "OnCamRotSynced")]
	private float
		syncCamRotation;
	
	[SerializeField]
	private Transform camTransform;
	
	[SerializeField]
	private float
		lerpRate = 20; 
	
	//variables to only send data when it's changed beyond a threshold.
	private float lastPlayerRot;
	private float lastCamRot;
	private float rotationThreshold = 0.2f;
	
	private List<float> syncPlayerRotList = new List<float> ();
	private List<float> syncCamRotList = new List<float> ();
	private float closeEnough = 0.4f;
	
	[SerializeField]
	private bool useHistoricalInterpolation = true;
	
	void Update ()
	{
		LerpRotations ();
	}
	
	// FixedUpdate will fire at regular intervals, making it a good place
	// To send our regular position updates.
	void FixedUpdate ()
	{
		TransmitRotations ();
	}
	
	void LerpRotations ()
	{
		if (!isLocalPlayer) {
			if (useHistoricalInterpolation) {
				HistoricalInterpolation ();
			} else {
				OrdinaryLerping ();
			}
		}
	}
	
	void HistoricalInterpolation ()
	{
		if (syncPlayerRotList.Count > 0) {
			LerpPlayerRotation (syncPlayerRotList [0]);
			
			if (Mathf.Abs (transform.localEulerAngles.y - syncPlayerRotList [0]) < closeEnough) {
				syncPlayerRotList.RemoveAt (0);
			}
			
		}
		
		if (syncCamRotList.Count > 0) {
			LerpCamRot (syncCamRotList [0]);
			
			if (Mathf.Abs (camTransform.localEulerAngles.x - syncCamRotList [0]) < closeEnough) {
				syncCamRotList.RemoveAt (0);
			}
		}
	}
	
	void OrdinaryLerping ()
	{
		LerpPlayerRotation (syncPlayerRotation);
		LerpCamRot (syncCamRotation);
	}
	
	void LerpPlayerRotation (float rotAngle)
	{
		Vector3 playerNewRot = new Vector3 (0, rotAngle, 0);
		transform.rotation = Quaternion.Lerp (transform.rotation, Quaternion.Euler (playerNewRot), lerpRate * Time.deltaTime);
	}
	
	void LerpCamRot (float rotAngle)
	{
		Vector3 camNewRot = new Vector3 (rotAngle, 0, 0);
		camTransform.localRotation = Quaternion.Lerp (camTransform.localRotation, Quaternion.Euler (camNewRot), lerpRate * Time.deltaTime);
	}
	
	
	[Command]
	void CmdSendRotationsToServer (float playerRot, float camRot)
	{
		//runs on server, we call on client
		syncPlayerRotation = playerRot;
		syncCamRotation = camRot;
	}
	
	
	
	[Client]
	void TransmitRotations ()
	{
		if (isLocalPlayer) {
			if (CheckIfBeyondThreshold (transform.localEulerAngles.y, lastPlayerRot) || 
				CheckIfBeyondThreshold (camTransform.localEulerAngles.x, lastCamRot)) {
				lastPlayerRot = transform.localEulerAngles.y;
				lastCamRot = camTransform.localEulerAngles.x;
				CmdSendRotationsToServer (lastPlayerRot, lastCamRot);
			}
		}
	}
	
	bool CheckIfBeyondThreshold (float rot1, float rot2)
	{
		if (Mathf.Abs (rot1 - rot2) > rotationThreshold) {
			return true;
		} else {
			return false;
		}
	}
	
	
	[Client]
	void OnPlayerRotSynced (float latestPlayerRot)
	{
		syncPlayerRotation = latestPlayerRot;
		syncPlayerRotList.Add (syncPlayerRotation);
	}
	
	[Client]
	void OnCamRotSynced (float latestCamRot)
	{
		syncCamRotation = latestCamRot;
		syncCamRotList.Add (syncCamRotation);
	}
	
	
	
	
	
}
