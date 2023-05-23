using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ButtonTest : MonoBehaviour
{

    bool chance = true;
    public GameMaster02 gameMaster02;   // Start is called before the first frame update
    public KikoriMetController kikorimetCo;
    void Start()
    {
        gameMaster02.Cut = false;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void downClick()
    {
        chance = gameMaster02.chance;

        if (chance == false)
        {
            Debug.Log("ワンモア");
            gameMaster02.PerfectReward = false;

        }
        else if (chance == true && gameMaster02.Cut == false)
        {
            gameMaster02.nextTree();
            gameMaster02.Cut =true;
            
        }
    }
    public void upClick()
    {
        
    }
}
