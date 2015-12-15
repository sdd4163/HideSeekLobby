using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System.Collections.Generic;

public class Freeze : Powerup {
	void Awake () {
        abilityID = 3;
        hiderOnly = true;
	}
}
