using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;

public class MapCreation : NetworkBehaviour {

    SyncListInt SyncListMapIndex = new SyncListInt();
    List<Vector3> positions = new List<Vector3>();
    const int mazepieces = 11;
    //index codex
    //0 = hider spawn point
    //1 = seeker spawn point
    //2+ = terrain/obsticales
    [SerializeField]
    public GameObject HiderSpawn;
    [SerializeField]
    public GameObject SeekerSpawn;
    [SerializeField]
    public GameObject Terrain1;
    [SerializeField]
    public GameObject Terrain2;
    [SerializeField]
    public GameObject Terrain3;
    [SerializeField]
    public GameObject Terrain4;
    public bool host;
    GameObject[] terrains = new GameObject[6];
    bool first = true;
    bool instantiated = false;
    bool generated = false;
	// Use this for initialization
	void Start () {
        if (first)
        {
            Vector3 a = new Vector3(0, 0, 0);
            for (int i = 0; i < mazepieces; i++)
            {
                GenerateMap();      
            }
          
            
            first = false;
            generated = true;
        }
        if (instantiated == false)
        {
            InstantiateMap();
        }
	}

	// Update is called once per frame
	void Update () {
        
            
	}
    
    void Awake()
    {
        positions.Add(new Vector3(0, 0, 0));
        positions.Add(new Vector3(50, 0, 0));
        positions.Add(new Vector3(100, 0, 0));
        positions.Add(new Vector3(150, 0, 0));
        positions.Add(new Vector3(200, 0, 0));
        positions.Add(new Vector3(0, 0, 50));
        positions.Add(new Vector3(0, 0, 100));
        positions.Add(new Vector3(0, 0, 150));
        positions.Add(new Vector3(0, 0, 200));
        positions.Add(new Vector3(0, 0, 250));
        positions.Add(new Vector3(50, 0, 50));
        
    }
   
    
    void InstantiateMap()
    {
  
        terrains[0] = HiderSpawn;
        terrains[1] = SeekerSpawn;
        terrains[2] = Terrain1;
        terrains[3] = Terrain2;
        terrains[4] = Terrain3;
        terrains[5] = Terrain4;
        int counter=0;
        if (generated)
        {
            for (int k = 0; k < mazepieces; k++)
            {
                Vector3 origin = positions[k];

                for (int i = 0; i < 5; i++)
                {
                    for (int j = 0; j < 5; j++)
                    {
                        Debug.Log(SyncListMapIndex.Count);
                        Instantiate(terrains[SyncListMapIndex[counter]], new Vector3(origin.x + i * 10, origin.y + 0, origin.z + j * 10), new Quaternion(0, 0, 0, 0));
                        counter++;
                    }
                }
            }
        }
        instantiated = true;
    }
    [Server]
    void GenerateMap()
    {
       
       for(int i =0; i<25;i++)
       {
           
           SyncListMapIndex.Add(Random.Range(0, 6));
               
       }
       

    }
    [Server]
    void ReleaseMap()
    {
        SyncListMapIndex.Clear();
    }

}
