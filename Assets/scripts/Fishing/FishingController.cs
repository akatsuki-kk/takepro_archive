using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;
using UnityEngine.UI;

public enum FishingGameStates{
    Start,
    Play_pos,
    Play_throwing,
    Play_hit_judge,
    Play_catch_hooking_rotate,
    Play_catch_hooking_pos,
    Play_tactics,
    Play_catch_pull,
    Play_catch_net,
    Check_Fishkind,
    Pause,
    Select_continue,
    End,
    End_score
}
//シーン遷移
//start                   ->                    play_pos->play_throwing->play_hit_judge->
//play_catch_->play_catch_hooked->       ^
//play_tactics->play_catch_pull->play_catch_net ┘ pause end->end_score

public class FishingController : MonoBehaviour
{
    public int norma_count;
    public int initial_fishcount;
    
    public GameObject[] boxList = new GameObject[2];
    private List<Fish> Fishes = new List<Fish>();
    public GameObject metcharactor;
    public GameObject spritecanvas;
    public GameObject countdown_group;
        Image countdown_obj;//カウントダウンの数字を表示するオブジェクト
        Image countdown_st_obj;//カウントダウン「スタート」のオブジェクト
        public Sprite cnt_1;//カウントダウン「１」の画像
        public Sprite cnt_2;//カウントダウン「２」の画像
        public Sprite cnt_3;//カウントダウン「３」の画像
    public GameObject pos_desition_group;//投擲位置指定関連の親オブジェクト
        GameObject pos_rail_obj;//投擲位置指定の可動部のオブジェクト
        GameObject pos_arrow_obj;//投擲位置のオブジェクト
        GameObject pos_line_obj;//投擲位置表示線のオブジェクト
        float arrowpos = 0.0f;//釣り針投擲位置(ローカルy座標)
        float arrowspeed = 3.0f;//釣り針投擲位置指定速度
        float[] arrowrange = {10f,320.0f};//釣り針投擲位置範囲
        float arrowrot = 0.0f;//釣り針投擲角度
        float catch_dist;//魚と釣り針の捕獲距離
        bool arrowLeft = true;//針が上に動いている間true
        bool throwing = false;//針を投げている間ture
        int hit = -1;//魚がかかったらその魚のfishesでの位置を格納
        bool hooking = false;//針に引き寄せている間true
        bool hooked = false;//針にかかったらtrue
        bool catching = false;//釣りあげている間true
    //釣り上げ関係
    public GameObject fishingrod_group;
        Animator ryoushiAnimator;
        GameObject rod_obj;
        public GameObject string_obj;
        public GameObject hook_obj;
        public GameObject catch_net;
        float hook_homepos_x = 185.2f;
        float hook_homepos_y = -4.7f;
        float hook_middlepos_y = 90.0f;
        float string_scale;
        float string_target_scale;
        float string_target_pos;
        float string_edge_y;
        float fish_target_dec;
        float fish_box_dec;
        Vector3 relFishPos;
        Quaternion FishRot;
        Quaternion pullingFishRot;
        float rotFishSpeed = 45f;
        bool poschangeFlag = false;
        Vector2 posChangeSpeed;
        Vector2 boxdeltaPos;
        bool endflag=false;
        int phase;
        float Animtime = 0f;
        float tactics_time;
        float tactics_time_limit = 5f;
        int tactics_count;
        int tactics_count_objective = 20;
        float net_waittime;
        string throw_anim = "Throw";
        string catch_anim = "Catch";
    public GameObject NormaGauge_group;
        Image NormaGauge;
    public GameObject RendaButton;
    public GameObject CheckFish_group;
        GameObject checkfish_fish;
        Text gain_point;
        Text fish_name;
    public Image pausebtn;
    public TimeController timeCnt;
    public GameObject Timer_group;
    public GameObject PauseGroup;
    public Sprite[] pausebtn_inactive = new Sprite[3];
    public Sprite[] pausebtn_active = new Sprite[3];
    GameObject PauseButtonGroup;
    public FishPauseButtonController FPBC;
    public FishingPauseMenuController FPMC;
    public GameObject RumbleC;
    Dictionary<string, bool> before_enabled = new Dictionary<string, bool>();
    public int select_num;
    public GameObject SecondHand;
    public Image TimerCircle;
    public Sprite[] messages = new Sprite[2];
    public GameObject Message;
    public GameObject SelectContinueGroup;
        GameObject sc_yes;
        GameObject sc_no;
    public GameObject EndResultGroup;
    public AudioClip[] SoundEffects = new AudioClip[20];
    public AudioSource SESource;
    public GameObject back;
    public Sprite[] backs = new Sprite[2];

    //魚生成関連
    public Sprite[] Fishkind_sprites = new Sprite[10];
    //Fishkind(string 名前, Sprite 魚のスプライト, int 得点, float 出現率, bool つり上げアクション有無)
    List<Fishkind> Fishkinds = new List<Fishkind>();//魚種類ごとデータ格納
    Vector2[] SpawnFishRange = {new Vector2(-300.0f,-90f), new Vector2(300f,150f)};
    float generateTime=0f;//魚生成用タイマー
    float generateFreq=3.5f;//魚生成頻度
    bool generated = false;
    

    public int GameTime = 180;
    int GameScore=0;
    int FishCount=0;
    int time;
    public int NowGameState;
    int localflag;

    //ゲーム終了処理関連
    public GameObject EndGroup;
    public Sprite Time_up;
    public Sprite[] GameClear = new Sprite[3];
        GameObject GameClearobj;

    private static readonly Joycon.Button[] m_buttons = 
        Enum.GetValues(typeof(Joycon.Button)) as Joycon.Button[];
    private List<Joycon> m_joycons;
    public Joycon m_joyconL;
    public Joycon m_joyconR;
    private Joycon.Button? m_pressedButtonL;
    private Joycon.Button? m_pressedButtonR;
    Quaternion orientation;
    Vector3 accel;
    public bool debug;
    

    // Start is called before the first frame update
    void Start()
    {
        NowGameState=(int)FishingGameStates.Start;
        countdown_obj = countdown_group.transform.Find("CountDown").gameObject.GetComponent<Image>();
        countdown_st_obj = countdown_group.transform.Find("CountDown_start").gameObject.GetComponent<Image>();
        pos_rail_obj = pos_desition_group.transform.Find("Pos_rail").gameObject;
        pos_arrow_obj = pos_rail_obj.transform.Find("to_throw").gameObject;
        pos_line_obj = pos_rail_obj.transform.Find("line").gameObject;
        NormaGauge = NormaGauge_group.transform.Find("NormaGauge").gameObject.GetComponent<Image>();
        checkfish_fish = CheckFish_group.transform.Find("fish").gameObject;
        fish_name = CheckFish_group.transform.Find("name").gameObject.GetComponent<Text>();
        gain_point = CheckFish_group.transform.Find("score").gameObject.GetComponent<Text>();
        PauseButtonGroup = PauseGroup.transform.Find("buttons").gameObject;
        sc_yes = SelectContinueGroup.transform.Find("Yes").gameObject;
        sc_no = SelectContinueGroup.transform.Find("No").gameObject;
        GameClearobj = EndGroup.transform.Find("gameclear").gameObject;
        pausebtn.enabled = false;

        ryoushiAnimator = metcharactor.GetComponent<Animator>();

        //魚種類初期設定
        Fishkind[] Fishkinddatas = new Fishkind[]{
            new Fishkind("スシナマズ（タマゴ）",Fishkind_sprites[0],200,0.35f),
            new Fishkind("スシナマズ（カリフォルニア）",Fishkind_sprites[1],200,0.35f),
            new Fishkind("スシナマズ（マグロ）",Fishkind_sprites[2],300,0.2f,true),
            new Fishkind("スシナマズ（ナガイアナゴ）",Fishkind_sprites[3],500,0.1f,true),
            new Fishkind("スーシールーパー（グンカン）",Fishkind_sprites[4],800,0.0f,true)
        };
        Fishkinds.AddRange(Fishkinddatas);

        //分岐による難易度調整
        if(Global.fishing_diff == 0){
            initial_fishcount = 4;
            generateFreq = 3.5f;
            back.GetComponent<SpriteRenderer>().sprite = backs[0];
        }else{
            initial_fishcount = 2;
            generateFreq = 6f;
            back.GetComponent<SpriteRenderer>().sprite = backs[1];
        }



        countdown_group.SetActive(true);
        countdown_st_obj.enabled = false;
        pos_rail_obj.SetActive(false);
        CheckFish_group.SetActive(false);
        PauseGroup.SetActive(false);
        RendaButton.SetActive(false);
        SelectContinueGroup.SetActive(false);
        EndResultGroup.SetActive(true);
        EndResultGroup.GetComponent<Animator>().enabled = false;
        EndResultGroup.SetActive(false);
        
        localflag = 0;
        string_target_scale = 400f;

        NormaGauge.fillAmount = 0f;
        joycon_setup();
        if(initial_fishcount>0){
            for(int i=0; i<initial_fishcount; i++){
                generateFish();
            }
        }
        // generateAnim(-300.0f,-100.0f);
        // fishingrod_throw();
    }

    // Update is called once per frame
    void Update()
    {
        //常時実行処理
        joycon_update();
        pause_button_check();
        if(NowGameState == (int)FishingGameStates.Start){
            start();
        }else if(NowGameState == (int)FishingGameStates.Play_pos){
            pausebtn.enabled = true;
            play_pos();
            timerupdate();
            generate_fish_ingame();
        }else if(NowGameState == (int)FishingGameStates.Play_throwing){
            timerupdate();
            play_throwing();
        }else if(NowGameState == (int)FishingGameStates.Play_hit_judge){
            timerupdate();
            play_hit_judge();
        }else if(NowGameState == (int)FishingGameStates.Play_catch_hooking_rotate){
            timerupdate();
            play_catch_hooking_rotate();
        }else if(NowGameState == (int)FishingGameStates.Play_catch_hooking_pos){
            timerupdate();
            play_catch_hooking_pos();
        }else if(NowGameState == (int)FishingGameStates.Play_tactics){
            timerupdate();
            play_tactics();
        }else if(NowGameState == (int)FishingGameStates.Play_catch_pull){
            timerupdate();
            play_catch_pull();
        }else if(NowGameState == (int)FishingGameStates.Play_catch_net){
            timerupdate();
            play_catch_net();
        }else if(NowGameState == (int)FishingGameStates.Check_Fishkind){
            pausebtn.enabled = false;
            check_fishkind();
        }else if(NowGameState == (int)FishingGameStates.Pause){
            pause();
        }else if(NowGameState == (int)FishingGameStates.Select_continue){
            pausebtn.enabled = false;
            select_continue();
        }else if(NowGameState == (int)FishingGameStates.End){
            pausebtn.enabled = false;
            end();
        }
    }

    void start(){
        int starttime;
        if(localflag == 0){
            timeCnt.set(5.0f);
            starttime = (int)timeCnt.displayTime;
            localflag = 1;
        }else if(localflag == 1){
            starttime = (int)timeCnt.displayTime;
            if(starttime==4){
              countdown_obj.sprite = cnt_3;
              // countd_text.text = starttime.ToString();
            }else if(starttime==3){
              countdown_obj.sprite = cnt_2;
            }else if(starttime==2){
              countdown_obj.sprite = cnt_1;
            }else if(starttime==1){
              countdown_obj.enabled = false;
              countdown_st_obj.enabled = true;
            }
            if(starttime <= 0)
            {
              countdown_st_obj.enabled = false;
              countdown_group.SetActive(false);
              localflag=2;
            }
        }else if(localflag==2){
            timeCnt.set(GameTime);
            pausebtn.enabled = true;
            pos_rail_obj.SetActive(true);
            hook_obj.SetActive(false);
            string_obj.SetActive(false);
            pos_line_obj.SetActive(true);
            catch_net.GetComponent<SpriteRenderer>().color = new Color32(255,255,255,0);
            phase = 0;
            Animtime = 0f;
            NowGameState = (int)FishingGameStates.Play_pos;
        }
    }

    void generate_fish_ingame(){
        generateTime += Time.deltaTime;
        float ftime=((float)Math.Floor(generateTime*10)/10);
        if(ftime >= generateFreq){
            if(generated){}else{
                generated=true;
                generateTime=0f;
                if(Fishes.Count < 8){
                    Debug.Log("time!");
                    generateFish();
                }
            }
        }else{
            generated=false;
        }
    }

    void play_pos(){
        switch(phase){
            case 0:
                if(arrowLeft){
                    arrowpos -= arrowspeed;
                    if (arrowpos <= arrowrange[0]){
                        arrowpos = arrowrange[0];
                        arrowLeft = false;
                    } 
                }else{
                    arrowpos += arrowspeed;
                    if(arrowpos >= arrowrange[1]){
                        arrowpos = arrowrange[1];
                        arrowLeft = true;
                    }
                }
                try{
                    if(m_joyconL.GetStick()[0]>=0.5f || Input.GetKey(KeyCode.RightArrow)){
                        arrowrot -= 2.0f;
                        Debug.Log("Right");
                        if(arrowrot >=-74.0f){
                            pos_rail_obj.transform.Rotate(0.0f,0.0f,-2.0f);
                            pos_arrow_obj.transform.Rotate(0.0f,0.0f,2.0f);
                        }else{
                            arrowrot = -74.0f;
                        }
                    }
                    if(m_joyconL.GetStick()[0]<=-0.5f || Input.GetKey(KeyCode.LeftArrow)){
                        arrowrot += 2.0f;
                        if(arrowrot <= 74.0f){
                            pos_rail_obj.transform.Rotate(0.0f,0.0f,2.0f);
                            pos_arrow_obj.transform.Rotate(0.0f,0.0f,-2.0f);
                        }else{
                            arrowrot = 74.0f;
                        }
                        Debug.Log("Left");
                    }
                }catch(System.NullReferenceException e){
                    if(Input.GetKey(KeyCode.RightArrow)){
                        arrowrot -= 2.0f;
                        if(arrowrot >=-74.0f){
                            pos_rail_obj.transform.Rotate(0.0f,0.0f,-2.0f);
                            pos_arrow_obj.transform.Rotate(0.0f,0.0f,2.0f);
                        }else{
                            arrowrot = -74.0f;
                        }
                        Debug.Log("Right");
                    }
                    if(Input.GetKey(KeyCode.LeftArrow)){
                        arrowrot += 2.0f;
                        if(arrowrot <= 74.0f){
                            pos_rail_obj.transform.Rotate(0.0f,0.0f,2.0f);
                            pos_arrow_obj.transform.Rotate(0.0f,0.0f,-2.0f);
                        }else{
                            arrowrot = 74.0f;
                        }
                        
                        Debug.Log("Left");
                    }
                }
                if(arrowrot > 0){
                    ryoushiAnimator.SetBool("isRight", false);
                }else{
                    ryoushiAnimator.SetBool("isRight", true);
                }
                pos_arrow_obj.transform.localPosition = new Vector3(0,arrowpos,0);
                try{
                    Vector3 gyro = m_joyconL.GetGyro();
                    Vector3 accel = m_joyconL.GetAccel();
                    if(gyro.y <= -10f){
                        if(Math.Abs(accel.z)>=2){
                            pos_line_obj.SetActive(false);
                            pos_arrow_obj.SetActive(false);
                            hook_obj.SetActive(true);
                            string_obj.SetActive(true);
                            SESource.PlayOneShot(SoundEffects[8]);
                            string_target_pos = pos_arrow_obj.transform.localPosition.y;
                            string_edge_y = 0f;
                            string_scale = ScaleAround(string_obj, new Vector3(0f,0f,0f), new Vector3(1f, 0.01f, 1f));
                            string_edge_y = string_obj.transform.localScale.y / 2f + string_obj.transform.localPosition.y;
                            hook_obj.transform.localPosition = new Vector3(0f,string_edge_y, 0f);
                            ryoushiAnimator.SetBool("isThrowing",true);
                            phase = 1;
                        }
                    }
                }catch(System.NullReferenceException e){
                    if(Input.GetKey(KeyCode.Return)){
                        pos_line_obj.SetActive(false);
                        pos_arrow_obj.SetActive(false);
                        hook_obj.SetActive(true);
                        string_obj.SetActive(true);
                        string_target_pos = pos_arrow_obj.transform.localPosition.y;
                        string_edge_y = 0f;
                        string_scale = ScaleAround(string_obj, new Vector3(0f,0f,0f), new Vector3(1f, 0.01f, 1f));
                        string_edge_y = string_obj.transform.localScale.y / 2f + string_obj.transform.localPosition.y;
                        hook_obj.transform.localPosition = new Vector3(0f,string_edge_y, 0f);
                        ryoushiAnimator.SetBool("isThrowing",true);
                        phase = 1;
                    }
                }
                
                // pos_rail_obj.transform.rotation = new Quaternion(0,0,arrowrot,0);
            break;
            case 1:
                Animtime+=Time.deltaTime;
                if(Animtime >= 0.5f){
                    phase = 2;
                }
            break;
            case 2:
                NowGameState = (int)FishingGameStates.Play_throwing;
            break;
        }
        
    }

    void play_throwing(){
        if(string_edge_y <= string_target_pos){
            string_scale += 350.0f * Time.deltaTime;
            string_scale = ScaleAround(string_obj, new Vector3(0f,0f,0f), new Vector3(1f, string_scale, 1f));
            string_edge_y = string_obj.transform.localScale.y / 2f + string_obj.transform.localPosition.y;
            hook_obj.transform.localPosition = new Vector3(0f,string_edge_y, 0f);
            Debug.Log("stp ="+string_target_pos+" localscaley= "+string_obj.transform.localScale.y + " localposy="+string_obj.transform.localPosition.y +" edge="+string_edge_y);
        }else{
            string_scale = (string_target_pos - string_obj.transform.localPosition.y) * 2f;
            string_scale = ScaleAround(string_obj, new Vector3(0f,0f,0f), new Vector3(1f, string_scale, 1f));
            string_edge_y = string_obj.transform.localScale.y / 2f + string_obj.transform.localPosition.y;
            SESource.PlayOneShot(SoundEffects[11]);
            hook_obj.transform.localPosition = new Vector3(0f,string_edge_y, 0f);
            try{
                RumbleC.GetComponent<Animator>().SetBool("Rumble",true);
            }catch(System.NullReferenceException e){}
            

            NowGameState = (int)FishingGameStates.Play_hit_judge;
        }
    }

    void play_hit_judge(){
        hit = -1;
        int index = 0;
        float nearest_dist= 10000f;
        RumbleC.GetComponent<Animator>().SetBool("Rumble",false);
        foreach(Fish f in Fishes){
            GameObject farea = f.getCatchAreaPrefab();
            float dist = Vector3.Distance(hook_obj.transform.position, farea.transform.position);
            catch_dist = farea.GetComponent<SpriteRenderer>().bounds.size.x / 2;
            if(dist<=catch_dist){
                if(dist<nearest_dist){
                    hit = index;
                    nearest_dist = dist;
                }
            }
            // Debug.Log("hook_pos="+hook_obj.transform.position+ "farea_pos="+farea.transform.position 
            //     +" fareascale="+farea.GetComponent<SpriteRenderer>().bounds.size+" dist="+dist);
            index +=1;
        }
        if(hit>=0){
            //針に魚を引き寄せる
            Debug.Log("hooking");
            GameObject f = Fishes[hit].getFishPrefab();
            Vector3 before_pos = f.transform.localPosition;
            Fishes[hit].setActive(false);
            f.GetComponent<Animator>().enabled = false;
            f.transform.localPosition = before_pos;

            //角度を決める
            Vector3 fishXYZ = Fishes[hit].getFishPrefab().transform.position;
            Vector3 hookXYZ = hook_obj.transform.position;
            double rad = Math.Atan2(hookXYZ.y - fishXYZ.y, hookXYZ.x - fishXYZ.x);
            fish_target_dec = (float)(rad * 180.0 / Math.PI);
            if(f.transform.rotation.y == 0f){
                if(hookXYZ.y >= fishXYZ.y)
                    fish_target_dec = -1*(180-fish_target_dec);
                else
                    fish_target_dec = (180+fish_target_dec);
                pullingFishRot = Quaternion.Euler(0f,0f,90f);
            }else{
                fish_target_dec *= -1;
                pullingFishRot = Quaternion.Euler(0f,180f,90f);
            }
            fish_box_dec = 0f;
            Debug.Log("rad="+rad+" dec ="+fish_target_dec);
            NowGameState = (int)FishingGameStates.Play_catch_hooking_rotate;
        }else{
            ryoushiAnimator.SetBool("isCatching",true);
            NowGameState = (int)FishingGameStates.Play_catch_pull;
        }
    }

    

    void play_catch_hooking_rotate(){
        float decdiff;
        if(fish_target_dec >=0){
            decdiff = rotFishSpeed * Time.deltaTime;
            fish_box_dec+=decdiff;
            if(fish_box_dec >= fish_target_dec){
                fish_box_dec = fish_target_dec;
                decdiff = fish_target_dec - fish_box_dec;
                GameObject f = Fishes[hit].getFishPrefab();
                f.transform.SetParent(hook_obj.transform);
                Vector3 fpos = f.transform.position;
                Vector3 hpos = hook_obj.transform.position;
                posChangeSpeed = new Vector2(hpos.x - fpos.x , hpos.y - fpos.x);
                
                poschangeFlag = false;
                NowGameState = (int)FishingGameStates.Play_catch_hooking_pos;
            }
        }else{
            decdiff = rotFishSpeed * Time.deltaTime *(-1);
            fish_box_dec+=decdiff;
            if(fish_box_dec <= fish_target_dec){
                fish_box_dec = fish_target_dec;
                decdiff = fish_target_dec - fish_box_dec;
                GameObject f = Fishes[hit].getFishPrefab();
                f.transform.SetParent(hook_obj.transform);
                Vector3 fpos = f.transform.position;
                Vector3 hpos = hook_obj.transform.position;
                posChangeSpeed = new Vector2(hpos.x - fpos.x, hpos.y - fpos.x);
                poschangeFlag = false;
                NowGameState = (int)FishingGameStates.Play_catch_hooking_pos;
            }
        }
        
        Fishes[hit].getFishPrefab().transform.Rotate(new Vector3(0f,0f,decdiff));
    }

    void play_catch_hooking_pos(){
        GameObject f = Fishes[hit].getFishPrefab();
        // float dt = Time.deltaTime;
        f.transform.localPosition = Vector3.MoveTowards(f.transform.localPosition, new Vector3(0f,0f,0f),0.1f);
        Debug.Log(posChangeSpeed);
        if(f.transform.localPosition == new Vector3(0,0,0)){
            phase = 0;
            tactics_time = 0;
            tactics_count = 0;
            NowGameState = (int)FishingGameStates.Play_tactics;
        }        
    }

    void play_tactics(){
        
        if(Fishes[hit].getKind().getIsBig()){
            switch(phase){
                case 0:
                    RendaButton.SetActive(true);
                    RumbleC.GetComponent<Animator>().SetBool("Rumbling",true);
                    RendaButton.GetComponent<Animator>().enabled = true;
                    if(hook_obj.transform.position.x >0){
                        RendaButton.transform.position = hook_obj.transform.position - new Vector3(2,1,0);
                    }else{
                        RendaButton.transform.position = hook_obj.transform.position + new Vector3(2,1,0);
                    }
                    
                    phase = 1;
                break;
                case 1:
                    tactics_time += Time.deltaTime;
                    try{
                        if(m_joyconL.GetButtonDown(Joycon.Button.DPAD_RIGHT)
                        || m_joyconL.GetButtonDown(Joycon.Button.DPAD_DOWN)
                        || m_joyconL.GetButtonDown(Joycon.Button.DPAD_LEFT)
                        || m_joyconL.GetButtonDown(Joycon.Button.DPAD_UP)){
                            tactics_count+=1;
                        }
                    }catch(System.NullReferenceException e){
                        if(Input.GetKeyDown(KeyCode.Return)){
                            tactics_count+=1;
                        }
                    }
                    if(tactics_count >= tactics_count_objective){
                        RendaButton.SetActive(false);
                        Message.SetActive(true);
                        RumbleC.GetComponent<Animator>().SetBool("Rumbling",false);
                        Message.GetComponent<SpriteRenderer>().sprite = messages[0];
                        if(hook_obj.transform.position.x >0){
                        Message.transform.position = hook_obj.transform.position - new Vector3(3.5f,-1,0);
                    }else{
                        Message.transform.position = hook_obj.transform.position + new Vector3(4,1,0);
                    }
                        phase = 2;
                    }
                    if(tactics_time > tactics_time_limit){
                        destroyFish(hit);
                        RumbleC.GetComponent<Animator>().SetBool("Rumbling",false);
                        RendaButton.GetComponent<Animator>().enabled = false;
                        RendaButton.SetActive(false);
                        SESource.PlayOneShot(SoundEffects[11]);
                        hit = -1;
                        ryoushiAnimator.SetBool("isCatching",true);
                        NowGameState = (int)FishingGameStates.Play_catch_pull;
                    }
                break;
                case 2:
                    try{
                        Vector3 gyro = m_joyconL.GetGyro();
                        Vector3 accel = m_joyconL.GetAccel();
                        if(gyro.y>=10){
                            if(Math.Abs(accel.z)>=2){
                                ryoushiAnimator.SetBool("isCatching",true);
                                RumbleC.GetComponent<Animator>().SetBool("Rumbling",true);
                                SESource.PlayOneShot(SoundEffects[11]);
                                NowGameState = (int)FishingGameStates.Play_catch_pull;
                            }
                        }
                    }catch(System.NullReferenceException e){
                        if(Input.GetKey(KeyCode.Return)){
                            ryoushiAnimator.SetBool("isCatching",true);
                            SESource.PlayOneShot(SoundEffects[11]);
                            NowGameState = (int)FishingGameStates.Play_catch_pull;    
                        }
                    }
                break;
            }


        }else{
            //戦術がない場合
            switch(phase){
                case 0:
                    Message.SetActive(true);
                    Message.GetComponent<SpriteRenderer>().sprite = messages[0];
                    if(hook_obj.transform.position.x >0){
                        Message.transform.position = hook_obj.transform.position - new Vector3(3.5f,-1,0);
                    }else{
                        Message.transform.position = hook_obj.transform.position + new Vector3(4,1,0);
                    }
                    phase = 1;
                break;
                case 1:
                    try{
                        Vector3 gyro = m_joyconL.GetGyro();
                        Vector3 accel = m_joyconL.GetAccel();
                        if(gyro.y>=10){
                            if(Math.Abs(accel.z)>=2){
                                Message.SetActive(false);
                                ryoushiAnimator.SetBool("isCatching",true);
                                RumbleC.GetComponent<Animator>().SetBool("Rumbling",true);
                                SESource.PlayOneShot(SoundEffects[11]);
                                NowGameState = (int)FishingGameStates.Play_catch_pull;
                            }
                        }
                    }catch(System.NullReferenceException e){
                        if(Input.GetKey(KeyCode.Return)){
                            Message.SetActive(false);
                            ryoushiAnimator.SetBool("isCatching",true);
                            SESource.PlayOneShot(SoundEffects[11]);
                            NowGameState = (int)FishingGameStates.Play_catch_pull;    
                        }
                    }
                break;
            }
            
        }
    }
    
    void play_catch_pull(){
        //引き揚げ
        GameObject f = null;
        if(hit >= 0){
            f = Fishes[hit].getFishPrefab();
            gain_point.text = Fishes[hit].getKind().getPoint().ToString();
            fish_name.text = Fishes[hit].getKind().getName();
            checkfish_fish.GetComponent<SpriteRenderer>().sprite = Fishes[hit].getKind().getSprite();
        }
        if(string_edge_y >= 0.0f){
            string_scale -= 200.0f * Time.deltaTime;
            string_scale = ScaleAround(string_obj, new Vector3(0f,0f,0f), new Vector3(1f, string_scale, 1f));
            string_edge_y = string_obj.transform.localScale.y / 2f + string_obj.transform.localPosition.y;
            hook_obj.transform.localPosition = new Vector3(0f,string_edge_y, 0f);
            if(hit>=0){
                f.transform.localRotation = Quaternion.RotateTowards(f.transform.localRotation, pullingFishRot, 360f*Time.deltaTime);
            }
            Debug.Log("stp ="+string_target_pos+" localscaley= "+string_obj.transform.localScale.y + " localposy="+string_obj.transform.localPosition.y +" edge="+string_edge_y);
        }else{
            string_scale = (0f - string_obj.transform.localPosition.y) * 2f;
            string_scale = ScaleAround(string_obj, new Vector3(0f,0f,0f), new Vector3(1f, string_scale, 1f));
            string_edge_y = string_obj.transform.localScale.y / 2f + string_obj.transform.localPosition.y;
            hook_obj.transform.localPosition = new Vector3(0f,string_edge_y, 0f);
            RumbleC.GetComponent<Animator>().SetBool("Rumbling",false);
            if(hit>=0){
                catch_net.GetComponent<Animator>().SetBool("ready", true);
                Message.SetActive(true);
                Message.transform.localPosition = new Vector3(0f, -30f,0f);
                Message.GetComponent<SpriteRenderer>().sprite = messages[1];
            }else{
                ryoushiAnimator.SetBool("isThrowing",false);
                ryoushiAnimator.SetBool("isCatching",false);
            }
            phase=0;
            NowGameState = (int)FishingGameStates.Play_catch_net;
        }
    }

    void play_catch_net(){
        if(hit >= 0){
            try{
                Vector3 gyro = m_joyconR.GetGyro();
                Vector3 accel = m_joyconR.GetAccel();
                Quaternion vec = m_joyconR.GetVector();
                switch(phase){
                    case 0:
                        if(gyro.y <= -5){
                            if(Math.Abs(accel.z) >= 3){
                                phase = 1;
                                catch_net.GetComponent<Animator>().SetBool("shake", true);
                                Debug.Log("netcatchphase = "+phase);
                                SESource.PlayOneShot(SoundEffects[0]);
                                RumbleC.GetComponent<Animator>().SetBool("RumbleR",true);
                                net_waittime = 0f;
                            }
                        }
                        break;
                    case 1:
                        net_waittime += Time.deltaTime;
                        if(net_waittime >= 1.5f){
                            phase = 2;
                            Fishes[hit].getFishPrefab().GetComponent<SpriteRenderer>().color = new Color32(255,255,255,0);
                            Fishes[hit].setShow(false);
                            RumbleC.GetComponent<Animator>().SetBool("RumbleR",false);
                            SESource.PlayOneShot(SoundEffects[11]);
                            Debug.Log("netcatchphase = "+phase);
                        }
                        break;
                    case 2:
                        if(gyro.y >= 5){
                            if(Math.Abs(accel.z) >= 3){
                                phase = 3;
                                catch_net.GetComponent<Animator>().SetBool("shake", false);
                                catch_net.GetComponent<Animator>().SetBool("ready", false);
                                RumbleC.GetComponent<Animator>().SetBool("RumbleR",true);
                                SESource.PlayOneShot(SoundEffects[0]);
                                Debug.Log("netcatchphase = "+phase);
                                net_waittime = 0f;
                            }
                        }
                        break;
                    case 3:
                        net_waittime += Time.deltaTime;
                        if(net_waittime >= 2f){
                            CheckFish_group.SetActive(true);
                            Message.SetActive(false);
                            NowGameState = (int)FishingGameStates.Check_Fishkind;
                            RumbleC.GetComponent<Animator>().SetBool("RumbleR",false);
                            timeCnt.stop();
                        }
                        break;
                        
                }
                
            }catch(System.NullReferenceException e){
                if(Input.GetKeyDown(KeyCode.Return)){
                    Message.SetActive(false);
                    NowGameState = (int)FishingGameStates.Check_Fishkind;
                }
            }
            
        }else{
            catching = false;
            hooked = false;
            hook_obj.SetActive(false);
            string_obj.SetActive(false);
            pos_line_obj.SetActive(true);
            pos_arrow_obj.SetActive(true);
            phase = 0;
            Animtime = 0f;
            NowGameState = (int)FishingGameStates.Play_pos;
        }
        
    }

    void check_fishkind(){
        try{
            if(m_joyconR.GetButton(Joycon.Button.DPAD_RIGHT)){
                GameScore += Fishes[hit].getKind().getPoint();
                Fishes[hit].getKind().countUp();
                FishCount += 1;
                destroyFish(hit);
                float norma_percent = (float)FishCount/ (float)norma_count;
                if(norma_percent > 1f) norma_percent = 1f;
                NormaGauge.fillAmount = norma_percent;
                catching = false;
                hooked = false;
                ryoushiAnimator.SetBool("isThrowing",false);
                ryoushiAnimator.SetBool("isCatching",false);
                hook_obj.SetActive(false);
                string_obj.SetActive(false);
                pos_line_obj.SetActive(true);
                pos_arrow_obj.SetActive(true);
                CheckFish_group.SetActive(false);
                if(norma_percent < 1){
                    timeCnt.move();
                    phase = 0;
                    Animtime = 0f;
                    NowGameState = (int)FishingGameStates.Play_pos;
                }else{
                    hideSprites();
                    SelectContinueGroup.SetActive(true);
                    select_num = 0;
                    NowGameState = (int)FishingGameStates.Select_continue;
                }
                
            }
        }catch(System.NullReferenceException e){
            if(Input.GetKey(KeyCode.Return)){
                GameScore += Fishes[hit].getKind().getPoint();
                Fishes[hit].getKind().countUp();
                FishCount += 1;
                float norma_percent = (float)FishCount/ (float)norma_count;
                if(norma_percent > 1f) norma_percent = 1f;
                NormaGauge.fillAmount = norma_percent;
                destroyFish(hit);
                catching = false;
                hooked = false;
                ryoushiAnimator.SetBool("isThrowing",false);
                ryoushiAnimator.SetBool("isCatching",false);
                hook_obj.SetActive(false);
                string_obj.SetActive(false);
                pos_line_obj.SetActive(true);
                pos_arrow_obj.SetActive(true);
                if(norma_percent < 1){
                    timeCnt.move();
                    phase = 0;
                    Animtime=0f;
                    NowGameState = (int)FishingGameStates.Play_pos;
                }else{
                    hideSprites();
                    SelectContinueGroup.SetActive(true);
                    select_num = 0;
                    NowGameState = (int)FishingGameStates.Select_continue;
                }
            }
        }
    }

    void select_continue(){
        try{
            if(m_joyconL.GetButtonDown(Joycon.Button.DPAD_LEFT))select_num -= 1;
            if(m_joyconL.GetButtonDown(Joycon.Button.DPAD_RIGHT))select_num += 1;
            if(select_num < 0) select_num =0;
            if(select_num > 1) select_num =1;
            if(select_num == 0){
                sc_yes.GetComponent<Image>().color = new Color32(255,255,255,255);
                sc_no.GetComponent<Image>().color = new Color32(125,125,125,255);
            }else{
                sc_yes.GetComponent<Image>().color = new Color32(125,125,125,255);
                sc_no.GetComponent<Image>().color = new Color32(255,255,255,255);
            }
            if(m_joyconR.GetButtonDown(Joycon.Button.DPAD_RIGHT)){
                if(select_num == 0){
                    showSprites();
                    timeCnt.move();
                    SelectContinueGroup.SetActive(false);
                    phase = 0;
                    Animtime = 0f;
                    NowGameState = (int)FishingGameStates.Play_pos;
                }else{
                    SelectContinueGroup.SetActive(false);
                    hideSprites();
                    EndGroup.SetActive(true);
                    GameClearobj.GetComponent<SpriteRenderer>().sprite = GameClear[0];
                    phase = 0;
                    SESource.PlayOneShot(SoundEffects[3]);
                    NowGameState = (int)FishingGameStates.End;
                }
            }
        }catch(System.NullReferenceException e){}
    }

    void end(){
        switch(phase){
            case 0:
                try{
                    if(m_joyconR.GetButtonDown(Joycon.Button.DPAD_RIGHT)){
                        SESource.PlayOneShot(SoundEffects[2]);
                        EndGroup.SetActive(false);
                        EndResultGroup.SetActive(true);
                        int total=0;
                        for(int i = 0; i<Fishkinds.Count;i++){
                            EndResultGroup.transform.GetChild(i).Find("number").gameObject.GetComponent<Text>().text = Fishkinds[i].getCount().ToString();
                            EndResultGroup.transform.GetChild(i).Find("score").gameObject.GetComponent<Text>().text = (Fishkinds[i].getCount()* Fishkinds[i].getPoint()).ToString();
                            total += Fishkinds[i].getCount()* Fishkinds[i].getPoint();
                        }
                        EndResultGroup.transform.Find("total_score").gameObject.GetComponent<Text>().text = total.ToString();
                        EndResultGroup.GetComponent<Animator>().enabled = true;
                        if(Fishkinds[4].getCount() > 0){
                            EndResultGroup.GetComponent<Animator>().SetBool("isContainRare",true);
                        }
                        EndResultGroup.GetComponent<Animator>().SetBool("resultShowing",true);
                        phase = 1;
                    }
                }catch(System.NullReferenceException e){
                    
                    if(Input.GetKeyDown(KeyCode.Return)){
                        EndGroup.SetActive(false);
                        EndResultGroup.SetActive(true);
                        EndResultGroup.GetComponent<Animator>().enabled = true;
                        if(Fishkinds[4].getCount() > 0){
                            EndResultGroup.GetComponent<Animator>().SetBool("isContainRare",true);
                        }
                        EndResultGroup.GetComponent<Animator>().SetBool("resultShowing",true);
                        phase = 1;
                    }
                }
            break;
            case 1:
                try{
                    if(m_joyconR.GetButtonDown(Joycon.Button.DPAD_RIGHT)){
                        if(FishCount < norma_count + 5)
                            Global.ED = 1;
                        else
                            Global.ED = 2;
                        FadeController.SceneFadeOut(1f, "ED");
                    }
                }catch(System.NullReferenceException e){
                    if(Input.GetKeyDown(KeyCode.Return)){
                        if(FishCount < norma_count + 5)
                            Global.ED = 1;
                        else
                            Global.ED = 2;
                        FadeController.SceneFadeOut(1f, "ED");
                    }
                }
            break;
        }
    }

    void pause(){
        try{
            if(m_joyconL.GetButtonDown(Joycon.Button.DPAD_UP)) select_num -=1;
            if(m_joyconL.GetButtonDown(Joycon.Button.DPAD_DOWN)) select_num +=1;
            int button_count = PauseButtonGroup.transform.childCount;
            if(select_num >= button_count) select_num = button_count-1;
            if(select_num < 0) select_num = 0;
            for(int i=0; i<button_count; i++){
                if(i == select_num){
                    PauseButtonGroup.transform.GetChild(i).GetComponent<Image>().sprite = pausebtn_active[i];
                }else{
                    PauseButtonGroup.transform.GetChild(i).GetComponent<Image>().sprite = pausebtn_inactive[i];
                }    
            }
            if(m_joyconR.GetButtonDown(Joycon.Button.DPAD_RIGHT)){
                if(select_num == 0){
                    Debug.Log("pause 0");
                    SESource.PlayOneShot(SoundEffects[1]);
                    FPMC.returnGame();
                }else if(select_num == 1){
                    Debug.Log("pause 1");
                    SESource.PlayOneShot(SoundEffects[9]);
                    FPMC.restart();
                }else if(select_num == 2){
                    Debug.Log("pause 2");
                    SESource.PlayOneShot(SoundEffects[9]);
                    FPMC.back_menu();                    
                }
            }

        }catch(System.NullReferenceException e){}
    }

    void joycon_setup(){
        m_joycons = JoyconManager.Instance.j;
        if (m_joycons == null || m_joycons.Count <= 0) return;

        m_joyconL = m_joycons.Find(c => c.isLeft);
        m_joyconR = m_joycons.Find(c => !c.isLeft);
    }

    void joycon_update(){
        m_pressedButtonL = null;
        m_pressedButtonR = null;

        if (m_joycons == null || m_joycons.Count <= 0) return;

        foreach (var button in m_buttons)
        {
            //左ジョイコンが接続されていなければ例外処理
            try{
                if (m_joyconL.GetButton(button))
                {
                    m_pressedButtonL = button;
                }
            }catch(System.NullReferenceException e){}
            //右ジョイコンが接続されていなければ例外処理
            try{
                if (m_joyconR.GetButton(button))
                {
                    m_pressedButtonR = button;
                }
            }catch(System.NullReferenceException e){}
            
        }
    }

    void timerupdate(){
        time = (int)timeCnt.displayTime;
        float SecondHandRot = -1*(360 / GameTime) * (GameTime - time);
        SecondHand.transform.rotation = Quaternion.Euler(0,0,SecondHandRot);
        float timepercent = ((float)(GameTime - time) / (float)GameTime);
        Debug.Log("timeper=" + timepercent);
        TimerCircle.fillAmount = timepercent;
        if (time <= 0)
        {
            timeCnt.stop();
            hideSprites();
            EndGroup.SetActive(true);
            GameClearobj.GetComponent<SpriteRenderer>().sprite = GameClear[2];
            SESource.PlayOneShot(SoundEffects[3]);
            phase = 0;
            NowGameState = (int)FishingGameStates.End;
        }
    }

    void pause_button_check(){
        if(pausebtn.enabled){
            try{
                if(m_joyconR.GetButtonDown(Joycon.Button.PLUS)){
                    select_num = 0;
                    FPBC.PauseButton();
                }
            }catch(System.NullReferenceException e){}
        }
    }

    void generateFish(){
        float x = UnityEngine.Random.Range(SpawnFishRange[0].x, SpawnFishRange[1].x);
        float y = UnityEngine.Random.Range(SpawnFishRange[0].y, SpawnFishRange[1].y);
        Vector3 pos = new Vector3(x,y,0f);
        float kind_dice = UnityEngine.Random.Range(0.0f,1.0f);
        float diceres =0f;
        Fishkind kind = null;
        int index = 0;
        while(diceres < 1f){
            diceres = Fishkinds[index].getWeight();
            if(kind_dice <= diceres){
                kind = Fishkinds[index];
                break;
            }
            kind_dice -= diceres;
            index += 1;
        }
        if(kind == null)kind=Fishkinds[0];
        if(norma_count == FishCount) kind = Fishkinds[4];
        Fish sakana = new Fish(kind, boxList, pos);
        GameObject boxobject = Instantiate(sakana.getBox(), spritecanvas.transform, false) as GameObject;
        GameObject sakanaobject = boxobject.transform.GetChild(0).gameObject;
        GameObject catch_areaobject = sakanaobject.transform.Find("catch_area").gameObject;
        sakana.register_object(boxobject, sakanaobject, catch_areaobject);
        boxobject.transform.localPosition=sakana.getPos();
        sakanaobject.transform.localPosition=new Vector3(0,0,0);
        sakanaobject.GetComponent<SpriteRenderer>().sortingOrder=1;
        Fishes.Add(sakana);
    }

    void destroyFish(int index){
        Destroy(Fishes[index].getFishPrefab());
        Destroy(Fishes[index].getBoxPrefab());
        Fishes.RemoveAt(index);
    }

    void hideSprite(GameObject obj){
        obj.GetComponent<SpriteRenderer>().color = new Color32(255,255,255,0);
    }

    void hideAnimatedSprite(GameObject obj){
        obj.GetComponent<Animator>().enabled = false;
        obj.GetComponent<SpriteRenderer>().color = new Color32(255,255,255,0);
    }

    void showSprite(GameObject obj){
        obj.GetComponent<SpriteRenderer>().color = new Color32(255,255,255,255);
    }

    void showAnimatedSprite(GameObject obj){
        obj.GetComponent<Animator>().enabled = true;
        obj.GetComponent<SpriteRenderer>().color = new Color32(255,255,255,255);
    }


    public void hideSprites(){
        NormaGauge_group.SetActive(false);
        Timer_group.SetActive(false);
        pos_desition_group.SetActive(false);
        before_enabled["message"]=Message.activeInHierarchy;
        RumbleC.GetComponent<Animator>().enabled = false;
        RumbleC.GetComponent<RumbleController>().Rumble1 = false;
        RumbleC.GetComponent<RumbleController>().Rumble2 = false;
        Message.SetActive(false);
        foreach(Fish f in Fishes){
            f.setActive(false);
            hideAnimatedSprite(f.getFishPrefab());
            f.setShow(false);
        }
        before_enabled["catch_net"]=(catch_net.GetComponent<SpriteRenderer>().color.a >0f);
        hideAnimatedSprite(metcharactor);
        Debug.Log("catch_net_enabled"+before_enabled["catch_net"]);
        hideAnimatedSprite(catch_net);

    }

    public void showSprites(){
        NormaGauge_group.SetActive(true);
        Timer_group.SetActive(true);
        pos_desition_group.SetActive(true);
        RumbleC.GetComponent<Animator>().enabled = true;
        Message.SetActive(before_enabled["message"]);
        foreach(Fish f in Fishes){
            if(f.isBeforeActive()){
                f.getFishPrefab().GetComponent<Animator>().enabled = true;
                f.setActive(true);
            }            
            SpriteRenderer fs = f.getFishPrefab().GetComponent<SpriteRenderer>();
            if(f.isBeforeShow()){
                fs.color = new Color32(255,255,255,255);
            }else{
                fs.color = new Color32(255,255,255,0);
            }
            f.setShow(f.isBeforeShow());
        }
        if(before_enabled["catch_net"]){
            showAnimatedSprite(catch_net);
        }else{
            catch_net.GetComponent<Animator>().enabled = true;
        }
        showAnimatedSprite(metcharactor);
    }

    private void OnGUI()
    {
        var style = GUI.skin.GetStyle("label");
        style.fontSize = 24;
        if (m_joycons == null || m_joycons.Count <= 0)
        {
            GUILayout.Label("Joy-Con が接続されていません");
            return;
        }

        if (!m_joycons.Any(c => c.isLeft))
        {
            GUILayout.Label("Joy-Con (L) が接続されていません");
            return;
        }
        if(!debug){
            if (!m_joycons.Any(c => !c.isLeft))
            {
                GUILayout.Label("Joy-con (R) が接続されていません");
                return;
            }
        }
        GUILayout.BeginHorizontal(GUILayout.Width(960));
        foreach (var joycon in m_joycons)
        {
            var isLeft = joycon.isLeft;
            var name = isLeft ? "Joy-Con (L)" : "Joy-Con (R)";
            var key = isLeft ? "Z キー" : "X キー";
            var button = isLeft ? m_pressedButtonL : m_pressedButtonR;
            var stick = joycon.GetStick();
            var gyro = joycon.GetGyro();
            var accel = joycon.GetAccel();
            var orientation = joycon.GetVector();
            var katamuki = new Quaternion(orientation.z, orientation.y, orientation.x, orientation.w);
            if(debug){
                GUILayout.BeginVertical(GUILayout.Width(480));
                GUILayout.Label(name);
                GUILayout.Label(key + "：振動");
                GUILayout.Label("押されているボタン：" + button);
                GUILayout.Label(string.Format("スティック：({0}, {1})", stick[0], stick[1]));
                GUILayout.Label("ジャイロ：" + gyro);
                GUILayout.Label("加速度：" + accel);
                GUILayout.Label("傾き：" + orientation);
                GUILayout.Label("変換傾き：" + katamuki);
                GUILayout.EndVertical();
            }
        }

        GUILayout.EndHorizontal();
    }

    public float ScaleAround(GameObject target, Vector3 pivot, Vector3 newScale){
        Vector3 targetPos = target.transform.localPosition;
        Vector3 diff = targetPos - pivot;
        Debug.Log(targetPos);
        float relativeScale = newScale.y / target.transform.localScale.y;

        Vector3 resultPos = pivot + diff * relativeScale; 
        target.transform.localScale = newScale;
        target.transform.localPosition = resultPos;
        return target.transform.localScale.y;
    }


}

class Fish{
    GameObject box;
    GameObject box_object;
    GameObject fish_object;
    GameObject catch_area_object;
    Fishkind kind;
    Vector3 pivot;
    bool show;
    bool beforeshow;
    bool before_active;
    bool active;
    public Fish(){}
    public Fish(Fishkind kind, GameObject[] objects, Vector3 pivot){
        this.kind = kind;
        if(this.kind.getIsBig()){
            this.box = objects[1];
        }else{
            this.box = objects[0];
        }
        this.show = true;
        this.pivot = pivot;
        this.active = true;
        this.before_active = true;
    }

    public Fishkind getKind(){
        return kind;
    }

    public GameObject getBox(){
        return box;
    }

    public GameObject getBoxPrefab(){
        return box_object;
    }

    public GameObject getFishPrefab(){
        return fish_object;
    }

    public GameObject getCatchAreaPrefab(){
        return catch_area_object;
    }

    public Vector3 getPos(){
        return pivot;
    }

    public bool isShow(){
        return show;
    }

    public bool isBeforeShow(){
        return beforeshow;
    }

    public bool isBeforeActive(){
        return before_active;
    }

    public bool isActive(){
        return active;
    }

    public void setShow(bool b){
        this.beforeshow = this.show;
        this.show = b;
    }

    public void setActive(bool b){
        this.before_active = this.active;
        this.active = b;
    }

    public void register_object(GameObject boxobject, GameObject fishobject, GameObject catch_area_object){
        this.box_object = boxobject;
        this.fish_object = fishobject;
        this.catch_area_object = catch_area_object;
    }

}

class Fishkind{
    Sprite sprite;
    string name;
    int point;
    bool isBig;
    float weight;
    int count;
    public Fishkind(){}
    public Fishkind(string name, Sprite fishsprite, int point, float weight, bool isBig=false){
        this.name = name;
        this.sprite = fishsprite;
        this.point = point;
        this.weight = weight;
        this.isBig = isBig;
        this.count = 0;
    }

    public Sprite getSprite(){
        return this.sprite;
    }
    public string getName(){
        return this.name;
    }
    public int getPoint(){
        return this.point;
    }
    public bool getIsBig(){
        return this.isBig;
    }
    public float getWeight(){
        return this.weight;
    }

    public void countUp(){
        this.count +=1;
    }

    public int getCount(){
        return this.count;
    }

}