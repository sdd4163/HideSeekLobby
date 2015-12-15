using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System.Collections.Generic;

public class SpeedReduce : Powerup {
    void Awake()
    {
        abilityID = 2;
        hiderOnly = true;
    }
}
