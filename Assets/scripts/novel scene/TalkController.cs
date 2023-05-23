using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TalkController : MonoBehaviour
{
    GameObject[] prefabs = new GameObject[2];
    //UIパーツ関連 
    public GameObject selection;
    string logs ="";
    public Text name;
    public Text talk;
    public GameObject LogObject;
    public GameObject BGMObject;
    public Text Log;
    public Image Log_back;
    public Image back;
    public Sprite back_tmp;
    public logbutton logbutton;
    //エンディングならTRUE
    public bool isED;
    //キャラクター関連
    //chara_spritesはキャラクター番号に対応するキャラを格納してください。
    public Sprite[] chara_sprites = new Sprite[20];
    public Image[] chara_object = new Image[2];
    public int[] chara_object_id = new int[2];
    List<string> charactors = new List<string>();

    //hero_spritesは0番に普段着,1番に宇宙服姿の主人公を入れてください。
    public Sprite[] hero_sprites = new Sprite[2];

    //ジョイコン関連
    private static readonly Joycon.Button[] m_buttons = 
        Enum.GetValues(typeof(Joycon.Button)) as Joycon.Button[];
    private List<Joycon> m_joycons;
    private Joycon m_joyconL;
    private Joycon m_joyconR;
    private Joycon.Button? m_pressedButtonL;
    private Joycon.Button? m_pressedButtonR;
    Quaternion orientation;
    Vector3 accel;
    public bool debug;

    //トーク遷移制御関連
    public string filename;
    public GameObject select_uis;
    public GameObject[] selects = new GameObject[2];
    int selecting = 0;
    public Sprite[] backs = new Sprite[10];
    public AudioClip[] bgms = new AudioClip[5];
    TalkFromFile talkf;
    public string current_talk;
    public string current_name;
    public bool isLogMenu;
    bool isFading;
    int YesNo=-1;
    bool YesNoPressed;
    int backcount=0;
    readonly string[] ex_patterns={"<yn>","<y>","<n>","<fade>","<change_back>","<change_scene>","<change_bgm>","<change_chara>","<change_uniform>","<show>","<hide>","<end>"};

    //テキスト内スクリプト使い方
    // <yn>[疑問文],[選択肢1]|[選択肢2]
    // <y>[選択肢1を選んだ時の処理]
    // <n>[選択肢2を選んだ時の処理]
    // <fade>
    // <change_back>[背景番号]
    // <change_bgm>[BGM番号]
    // <change_scene>[シーン名]
    // <change_chara>[オブジェクト位置],[キャラクター番号]
    // <end>



    // Start is called before the first frame update
    void Start()
    {
        //ジョイコン関連処理
        joycon_init();
        //エンディング専用処理
        if(isED){
            if(Global.ED==0){
                filename="metcityendb";
            }else if(Global.ED==1){
                filename="metcityenda";
            }else if(Global.ED==2){
                filename="metcityendc";
            }
        }
        //変数初期化
        isLogMenu=false;
        isFading=false;
        YesNoPressed=false;
        Log = LogObject.GetComponent<Text>();
        Log_back.enabled = false;
        Log.enabled=false;
        select_uis.SetActive(false);
        talkf = new TalkFromFile(filename);
        charactors = talkf.getCharas();
        for (int i=0; i<2; i++){
            chara_object[i].sprite=chara_sprites[chara_object_id[i]];
        }
        //画面更新
        update();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(YesNoPressed);
        joycon_update();
        //ジョイコンボタン判定
        //右ジョイコンXボタンでログを開く
        try{
            if(m_joyconR.GetButtonUp(Joycon.Button.DPAD_UP)){
                if(isLogMenu){
                    isLogMenu=false;
                }else{
                    isLogMenu=false;
                }
                logbutton.OnClick();
            }
        }catch(System.NullReferenceException e){}
        //選択肢でジョイコン操作
        try{
            if(YesNoPressed){
                if(m_joyconL.GetButtonUp(Joycon.Button.DPAD_UP)) selecting -=1;
                else if(m_joyconL.GetButtonUp(Joycon.Button.DPAD_DOWN)) selecting +=1;
                if(selecting <0) selecting = 0; else if(selecting > 1) selecting = 1;
                if(m_joyconR.GetButtonUp(Joycon.Button.DPAD_RIGHT)){
                    if(selecting == 0) yes();
                    else if(selecting == 1) no();
                }
                if(selecting == 0){
                    selects[0].GetComponent<Image>().color = new Color32(255,255,255,255);
                    selects[1].GetComponent<Image>().color = new Color32(133,133,133,255);
                }else if(selecting == 1){
                    selects[0].GetComponent<Image>().color = new Color32(133,133,133,255);
                    selects[1].GetComponent<Image>().color = new Color32(255,255,255,255);
                }
            }else{
                if(m_joyconR.GetButtonUp(Joycon.Button.DPAD_RIGHT)){
                    nextText();
                }
            }
        }catch(System.NullReferenceException e){}

        //ログ画面でマウスからの入力を検知
        if(isLogMenu){
            float scroll = Input.GetAxis("Mouse ScrollWheel");
            Vector3 pos = LogObject.transform.position;
            float height = LogObject.GetComponent<RectTransform>().rect.height;
            float ypos = pos.y;
            if(scroll >0){
                ypos -= 100;
            }else if(scroll <0){
                ypos += 100;
            }
            try{
                if (m_joyconL.GetButton(Joycon.Button.DPAD_UP) || m_joyconL.GetStick()[1] >= 0.5f){
                    ypos -=20;
                }else if(m_joyconL.GetButton(Joycon.Button.DPAD_DOWN) || m_joyconL.GetStick()[1] <= -0.5f){
                    ypos +=20;
                }
            }catch(System.NullReferenceException e){}
            
            if(ypos <0) ypos=0.0f;
            if(ypos >height) ypos = height;
            LogObject.transform.position = new Vector3(pos.x,ypos,pos.z);
        }
        
        
    }

    public void selection_init(string select_1, string select_2){
        Debug.Log("generated");
        select_uis.SetActive(true);
        selecting = 0;
        string[] sa = {select_1, select_2};
        int i = 0;
        foreach(GameObject s in selects){
            Text selects_text = s.transform.GetChild(0).GetComponent<Text>();
            selects_text.text = sa[i];
            i++;
        }
        YesNoPressed=true;
    }

    public string Getlogs(){
        return logs;
    }

    public void nextText(){
        if(isFading){
            fadeInEffect();
        }
        if(!YesNoPressed){
            if(!isLogMenu){
                talkf.nextText();
                update();
            }
        }
    }

    //テキストを更新
    void update(){
        if(current_name.Trim() !=""){
            if(current_talk.Trim() !=""){
                logs = logs + "\n"+current_name+" : "+(current_talk.Replace("\n",""));
                Log.text = logs;    
            }
        }else{
            logs = logs+"\n"+(current_talk.Replace("\n",""));
            Log.text = logs;
        }
        current_talk = talkf.getTalk();
        current_name = talkf.getChara();
        for(int i = 0; i<2; i++){
            Debug.Log("current_id: "+ searchChara(charactors,current_name));
            if (chara_object_id[i] == searchChara(charactors,current_name)){
                chara_object[i].color = new Color32(255,255,255,255);
            }else{
                chara_object[i].color = new Color32(114,114,114,255);
            }
            chara_object[i].sprite=chara_sprites[chara_object_id[i]];
        }
        Debug.Log("index: "+talkf.getIndex());
        

        current_talk = interpreter(current_talk);
        talk.text = current_talk;
        name.text = current_name;
    }

    //テキスト内表現を探して、それごとの処理を行う
    string interpreter(string talk){
        string ret = talk;
        if(isFading){
            isFading=false;
        }
        bool sw = false;
        foreach(string pt in ex_patterns){
            if(talk.Contains(pt)){
                ret = text_process(talk, pt);
                Debug.Log("pt: "+pt+"  ret: "+ret);
                sw = true;
            }
        }
        if(!sw) YesNo=-1;
        return ret;
    }

    string text_process(string text, string pt){
        string ret=text;
        switch(pt){
            case "<yn>":
                ret = YesNoChoice(ret);
                ret = ret.Replace(pt,"");
                break;
            case "<y>":
                Debug.Log(YesNo);
                if(YesNo==0){
                    //Yesの場合
                    ret = ret.Replace(pt,"");
                }else if(YesNo==1){
                    //Noの場合
                    //Noの選択肢の場所まで飛ばす
                    while(true){
                        talkf.nextText();
                        ret = talkf.getTalk();
                        if(ret.Contains(ex_patterns[1])){
                        }else{
                            ret = ret.Replace(ex_patterns[2],"");
                            break;
                        }
                    }
                }
                break;
            case "<n>":
                if(YesNo==0){
                    //Yesの場合
                    while(true){
                        talkf.nextText();
                        ret = talkf.getTalk();
                        if(!ret.Contains(ex_patterns[2])){
                            ret=interpreter(ret);
                            break;
                        }
                    }
                }else{
                    //Noの場合
                    ret = ret.Replace(pt,"");
                    break;
                }
                break;
            case "<fade>":
                fadeOutEffect();
                ret = ret.Replace(pt,"");
                talkf.nextText();
                ret = talkf.getTalk();
                ret = interpreter(ret);
                break;
            case "<show>":
                ret = ret.Replace(pt,"");
                int show_charaid = int.Parse(pattern_trim(ret));
                chara_object[show_charaid].enabled = true;
                talkf.nextText();
                ret = talkf.getTalk();
                ret = interpreter(ret);
                break;
            case "<hide>":
                ret = ret.Replace(pt,"");
                int hide_charaid = int.Parse(pattern_trim(ret));
                chara_object[hide_charaid].enabled = false;
                talkf.nextText();
                ret = talkf.getTalk();
                ret = interpreter(ret);
                break;
            case "<change_back>":
                ret = ret.Replace(pt,"");
                backcount = int.Parse(pattern_trim(ret));
                back.sprite = backs[backcount];
                talkf.nextText();
                ret = talkf.getTalk();
                ret = interpreter(ret);
                break;
            case "<change_scene>":
                ret = ret.Replace(pt,"");
                string sc_to = pattern_trim(ret);
                FadeController.SceneFadeOut(1.0f, sc_to);
                ret = "";
                break;
            case "<change_bgm>":
                AudioSource audioSource = BGMObject.GetComponent<AudioSource>();
                ret = ret.Replace(pt,"");
                int index = int.Parse(pattern_trim(ret));
                audioSource.clip = bgms[index];
                audioSource.volume = 0.3f;
                audioSource.Play();
                talkf.nextText();
                ret = talkf.getTalk();
                ret = interpreter(ret);
                break;
            case "<change_chara>":
                ret = ret.Replace(pt,"");
                string[] indexes = pattern_trim(ret).Split(",");
                int chara_index = int.Parse(indexes[0]);
                chara_object_id[chara_index] = int.Parse(indexes[1]);
                chara_object[chara_index].sprite=chara_sprites[chara_object_id[chara_index]];
                talkf.nextText();
                ret = talkf.getTalk();
                ret = interpreter(ret);
                update();
                break;
            case "<change_uniform>":
                ret = ret.Replace(pt,"");
                int uniformid = int.Parse(pattern_trim(ret));
                chara_sprites[1] = hero_sprites[uniformid];
                talkf.nextText();
                ret = talkf.getTalk();
                ret = interpreter(ret);
                break;
            case "<end>":
                #if UNITY_EDITOR
                    UnityEditor.EditorApplication.isPlaying = false;
                #else
                    Application.Quit();
                #endif
                ret = ret.Replace(pt,"");
                break;
        }
        return ret;
    }

    //文字列から処理パターンを消去
    string pattern_trim(string str){
        string ret = str;
        foreach(string pt in ex_patterns){
            ret = ret.Replace(pt, "");
        }
        return ret.Trim();
    }

    int searchChara(List<string> array, string str){
        int ret = 0;
        foreach(string chara in array){
            if(chara.Equals(str))
                break;
            ret++;
        }
        return ret;
    }



    //選択肢発生処理
    string YesNoChoice(string text){
        string ret = text;
        string[] selects;
        selects = text.Split(",");
        ret = selects[0];
        selects = selects[1].Split("|");
        selection_init(selects[0],selects[1]);
        return ret;
    }

    //フェード処理
    void fadeOutEffect(){
        Debug.Log("fading");
        back_tmp = back.sprite;
        back.sprite = backs[0];
        chara_object[0].color=new Color(255,255,255,0);
        chara_object[1].color=new Color(255,255,255,0);
        isFading=true;
    }

    void fadeInEffect(){
        Debug.Log("fading");
        back.sprite = back_tmp;
        chara_object[0].color=new Color(255,255,255,255);
        chara_object[1].color=new Color(255,255,255,255);
    }

    //ジョイコン関連処理
    void joycon_init(){
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

    //↓↓ボタン入力用メソッド
    public void yes(){
        YesNo=0;
        Debug.Log("yes"+YesNo);
        YesNoPressed=false;
        select_uis.SetActive(false);
        nextText();
    }

    public void no(){
        YesNo=1;
        Debug.Log("no"+YesNo);
        YesNoPressed=false;
        select_uis.SetActive(false);
        nextText();
    }
}
