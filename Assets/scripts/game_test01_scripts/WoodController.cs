
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WoodController : MonoBehaviour
{
    public GameObject master;
    bool chance = true;
    int Score = 0;
    GameMaster gameMaster;

    // Start is called before the first frame update
    
    void Start()
    {
        gameMaster = master.GetComponent<GameMaster>();
    }

    void Update()
    {
        chance = gameMaster.chance;
        Debug.Log(chance);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (chance == false){
            Debug.Log("reTry");
        }
        else if (chance == true)
        {
            Debug.Log("hit");
            transform.Rotate(new Vector3(0, 0, 30));
            Score += 1000;
            Debug.Log(Score);
            master.GetComponent<GameMaster>().Score = Score;
        }
        
    }
}
