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

	public int abilityID;
	//protected PlayerManager playManager;

	[SyncVar]
	public Color color;

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
        Debug.Log("Ability Countdown Start: " + abilityCountdown);
	}
	
	// Update is called once per frame
	void Update () {
        UseAbility();
        //Debug.Log("Ability Countdown Update: " + abilityCountdown);
    }

	void UseAbility () {
        //Use ability if you have one

        if (abilityID == 1)
        {
            GetComponent<FirstPersonController>().m_WalkSpeed = 8;
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
            Debug.Log("Ability Timer: " + abilityTimer);
            Debug.Log("Ability Countdown: " + abilityCountdown);
            if (abilityTimer >= abilityCountdown)
            {
                Debug.Log("Ran out of ability");
                abilityTimer = 0.0f;
                abilityID = 0;
                GetComponent<CharacterController>().enabled = true;
                GetComponent<FirstPersonController>().m_WalkSpeed = 5;
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
