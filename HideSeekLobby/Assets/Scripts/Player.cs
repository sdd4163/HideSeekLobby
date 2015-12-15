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
	//protected PlayerManager playManager;

	[SyncVar]
	public Color color;

    protected float abilityCountdown;
    protected float abilityTimer;


    void Awake()
    {
        isHider = false;
    }

	// Use this for initialization
	void Start () {
		playerTransform = gameObject.transform;

        abilityCountdown = 15.0f;
        abilityTimer = 0.0f;
        abilityID = 0;
	}
	
	// Update is called once per frame
	void Update () {
        UseAbility();
	}

	void UseAbility () {
        //Use ability if you have one
        if (abilityID == 0)
        {
            //Debug.Log("YOU HAVE NO POWERUPS");
            return;
        }

        if (abilityID == 1)
        {
            //Debug.Log("Speed Boost");
            GetComponent<FirstPersonController>().m_WalkSpeed = 15;
        }
        else if (abilityID == 2)
        {
            //Debug.Log("Ability 2");
            GetComponent<FirstPersonController>().m_WalkSpeed = 4;
        }
        else if (abilityID == 3)
        {
            GetComponent<CharacterController>().enabled = false;
        }
        else if (abilityID == 4)
        {

        }

        //Increment timer
        abilityTimer += Time.deltaTime;
        if (abilityTimer >= abilityCountdown)
        {
            abilityTimer = 0.0f;
            abilityID = 0;
            GetComponent<CharacterController>().enabled = true;
            GetComponent<FirstPersonController>().m_WalkSpeed = 8;
            return;
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
	}
}
