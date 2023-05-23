using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KikoriMetController : MonoBehaviour
{
    Rigidbody2D rbody;

    Animator animator; // �A�j���[�^�[
    public string UR = "KikoriUpRight";
    public string NR = "KikoriRight";
    public string DR = "KikoriDownRight";
    public string UL = "KikoriUpLeft";
    public string NL = "KikoriLeft";
    public string DL = "KikoriDownLeft";
    public string Stop = "stop";
    public string pause = "KikorimetPause";
    string nowAnime = "";
    string oldAnime = "";


    public GameMaster02 gameMaster02;
    // Start is called before the first frame update
    void Start()
    {
        rbody = this.GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        nowAnime = Stop;
        oldAnime = Stop;
        List<string> animaterlist = new List<string>();
        //animator.Play("KikoriUpRight");
    }

    // Update is called once per frame

    void Update()
    {
        if (gameMaster02.NowGameState == 1)
        {
            Transform myTransform = this.transform;


            if(gameMaster02.debug){
                Debug.Log(gameMaster02.Course_number);
            }
            if (gameMaster02.Course_number == 5)
            {
                nowAnime = UL;
                myTransform.position = new Vector3(-3.0f, -1, 0);
            }
            else if (gameMaster02.Course_number == 0)
            {
                nowAnime = NL;
                myTransform.position = new Vector3(-3.0f, -2, 0);
            }
            else if (gameMaster02.Course_number == 1)
            {
                nowAnime = DL;
                myTransform.position = new Vector3(-3.0f, -3, 0);
            }
            else if (gameMaster02.Course_number == 2)
            {
                nowAnime = DR;
                myTransform.position = new Vector3(3.0f, -3, 0);
            }
            else if (gameMaster02.Course_number == 3)
            {
                nowAnime = NR;
                myTransform.position = new Vector3(3.0f, -2, 0);
            }
            else if (gameMaster02.Course_number == 4)
            {
                nowAnime = UR;
                myTransform.position = new Vector3(3.0f, -1, 0);
            }
            if(gameMaster02.debug){
                Debug.Log(nowAnime);
            }
            if (nowAnime != oldAnime)
            {
                oldAnime = nowAnime;
                animator.Play(nowAnime);
            }
        }
        else if (gameMaster02.NowGameState == 2)
        {
            nowAnime = pause;
            if (nowAnime != oldAnime)
            {
                oldAnime = nowAnime;
                animator.Play(nowAnime);
            }
        }
        else if (gameMaster02.NowGameState == 4)
        {
            nowAnime = Stop;
            if (nowAnime != oldAnime)
            {
                oldAnime = nowAnime;
                animator.Play(nowAnime);
            }
        }

    }
}
