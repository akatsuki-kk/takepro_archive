using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public enum GameState
{
    Start,
    Play,
    Pause,
    Attention,
    End,
    Result
}

public class GameMaster02 : MonoBehaviour
{
    public int NowGameState;

    public GameObject Arrow;
    public GameObject Area;

    public bool chance = false;

    public GameObject Hinoki;
    public GameObject Reakeyaki;
    public GameObject Sakura;
    public GameObject Sugi;
    public GameObject Akamatu;
    public GameObject Watch;
    public GameObject SecondHand;
    public Image TimerFade;
    public Text ScoreText;
    public GameObject back;
    public int GameScore;
    int[] CountTree = {0,0,0,0,0};
    int[] ScoreTree = {100,100,150,200,300};
    public int TargetCnt = 10;
    int MaxTarget;
    public float ScoreRate = 0.0f;
    public bool Attentioned = false;
    int localflag;
    int endphase = 0;
    float endtime = 0f;

    public bool PerfectReward = false;


    public GameObject Course_02;
    public int Course_number = -1;
    public int select_num = 0;

    public int WoodCnt = 0;
    public int n;
    public List<int> WoodList = new List<int>();

    //UI関連
    public GameObject bar;
    public GameObject bar_met;
    public GameObject kikori_met;
    public GameObject power_gauge;
    public GameObject time_up;
    public GameObject game_clear;
    public GameObject bassai_button;
    
    public Vector3 bar_met_pos;
    public Image barImg;
    public float bar_met_origin;
    public float bar_met_limit;
    public SpriteRenderer countdwn;
    public SpriteRenderer countdwn_start;
    public Sprite cnt_3;
    public Sprite cnt_2;
    public Sprite cnt_1;
    public Sprite cnt_start;
    public Sprite clear_0;
    public Sprite clear_1;
    Sprite clear;
    public GameObject pauseobj;
    public Image pausebtn;

    //ゲーム進行処理関連
    public Text GameMessage;
    public Image FadeImage;
    public bool isFade;
    public int GameTime = 60;
    int time;

    //画面構成グループ化関連
    public GameObject Pause;
    public GameObject InGame;
    public GameObject UIInGame;
    public GameObject Countdown;
    public GameObject Attention;
    public GameObject End;
    public GameObject Result;
      GameObject Result_group;
    public GameObject Timer_group;
    //おと
    public AudioClip swingAudio;
    public AudioClip selctAudio;
    public AudioClip cancelAudio;
    public AudioClip dramrollAudio;
    public AudioClip pauseAudio;
    public AudioClip teketekeAudio;
    public AudioClip resultAudio;
    public AudioSource se;

    //ジョイコン入力
    public JoyconContoroller JoyconC;

    //デバッグモード
    public bool debug = false;

    int treeP = -1;
    int courseP = -1;

    public bool Cut = false;


    float ArrowYpos;
    float AreaYpos;
    float AreaTop;
    float AreaBotom;

    //public float TimeLimit = 60.0f;
    //public float NowTime;
    public TimeController timeCnt;
    public ArrowController ArrowCnt;
    public JoyconGlobalController JoyconGCnt;
    float SecondHandRot;

    GameObject[] prefabs = new GameObject[3];

    GameObject[] treeNames = new GameObject[5];


    Quaternion[] CoursePos = new Quaternion[]
    {
        Quaternion.Euler(0f, 0f, 0f),
        Quaternion.Euler(0f, 0f, 25f),
        Quaternion.Euler(0f, 0f, 165f),
        Quaternion.Euler(0f, 0f, 180f),
        Quaternion.Euler(0f, 0f, 205f),
        Quaternion.Euler(0f, 0f, 335f),
    };





    Vector3[] treePos = new Vector3[]
    {
        new Vector3(0,-1,0)
    };



    void Start()
    {
        //timeCnt = GetComponent<TimeController>();
        localflag = 0;
        if(debug){
          Debug.Log(timeCnt);
          Debug.Log(Area);
        }
        // pauseobj.enabled = false;
        pausebtn.enabled = false;

        Result_group = Result.transform.Find("resback").gameObject;
        Result_group.GetComponent<Animator>().enabled = false;
        pausebtn = pauseobj.GetComponent<Image>();
        //UIグループ表示初期設定
        InGame.SetActive(true);
        UIInGame.SetActive(true);
        Pause.SetActive(false);
        Attention.SetActive(false);
        Countdown.SetActive(true);
        End.SetActive(false);
        Result.SetActive(false);
        //カウントダウンUI初期設定
        countdwn_start.enabled = false;
        //判定エリアデバッグ用
        if(debug){
          bassai_button.SetActive(true);
          Debug.Log(AreaTop);
          Debug.Log(AreaBotom);
        }
        //スコアの文字を設定
        ScoreText.text = "0";
        //木の名前を設定
        treeNames[0] = Hinoki;
        treeNames[1] = Reakeyaki;
        treeNames[2] = Sakura;
        treeNames[3] = Sugi;
        treeNames[4] = Akamatu;
        //主人公の性別から使用するスプライトを決定
        if(Global.actor_sex == 0){
          clear = clear_0;
        }else{
          clear = clear_1;
        }
        //ノルマゲージ関連初期設定
        bar = GameObject.Find("bar");
        barImg = bar.GetComponent<Image>();
        barImg.fillAmount = 0.0f;
        GameScore = 0;
        bar_met_origin = bar_met.GetComponent<RectTransform>().anchoredPosition.x;
        bar_met_limit = bar_met_origin + bar.GetComponent<RectTransform>().rect.width;
        if(debug){
          Debug.Log("bar:"+bar.GetComponent<RectTransform>().rect.width);
          Debug.Log("origin:"+bar_met_origin);
          Debug.Log("lim:"+bar_met_limit);
        }
        //ゲーム開始準備
        n = 30;//木の生成数
        WoodList=generateTreeList(n);//木の生成
        MaxTarget = n;
        mainGameButtonTest(treeNames, CoursePos, Area);
        FadeController.FadeIn(1.0f);
        NowGameState = (int)GameState.Start;


    }

    // Update is called once per frame
    void Update()
    {
      //Debug.Log("state:"+NowGameState);
        if (NowGameState == (int)GameState.Start)
        {
            start();
        }
        else if (NowGameState == (int)GameState.Play)
        {
            main();
        }
        else if (NowGameState == (int)GameState.End)
        {
            end();
        }
        else if (NowGameState == (int)GameState.Pause)
        {
            pause();
        }
        else if (NowGameState == (int)GameState.Attention)
        {
            attention();
        }

        if(pausebtn.enabled){
          try{
            if(JoyconC.m_joyconR.GetButtonDown(Joycon.Button.PLUS)){
              pauseobj.GetComponent<PauseStartController>().OnClick();
            }
          }catch(System.NullReferenceException e){}
        }

            //timelimit();


    }

    //ゲームスタート時の処理
    void start()
    {
      int starttime;
          if(localflag == 0){
            timeCnt.set(5.0f);
            starttime = (int)timeCnt.displayTime;
            localflag=1;
          }else if(localflag==1){
            starttime = (int)timeCnt.displayTime;
            if(starttime==4){
              countdwn.sprite = cnt_3;
              // countd_text.text = starttime.ToString();
            }else if(starttime==3){
              countdwn.sprite = cnt_2;
            }else if(starttime==2){
              countdwn.sprite = cnt_1;
            }else if(starttime==1){
              countdwn.enabled = false;
              countdwn_start.enabled = true;
            }
            if(starttime <= 0)
            {
              countdwn_start.enabled = false;
              Countdown.SetActive(false);
              localflag=2;
            }
          }else if(localflag==2){
            timeCnt.set(GameTime);
            pausebtn.enabled = true;
            NowGameState = (int)GameState.Play;
          }
    }

    //木こりゲーム実行中の処理
    void main()
    {
      ArrowCnt.move();
      ArrowYpos = Arrow.transform.position.y;
      if (ArrowYpos <= AreaTop && ArrowYpos >= AreaBotom)
      {
          chance = true;
      }
      else
      {
          chance = false;
          Cut = false;
      }
      if (timeCnt != null)
      {
          if (timeCnt.gameTime > 0.0f)
          {
              // 整数に代入することで小数を切り捨てる
              time = (int)timeCnt.displayTime;

              // タイム更新
              SecondHandRot = -6 * (timeCnt.gameTime - time);
              TimerFade.fillAmount = (timeCnt.gameTime - time) / timeCnt.gameTime;
              //Debug.Log(SecondHandRot);
              //Debug.Log(time);
              SecondHand.transform.rotation = Quaternion.Euler(0, 0, SecondHandRot);
              // タイムオーバー
              if(debug){
                Debug.Log("time:"+time);
              }
              if (time <= 0)
              {
                  timeCnt.stop();
                  ArrowCnt.stop();
                  pausebtn.enabled=false;
                  end_init(1);
                  NowGameState = (int)GameState.End;
              }
          }
      }
      //ノルマ達成確認
      if (ScoreRate == 1)
      {
        if(Attentioned == false){
          select_num = 0;
          NowGameState = (int)GameState.Attention;
          timeCnt.stop();
          ArrowCnt.stop();
          InGame.SetActive(false);
          Attention.SetActive(true);

        }
      }
    }

    void pause()
    {
        try{
          if(JoyconC.m_joyconL.GetButtonDown(Joycon.Button.DPAD_UP)) select_num -=1;
          if(JoyconC.m_joyconL.GetButtonDown(Joycon.Button.DPAD_DOWN)) select_num += 1;
          if(select_num <0)select_num = 0;
          if(select_num >2)select_num = 2;
          for(int i=0;i<3;i++){
            if(i == select_num){
              Pause.transform.GetChild(i).gameObject.GetComponent<PauseMenuController>().Mouse_hovered();
            }else{
              Pause.transform.GetChild(i).gameObject.GetComponent<PauseMenuController>().Mouse_exited();
            }
          }
          if(JoyconC.m_joyconR.GetButtonDown(Joycon.Button.DPAD_RIGHT)){
            switch(select_num){
              case 0:
                        se.PlayOneShot(cancelAudio); 
                Pause.transform.GetChild(select_num).gameObject.GetComponent<PauseMenuController>().Back_Game();
              break;
              case 1:
                        se.PlayOneShot(selctAudio);
                Pause.transform.GetChild(select_num).gameObject.GetComponent<PauseMenuController>().Restart();
              break;
              case 2:
                        se.PlayOneShot(selctAudio);
                        Pause.transform.GetChild(select_num).gameObject.GetComponent<PauseMenuController>().Back_Menu();
              break;
            }
          }
        }catch(System.NullReferenceException e){}
    }

    void attention()
    {
      Attentioned = true;
      try{
        if(JoyconC.m_joyconL.GetButtonDown(Joycon.Button.DPAD_LEFT)) select_num -= 1;
        if(JoyconC.m_joyconL.GetButtonDown(Joycon.Button.DPAD_RIGHT)) select_num += 1;
        if(select_num <0) select_num = 0;
        if(select_num >1) select_num = 1;
        if(select_num == 0){
          Attention.transform.Find("att_yes_Y").gameObject.GetComponent<Image>().color = new Color32(255,255,255,255);
          Attention.transform.Find("att_yes_N").gameObject.GetComponent<Image>().color = new Color32(125,125,125,255);
        }else{
          Attention.transform.Find("att_yes_Y").gameObject.GetComponent<Image>().color = new Color32(125,125,125,255);
          Attention.transform.Find("att_yes_N").gameObject.GetComponent<Image>().color = new Color32(255,255,255,255);
        }

        if(JoyconC.m_joyconR.GetButton(Joycon.Button.DPAD_RIGHT)){
          if(select_num ==0){
            Attention.transform.Find("att_yes_Y").gameObject.GetComponent<AttentionButton>().btn_yes();
          }else{
            Attention.transform.Find("att_yes_N").gameObject.GetComponent<AttentionButton>().btn_no();
          }
        }
      }catch(System.NullReferenceException e){}
    }

    public void end_init(int type)
    {
      InGame.SetActive(false);
      End.SetActive(true);
      if(type == 1)
      {
        game_clear.SetActive(false);
        time_up.SetActive(true);
      }else{
        game_clear.SetActive(true);
        time_up.SetActive(false);
      }
    }

    void end()
    {
      switch(endphase){
        case 0:
          try{
            if(JoyconC.m_joyconR.GetButtonDown(Joycon.Button.DPAD_RIGHT)) {
              endphase = 1;
              //結果表示処理
              se.PlayOneShot(dramrollAudio);
              End.SetActive(false);
              Result.SetActive(true);
              int total = 0;
              for(int i=0; i<6; i++){
                GameObject j = Result_group.transform.GetChild(i).gameObject;
                int n,m;
                if(i<5){
                  n = CountTree[i];
                  m = CountTree[i] * ScoreTree[i];
                  total += m;
                }else{
                  m = GameScore - total;
                  n = m / 20;
                }
                j.transform.Find("Count").gameObject.GetComponent<Text>().text = n.ToString();
                j.transform.Find("Score").gameObject.GetComponent<Text>().text = m.ToString();
              }
              Result_group.transform.Find("totalscore").gameObject.GetComponent<Text>().text = GameScore.ToString();
              Animator resanim = Result_group.GetComponent<Animator>();
              resanim.enabled = true;
              resanim.SetBool("resultShowing",true);
              if(CountTree[4] > 0) resanim.SetBool("isContainRare",true);
            }
          }catch(System.NullReferenceException e){}
        break;
        case 1:
          endtime += Time.deltaTime;
          if(endtime > 4f) endphase = 2;
        break;
        case 2:
          try{
            if(JoyconC.m_joyconR.GetButtonDown(Joycon.Button.DPAD_RIGHT)) {
                        //終了処理
                        GameObject TimeUp = End.transform.Find("TimeUp").gameObject;
                        TimeUp.SetActive(false);
                        if (ScoreRate < 1)
                        {
                            Global.world_diff = 1;
                            FadeController.SceneFadeOut(1, "maptutorial");
                        }
                        else if (WoodCnt < n)
                        {
                            Global.world_diff = 1;
                            FadeController.SceneFadeOut(1, "maptutorial");
                        }
                        else
                        {
                            Global.world_diff = 2;
                            FadeController.SceneFadeOut(1, "maptutorial");
                        }
                    }
          }catch(System.NullReferenceException e){}
        break;
      }
    }



    void summonTree(GameObject treeName, Vector3 place, Quaternion rotation)
    {

        prefabs[0] =  Instantiate(treeName, place, Quaternion.identity) as GameObject;
        prefabs[0].transform.parent = InGame.transform;
        prefabs[0].GetComponent<SpriteRenderer>().sortingOrder = 3;
        if (treeName == treeNames[0])
        {
            prefabs[0].GetComponent<TreeController>().Treename = "Hinoki";
            prefabs[0].GetComponent<TreeController>().Score = 200;
        }
        else if (treeName == treeNames[1])
        {
            prefabs[0].GetComponent<TreeController>().Treename = "Reakeyaki";
            prefabs[0].GetComponent<TreeController>().Score = 300;
        }
        else if (treeName == treeNames[2])
        {
            prefabs[0].GetComponent<TreeController>().Treename = "Sakura";
            prefabs[0].GetComponent<TreeController>().Score = 100;
        }
        else if (treeName == treeNames[3])
        {
            prefabs[0].GetComponent<TreeController>().Treename = "Sugi";
            prefabs[0].GetComponent<TreeController>().Score = 100;
        }
        else if (treeName == treeNames[4])
        {
            prefabs[0].GetComponent<TreeController>().Treename = "Akamatu";
            prefabs[0].GetComponent<TreeController>().Score = 150;
        }
    }

    void summonCourse(Vector3 place, Quaternion rotation)
    {
        prefabs[1] = Instantiate(Course_02, place, rotation) as GameObject;
        prefabs[1].transform.parent = InGame.transform;
        prefabs[1].GetComponent<SpriteRenderer>().sortingOrder = 4;
    }

    void summonArea(GameObject area)
    {
        float y;
        y = UnityEngine.Random.Range(-3.26f, 1.78f);
        prefabs[2] = Instantiate(area, new Vector3(-6.6f, y, 0),Quaternion.identity);
        prefabs[2].transform.parent = InGame.transform;
        prefabs[2].GetComponent<SpriteRenderer>().sortingOrder = 3;
    }

    void mainGameButtonTest(GameObject[] treeName, Quaternion[] CoursePos,GameObject area)
    {
        try{
          treeP=WoodList[WoodCnt];
          courseP = UnityEngine.Random.Range(0, 6);
          Course_number = courseP;
          summonTree(treeName[treeP],  treePos[0], Quaternion.identity);
          summonCourse(new Vector3(0f, -2.5f, 0f), CoursePos[courseP]);
          summonArea(area);
          AreaTop = prefabs[2].transform.position.y + 1;
          AreaBotom = prefabs[2].transform.position.y - 1;
        }catch(ArgumentOutOfRangeException e){
          NowGameState = (int)GameState.End;
          end_init(2);
          pausebtn.enabled = false;
          timeCnt.stop();
          ArrowCnt.stop();
        }
        

        
    }

    List<int> generateTreeList(int n)
    {
      var resarray = new List<int>();
      var reakeyaki_index = UnityEngine.Random.Range(20,27);
        for(int i=0; i < n; i++){
          var r = UnityEngine.Random.Range(0.0f, 1.0f);
          if(i<7){
            if(r < 0.5){
              resarray.Add(2);
            }else{
              resarray.Add(3);
            }
          }else{
            if(r < 0.25){
              resarray.Add(2);
            }else if(r < 0.5){
              resarray.Add(3);
            }else if(r < 0.75){
              resarray.Add(0);
            }else{
              resarray.Add(4);
            }
          }
        }
        if(n >=20){
          resarray[reakeyaki_index-1]=1;
        }
      return resarray;
    }

    public void nextTree()
    {

        if(debug){
          Debug.Log(GameScore);
        }
        GameScore += prefabs[0].GetComponent<TreeController>().Score;
        int j = 0;
        switch(prefabs[0].GetComponent<TreeController>().Treename){
          case "Sakura":
            j=0;
          break;
          case "Sugi":
            j=1;
          break;
          case "Akamatu":
            j=2;
          break;
          case "Hinoki":
            j=3;
          break;
          case "Reakeyaki":
            j=4;
          break;
        }
        CountTree[j] += 1;
        if (PerfectReward == true){
            GameScore += 20;
        }
        PerfectReward = true;
        Cut = false;
        foreach(GameObject i in prefabs)
        {
            Destroy(i);
        }

        // Debug.Log(GameScore);
        WoodCnt += 1;
        // Debug.Log(WoodCnt);
        //ノルマゲージ
        ScoreRate = (float)WoodCnt / (float)TargetCnt;
        if (ScoreRate>=1){
          ScoreRate=1;
        }
        if(debug){
          Debug.Log("Score:"+ScoreRate);
        }
        barImg.fillAmount = ScoreRate;
        //ノルマゲージの上にいるやつ
        bar_met_pos=bar_met.GetComponent<RectTransform>().anchoredPosition;
        bar_met_pos.x=bar_met_origin+(float)(bar_met_limit-bar_met_origin)*ScoreRate;
        bar_met.GetComponent<RectTransform>().anchoredPosition = bar_met_pos;
        Destroy(prefabs[0]);
        Destroy(prefabs[1]);
        mainGameButtonTest(treeNames, CoursePos, Area);
        ScoreText.text =GameScore.ToString();
    }

//    void timelimit()
//    {
//        NowTime = TimeLimit - Time.time;
//        // Debug.Log(NowTime);
//        SecondHandRot = -6 * (TimeLimit - NowTime);
        // Debug.Log(SecondHandRot);
//        SecondHand.transform.rotation = Quaternion.Euler(0, 0, SecondHandRot);
//    }

    void FillScreen()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();

        // カメラの外枠のスケールをワールド座標系で取得
        float worldScreenHeight=Camera.main.orthographicSize*2f;
        float worldScreenWidth=worldScreenHeight/Screen.height*Screen.width;

        // スプライトのスケールもワールド座標系で取得
        float width  = sr.sprite.bounds.size.x;
        float height = sr.sprite.bounds.size.y;

        //  両者の比率を出してスプライトのローカル座標系に反映
        transform.localScale = new Vector3 (worldScreenWidth / width, worldScreenHeight / height);

        // カメラの中心とスプライトの中心を合わせる
        Vector3 camPos = Camera.main.transform.position;
        camPos.z = 0;
        transform.position = camPos;
    }

    public void ShowSprite()
    {
      foreach (GameObject g in prefabs){
        SpriteRenderer s = g.GetComponent<SpriteRenderer>();
        s.enabled = true;
      }
      Watch.GetComponent<Image>().enabled = true;
      power_gauge.GetComponent<SpriteRenderer>().enabled = true;
      Arrow.GetComponent<SpriteRenderer>().enabled = true;
      kikori_met.GetComponent<SpriteRenderer>().enabled = true;
      Timer_group.SetActive(true);
    }

    public void HideSprite()
    {
      foreach (GameObject g in prefabs){
        SpriteRenderer s = g.GetComponent<SpriteRenderer>();
        s.enabled = false;
      }
      Watch.GetComponent<Image>().enabled = false;
      power_gauge.GetComponent<SpriteRenderer>().enabled = false;
      Arrow.GetComponent<SpriteRenderer>().enabled = false;
      kikori_met.GetComponent<SpriteRenderer>().enabled = false;
      Timer_group.SetActive(false);
    }

}
