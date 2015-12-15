using UnityEngine;
using System.Collections;
using System.Reflection;
using UnityEngine.Networking;

public class Debugger : NetworkBehaviour
{

	public bool showDebugging = true;
	
	void OnGUI ()
	{
		if (showDebugging) {
			NetworkIdentity identity = GetComponent<NetworkIdentity> () as NetworkIdentity;
			
			Vector3 pos = Camera.main.WorldToScreenPoint (transform.position);
			int posY = 100;
			int xDiff = 20;
			int yDiff = 12;
			int xOffset = -100;
			if (identity.isLocalPlayer) {
				xOffset = 100;
			}

			if (identity.isLocalPlayer) {
				GUI.Label (new Rect (pos.x + xOffset, Screen.height - pos.y + posY, 200, 20), "IsLocalPlayer");
				posY += yDiff;
			}

			if (identity.isServer) {
				GUI.Label (new Rect (pos.x + xOffset, Screen.height - pos.y + posY, 200, 20), "IsServer");
				posY += yDiff;
			}
			
			if (GetComponent<NetworkIdentity> ().isClient) {
				GUI.Label (new Rect (pos.x + xOffset, Screen.height - pos.y + posY, 200, 20), "IsClient");
				posY += yDiff;
			}
			
			GUI.Label (new Rect (pos.x + xOffset, Screen.height - pos.y + posY, 100, 20), "netId = " + identity.netId);	
			posY += yDiff;

			GUI.Label (new Rect (pos.x + xOffset, Screen.height - pos.y + posY, 200, 20), "pos: (" + transform.position.x + ", " + transform.position.z + ")");
			posY += yDiff;
			

			//custom variables
			//if you have a PlayerID:
			//GUI.Label (new Rect (pos.x + xOffset, Screen.height - pos.y + posY, 200, 20), "PlayerID = " + GetComponent<Player> ().PlayerID);
			posY += yDiff;
			

			NetworkBehaviour[] netBehavs = GetComponents<NetworkBehaviour> ();
			
			if (netBehavs.Length > 0) {
				
				
				if (!showDebugging) {
					if (GUI.Button (new Rect (xOffset + xDiff, Screen.height - pos.y + posY, 80, 18), "behaviours")) {
						showDebugging = true;
					}
				} else {
					
					
					foreach (NetworkBehaviour beh in netBehavs) {
						GUI.Label (new Rect (pos.x + xOffset + xDiff, Screen.height - pos.y + posY, 200, 20), "behav: " + beh.GetType ().Name);
						posY += yDiff;
						
						//Debug.Log ("  GetFields()).GetLength " + ((beh.GetType ().GetFields ()).GetLength (0)));

						foreach (FieldInfo field in beh.GetType ().GetFields()) {
							System.Attribute[] markers = (System.Attribute[])field.GetCustomAttributes (typeof(SyncVarAttribute), true);

							if (markers.Length > 0) {
								GUI.Label (new Rect (pos.x + xOffset + xDiff, Screen.height - pos.y + posY, 300, 20), "- " + field.Name + "=" + field.GetValue (beh));
								posY += yDiff;
							}
						
						}
					}

				}
			}
		}
		
	}
}
