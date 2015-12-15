using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System.Collections.Generic;

public class Powerup : NetworkBehaviour {

    [SyncVar(hook="OrdinaryLerp")]
    private Vector3 syncPos;

    private Vector3 lastPosition;

    private float lerpRate;

    public int abilityID;

    [SyncVar]
    public bool isSpawned;

    public bool hiderOnly;

    // Use this for initialization
    void Start () {
        isSpawned = false;
        lerpRate = 16;
        lastPosition = new Vector3(-22f, -22f, -22f);
    }

    // Update is called once per frame
    void Update()
    {
        //OrdinaryLerp(transform.position);
    }

    void FixedUpdate()
    {
        TransmitPosition();
    }

    public void newPosition()
    {
        transform.position = new Vector3(Random.Range(-9.0f, 10.0f), 1.0f, Random.Range(-9.0f, 10.0f));
    }

    [Client]
    void TransmitPosition()
    {
        if (lastPosition.x != transform.position.x || lastPosition.y != transform.position.y || lastPosition.z != transform.position.z)
        {
            // Send a command to the server to update our position, and 
            // it will update a SyncVar, which then automatically updates on everyone's game instance
            lastPosition = transform.position;
            CmdSendPositionToServer(transform.position);
        }
    }

    [Command]
    void CmdSendPositionToServer(Vector3 pos)
    {
        //runs on server, we call on client
        syncPos = pos;
    }

    void OrdinaryLerp(Vector3 newPos)
    {
        if (isLocalPlayer) return;

        transform.position = newPos;
    }
}