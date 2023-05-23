using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TreeController : MonoBehaviour
{
    
    public GameObject master;
    //bool chance = true;
    public int Score = 0;
    public string Treename;

    GameMaster gameMaster;


    // Start is called before the first frame update

    void Start()
    {
        master = GameObject.Find("Master");
        gameMaster = master.GetComponent<GameMaster>();
    }

    void Update()
    {
        
    }

}
