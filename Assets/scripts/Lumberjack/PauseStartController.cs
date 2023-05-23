using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseStartController : MonoBehaviour
{
    public bool state;//true=ゲーム中, false=ポーズ中
    public Image Button;
    public Image pause;
    public Sprite back1;
    public Sprite back2;
    public Sprite start;
    public Sprite stop;
    public GameMaster02 master;
    public ArrowController ArrowCnt;
    public TimeController TimeCnt;
    public AudioSource se;
    Image backimg;

    // Start is called before the first frame update
    void Start()
    {
        state = true;
        backimg = master.back.GetComponent<Image>();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //バグ回避のため、きこりメットのみingameから退避

    public void OnClick()
    {

        if(state)
        {
            pause.enabled = true;
            Button.sprite=start;
            master.NowGameState = (int)GameState.Pause;
            master.select_num = 0;
            master.Pause.SetActive(true);
            master.InGame.SetActive(false);
            master.UIInGame.SetActive(false);
            master.HideSprite();
            backimg.sprite = back2;
            ArrowCnt.stop();
            TimeCnt.stop();
            state = false;
            Debug.Log("stopped");
            se.PlayOneShot(master.pauseAudio);
        }
        else
        {
            pause.enabled = false;
            master.Pause.SetActive(false);
            master.InGame.SetActive(true);
            master.UIInGame.SetActive(true);
            Button.sprite=stop; 
            ArrowCnt.move();
            TimeCnt.move();
            master.NowGameState = (int)GameState.Play;
            master.ShowSprite();
            backimg.sprite = back1;
            state = true;
            Debug.Log("Played");
            se.PlayOneShot(master.cancelAudio);
        }
    }




}
