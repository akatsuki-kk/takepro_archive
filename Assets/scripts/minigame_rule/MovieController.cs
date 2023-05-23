using System;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.Video;

public class MovieController : MonoBehaviour
{
    public GameObject description_rule;
    public GameObject description_ctrl;
    public GameObject Movie_obj;
    public GameObject Startbutton;

    //only debug mode
    // public GameObject joyconM;

    public Image ctrlobj;
    public Image ruleobj;
    Text description_rule_ui;
    Text description_ctrl_ui;
    VideoPlayer vid;

    public Image back_ui;
    public Sprite[] backs = new Sprite[2];
    public Sprite[] rules = new Sprite[2];
    public Sprite[] ctrls = new Sprite[2];

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

    // Start is called before the first frame update
    void Start()
    {
      joycon_init();
      Debug.Log(Global.minigameID);
      ctrlobj.sprite = ctrls[Global.minigameID];
      ruleobj.sprite = rules[Global.minigameID];

      description_rule_ui = description_rule.GetComponent<Text>();
      description_ctrl_ui = description_ctrl.GetComponent<Text>();
      vid = Movie_obj.GetComponent<VideoPlayer>();
      String[] config_names = {"game01","game02"};
      String config_name;
      
      try{
        config_name = config_names[Global.minigameID];
        back_ui.sprite = backs[Global.minigameID];
        
      }catch(IndexOutOfRangeException e){
        config_name = "sample";
        back_ui.sprite = backs[0];
      }
      var config = Resources.Load("datas/"+config_name) as TextAsset;
      Debug.Log(config.text);
      string[] configs = config.text.Split("\n");
      String movie_name = configs[1].Trim();
      Global.rule_description = configs[2];
      Global.ctrl_description = configs[3];
      Global.rule_description = Global.rule_description.Replace("<br>","\n");
      Global.ctrl_description = Global.ctrl_description.Replace("<br>","\n");
      description_rule_ui.text=Global.rule_description;
      description_ctrl_ui.text=Global.ctrl_description;
      vid.source = VideoSource.VideoClip;
      vid.clip = Resources.Load("Movies/"+movie_name) as VideoClip;
      vid.Play();
    }

    // Update is called once per frame
    void Update()
    {
      try{
        if(m_joyconR.GetButtonDown(Joycon.Button.DPAD_RIGHT)){
          Startbutton.GetComponent<StartButton>().OnClick();
        }
      }catch(System.NullReferenceException e){}
    }

    void joycon_init(){
        m_joycons = JoyconManager.Instance.j;
        if (m_joycons == null || m_joycons.Count <= 0) return;

        m_joyconL = m_joycons.Find(c => c.isLeft);
        m_joyconR = m_joycons.Find(c => !c.isLeft);
    }
}
