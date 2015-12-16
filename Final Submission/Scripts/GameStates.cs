using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;
public class GameStates : NetworkBehaviour {
    [SyncVar]
    bool hiderWin = false;
    [SyncVar]
    bool seekerWin = false;
    Text replaceText;

	// Use this for initialization
	void Start () {
        replaceText = gameObject.GetComponent<Clock>().clockText;
	}
	
	// Update is called once per frame
	void Update () {
        
    }
    [Command]
    public void CmdSeekerWin()
    {
        seekerWin = true;
        gameObject.GetComponent<Clock>().on = false;
        gameObject.GetComponent<Clock>().CmdUpdateText("The Seekers Won!", true);
    }
    [Command]
    public void CmdHiderWin()
    { 
        hiderWin = true;
        gameObject.GetComponent<Clock>().on = false;
        gameObject.GetComponent<Clock>().CmdUpdateText("The Hider Won!", true);
    }
    
   
}
