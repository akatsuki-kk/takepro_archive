using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeEXContoroller : MonoBehaviour
{
    public GameObject master;
    GameMaster gameMaster;
    bool chance = true;
    public bool TreeEnter = false;
    // Start is called before the first frame update
    void Start()
    {
        master = GameObject.Find("Master");
        gameMaster = master.GetComponent<GameMaster>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        chance = gameMaster.chance;
        Debug.Log(chance);

        if (chance == false)
        {
            TreeEnter = false;

        }
        else if (chance == true)
        {
            TreeEnter = true;
        }

    }
}
