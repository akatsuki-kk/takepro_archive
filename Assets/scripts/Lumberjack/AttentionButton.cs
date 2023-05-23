using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttentionButton : MonoBehaviour
{
    // Start is called before the first frame update
    public GameMaster02 master;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void btn_yes()
    {
        master.NowGameState=(int)GameState.Play;
        master.InGame.SetActive(true);
        master.Attention.SetActive(false);
        master.timeCnt.move();
        master.ArrowCnt.move();
    }

    public void btn_no()
    {
        master.NowGameState=(int)GameState.End;
        master.end_init(2);
        master.InGame.SetActive(false);
        master.Attention.SetActive(false);
        master.pausebtn.enabled = false;
    }
}
