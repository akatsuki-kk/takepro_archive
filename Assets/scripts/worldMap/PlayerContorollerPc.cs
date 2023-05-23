using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerContorollerPc : MonoBehaviour

{
    float inputHorizontal;
    float inputVertical;
    Rigidbody rb;

    public float moveSpeed = 10f;
    public float moveForceMultiplier;

    private Vector3 moveForward;
    private Vector3 moveVector;

    [SerializeField] WorldMapMaster worldMapMaster;


    Animator animator;

    public enum GameState
    {
        Start,
        Play,
        Map,
        Pause,
        Attention,
        Talk,
        End
    }

    

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        WorldMapMaster.NowGameState = WorldMapMaster.GameState.Play;
        // 初期位置を保持

    }

    void Update()
    {
        if(WorldMapMaster.NowGameState == WorldMapMaster.GameState.Play)
        {
            move();
        }
        else if (WorldMapMaster.NowGameState != WorldMapMaster.GameState.Play)
        {
            rb.velocity = Vector3.zero;
            animator.SetBool("Stop", true);
        }
    } 

    void FixedUpdate()
    {
        // カメラの方向から、X-Z平面の単位ベクトルを取得
        Vector3 cameraForward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;

        // 方向キーの入力値とカメラの向きから、移動方向を決定
        moveForward = cameraForward * inputVertical + Camera.main.transform.right * inputHorizontal;

        moveVector = moveForward * moveSpeed;

        rb.AddForce(moveForceMultiplier * (moveVector - rb.velocity));
    }

    void move()
    {
        inputHorizontal = worldMapMaster.CharaXinput;
        inputVertical = worldMapMaster.CharaZinput;

        // deltaTimeが0の場合は何もしない
        if (Mathf.Approximately(Time.deltaTime, 0))
            return;
        // 現在位置取得


        if (rb.velocity == Vector3.zero)
        {
            animator.SetBool("Stop", true);
        }
        else
        {
            animator.SetBool("Stop", false);
        }



        //Debug.Log(rb.velocity);
    }
}
  