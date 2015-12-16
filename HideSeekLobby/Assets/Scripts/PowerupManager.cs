using UnityEngine;
using UnityEngine.Networking;
using System.Collections.Generic;

public class PowerupManager : MonoBehaviour {

    static public PowerupManager s_Singleton;

    public GameObject speedBoostPrefab;
    public GameObject speedReducePrefab;
    public GameObject freezePrefab;

    Vector3 spawnPosition1;
    Vector3 spawnPosition2;
    Vector3 spawnPosition3;
    Vector3 spawnPosition4;
    Vector3 spawnPosition5;
    Vector3 spawnPosition6;
    Vector3 spawnPosition7;
    Vector3 spawnPosition8;
    Vector3 spawnPosition9;
    Vector3 spawnPosition10;

    GameObject[] seekers;
    bool seekersPopulated;

    List<GameObject> playerList = new List<GameObject>();
    List<GameObject> powerupList = new List<GameObject>();
    List<GameObject> spawnedPowerups = new List<GameObject>();
    List<Vector3> spawnList = new List<Vector3>();

    Vector3 newPos;
    Vector3 startPos;

    float timer;

    void Awake () {

        s_Singleton = this;

        timer = 0.0f;
        seekersPopulated = false;

        /*spawnPosition1 = new Vector3(Random.Range(119.0f, 130.0f), 6.5f, Random.Range(23.0f, 33.0f));
        spawnPosition2 = new Vector3(Random.Range(119.0f, 130.0f), 6.5f, Random.Range(23.0f, 33.0f));
        spawnPosition3 = new Vector3(Random.Range(119.0f, 130.0f), 6.5f, Random.Range(23.0f, 33.0f));
        spawnPosition4 = new Vector3(Random.Range(119.0f, 130.0f), 6.5f, Random.Range(23.0f, 33.0f));
        spawnPosition5 = new Vector3(Random.Range(119.0f, 130.0f), 6.5f, Random.Range(23.0f, 33.0f));
        spawnPosition6 = new Vector3(Random.Range(119.0f, 130.0f), 6.5f, Random.Range(23.0f, 33.0f));
        spawnPosition7 = new Vector3(Random.Range(119.0f, 130.0f), 6.5f, Random.Range(23.0f, 33.0f));
        spawnPosition8 = new Vector3(Random.Range(119.0f, 130.0f), 6.5f, Random.Range(23.0f, 33.0f));
        spawnPosition9 = new Vector3(Random.Range(119.0f, 130.0f), 6.5f, Random.Range(23.0f, 33.0f));
        spawnPosition10 = new Vector3(Random.Range(119.0f, 130.0f), 6.5f, Random.Range(23.0f, 33.0f));*/

        for(int i=0; i<10; i++)
        {
            spawnList.Add(new Vector3(Random.Range(119.0f, 150.0f), 6.5f, Random.Range(13.0f, 50.0f)));
        }
        /*spawnList.Add(spawnPosition1);
        spawnList.Add(spawnPosition2);
        spawnList.Add(spawnPosition3);
        spawnList.Add(spawnPosition4);
        spawnList.Add(spawnPosition5);
        spawnList.Add(spawnPosition6);
        spawnList.Add(spawnPosition7);
        spawnList.Add(spawnPosition8);
        spawnList.Add(spawnPosition9);
        spawnList.Add(spawnPosition10);*/

    }

	// Use this for initialization
	void Start () {
        startPos = new Vector3(0.0f, -100.0f, 0.0f);

        GameObject speed = (GameObject)Instantiate(speedBoostPrefab, startPos, Quaternion.identity);
        GameObject reduce = (GameObject)Instantiate(speedReducePrefab, startPos, Quaternion.identity);
        GameObject freeze = (GameObject)Instantiate(freezePrefab, startPos, Quaternion.identity);

        powerupList.Add(speed);
        powerupList.Add(reduce);
        powerupList.Add(freeze);

        createLists();
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        //Spawn a new Powerup every x seconds
        if(timer>10.0f && powerupList.Count>0)
        {
            int index = Random.Range(0, powerupList.Count);
            powerupList[index].transform.position = spawnList[index];
            powerupList[index].GetComponent<Powerup>().isSpawned = true;
            NetworkServer.Spawn(powerupList[index]);

            spawnedPowerups.Add(powerupList[index]);
            powerupList.RemoveAt(index);
            spawnList.RemoveAt(index);

            spawnPowerup(speedBoostPrefab);
            timer = 0.0f;
        }

        //Check for collisions between Players and Powerup's if there are spawned powerups and players
        if (spawnedPowerups.Count > 0 && playerList.Count > 0)
        {
            //Go through all the powerups
            for (int i = 0; i < spawnedPowerups.Count; i++)
            {
                GameObject powerup = spawnedPowerups[i];
                Vector3 powerupPos = powerup.transform.position;

                //Go through all the players
                foreach (GameObject player in playerList)
                {
                    Vector3 playerPos = player.transform.position;

                    //Check if the powerup can only be gotten by the hider and if the player is in fact the Hider
                    if (powerup.GetComponent<Powerup>().hiderOnly && player.gameObject.name.Equals("Hider"))
                    {
                        if (checkCollision(playerPos, powerupPos))
                        {
                            Debug.Log("The hider collided with ability: " + powerup.GetComponent<Powerup>().abilityID);
                            if (!seekersPopulated)
                            {
                                seekers = GameObject.FindGameObjectsWithTag("Seeker");
                            }

                            //Give the "ability" to all the seekers
                            foreach (GameObject s in seekers)
                            {
                                s.GetComponent<Player>().GainAbility(powerup.GetComponent<Powerup>().abilityID);

                            }
                            //Despawn the powerup
                            powerup.GetComponent<Powerup>().isSpawned = false;
                            powerup.transform.position = new Vector3(0.0f, -100.0f, 0.0f);
                        }

                    }
                    else if(player.GetComponent<Player>().abilityID != 1)
                    {
                        if (checkCollision(playerPos, powerupPos))
                        {
                            Debug.Log("Current abilityID: " + player.GetComponent<Player>().abilityID);
                            Debug.Log("You collided with ability: " + powerup.GetComponent<Powerup>().abilityID);

                            PlayerGainAbility(powerup.GetComponent<Powerup>().abilityID, player);

                            //Despawn the powerup
                            powerup.GetComponent<Powerup>().isSpawned = false;
                            powerup.transform.position = new Vector3(0.0f, -100.0f, 0.0f);
                        }
                    }

                }
            }
        }
    }

    [Client]
    void PlayerGainAbility(int id, GameObject player)
    {
        player.GetComponent<Player>().GainAbility(id);
    }

    bool checkCollision(Vector3 other, Vector3 speedBoost)
    {

        float dist = Vector3.Distance(other, speedBoost);
        if (dist <= 1.3f)
        {
            return true;
        }
        return false;
    }

    void spawnPowerup(GameObject prefab)
    {
        GameObject temp = (GameObject)Instantiate(prefab, startPos, Quaternion.identity);

        int index = Random.Range(0, spawnList.Count);
        temp.transform.position = spawnList[index];
        temp.GetComponent<Powerup>().isSpawned = true;
        NetworkServer.Spawn(temp);

        spawnedPowerups.Add(temp);
        spawnList.RemoveAt(index);
    }

    void createLists()
    {
        GameObject[] players;

        players = GameObject.FindGameObjectsWithTag("Player");

        foreach (GameObject p in players)
        {
            playerList.Add(p);
        }
    }


}
