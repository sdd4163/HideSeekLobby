using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityStandardAssets.Characters.FirstPerson;

public class Player : NetworkBehaviour {

    public bool isHider;

	protected int playerID;		//0 is hider, all others are seekers
	public int PlayerID {
		get {
			return playerID;
		}
		set {
			playerID = value;
		}
	}

	protected Transform playerTransform;
	public Transform PlayerTransform {
		get {
			return playerTransform;
		}
	}

	[SyncVar]
	public int abilityID;

    protected float abilityCountdown;
    protected float abilityTimer;


    void Awake()
    {
        isHider = false;

        abilityCountdown = 10.0f;
        abilityTimer = 0.0f;
        abilityID = 0;
    }

	// Use this for initialization
	void Start () {
		playerTransform = gameObject.transform;

        abilityCountdown = 10.0f;
        abilityTimer = 0.0f;
        abilityID = 0;
	}
	
	// Update is called once per frame
	void Update () {
		if (isLocalPlayer){
			UseAbility();
		}
        
        //Debug.Log("Ability Countdown Update: " + abilityCountdown);
    }

	void UseAbility () {
        //Use ability if you have one
		Debug.Log ("Ability");
        if (abilityID == 1)
        {
            gameObject.GetComponent<FirstPersonController>().m_WalkSpeed = 12;
			gameObject.GetComponent<FirstPersonController>().m_RunSpeed = 12;
			Debug.Log ("boost");
        }
        else if (abilityID == 2)
        {
            GetComponent<FirstPersonController>().m_WalkSpeed = 3;
        }
        else if (abilityID == 3)
        {
            GetComponent<CharacterController>().enabled = false;
        }

        if (abilityID != 0)
        {
            //Increment timer
            abilityTimer += Time.deltaTime;
			Debug.Log (abilityTimer);
            if (abilityTimer >= abilityCountdown)
            {
                abilityTimer = 0.0f;
                abilityID = 0;
                GetComponent<CharacterController>().enabled = true;
                GetComponent<FirstPersonController>().m_WalkSpeed = 5;
				GetComponent<FirstPersonController>().m_RunSpeed = 10;
                return;
            }
        }

    }

	public void GainAbility (int itemID) {
		switch (itemID) {
            case 1:
                abilityID = itemID;
                break;
            case 2:
                abilityID = itemID;
                break;
            case 3:
                abilityID = itemID;
                break;
            case 4:
                abilityID = itemID;
                break;
            default:
                //Debug.Log("No ability");
                break;
        }
        Debug.Log("Gained ability: " + abilityID);
    }
}
