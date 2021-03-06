﻿using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;
using UnityEngine.Networking;
public class Clock : NetworkBehaviour {
   
    int seconds = 0;
    int timeInMinutes = 0;
    [SerializeField]
    public Text clockText;
	public GameObject panel;
    public bool on = true;
    [SyncVar]
    string clockTime;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (on && isServer)
        {
            float timeInSeconds = 302.0f;
            timeInSeconds -= Time.timeSinceLevelLoad;
            if (timeInSeconds <= 0)
            {
                gameObject.GetComponent<GameStates>().CmdHiderWin();
                return;
            }
            timeInMinutes = Mathf.FloorToInt(timeInSeconds / 60);
            timeInSeconds = Mathf.FloorToInt(timeInSeconds % 60);
           if (timeInSeconds < 10)
            {
                 CmdUpdateText(timeInMinutes.ToString() + ":0" + timeInSeconds, false);
            }
            else if (timeInSeconds == 60)
            {
                timeInMinutes++;
                CmdUpdateText(timeInMinutes.ToString() + ":00", false);
            }
            else
            {
                 CmdUpdateText(timeInMinutes.ToString() + ":" + timeInSeconds, false);
            }
        }
        clockText.text = clockTime;
	}
    [Command]
  	public void CmdUpdateText(string text, bool activate)
    {
        clockText.text = text;
		if (activate){
			ActivatePanel();
		}
        clockTime = text;
    }
	[Client]
	void ActivatePanel(){
		panel.SetActive(true);
	}
}
