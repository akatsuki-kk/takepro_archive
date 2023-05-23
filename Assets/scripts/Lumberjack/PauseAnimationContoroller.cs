using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseAnimationContoroller : MonoBehaviour
{
    Animator animator;
    // Start is called before the first frame update
    int nowGamestate = Animator.StringToHash("nowGamestate");
    public GameMaster02 gameMaster02;
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetInteger(nowGamestate, gameMaster02.NowGameState);
    }

}
