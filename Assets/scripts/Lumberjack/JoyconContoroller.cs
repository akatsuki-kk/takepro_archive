using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class JoyconContoroller : MonoBehaviour
{

    bool chance = true;
    public GameMaster02 gameMaster02;
    public AudioSource se;
    bool kakusokudo = false;
    bool kasokudo = false;
    Pulse[] left = new Pulse[2];
    Pulse[] right = new Pulse[2];
    private static readonly Joycon.Button[] m_buttons =
        Enum.GetValues(typeof(Joycon.Button)) as Joycon.Button[];

    private List<Joycon> m_joycons;
    public Joycon m_joyconL;
    public Joycon m_joyconR;

    private Joycon.Button? m_pressedButtonL;
    private Joycon.Button? m_pressedButtonR;

    Quaternion orientation;
    Vector3 accel;




    private void Start()
    {
        m_joycons = JoyconManager.Instance.j;
        for(var i =0; i<2; i++){
            left[i] = new Pulse();
            right[i] = new Pulse();
        }

        if (m_joycons == null || m_joycons.Count <= 0) return;

        m_joyconL = m_joycons.Find(c => c.isLeft);
        m_joyconR = m_joycons.Find(c => !c.isLeft);
    }

    private void Update()
    {
        chance = gameMaster02.chance;
        m_pressedButtonL = null;
        m_pressedButtonR = null;

        if (m_joycons == null || m_joycons.Count <= 0) return;

        foreach (var button in m_buttons)
        {
            
            if (m_joyconL.GetButton(button))
            {
                m_pressedButtonL = button;
            }
            //右ジョイコンが接続されていなければ例外処理
            try{
                if (m_joyconR.GetButton(button))
                {
                    m_pressedButtonR = button;
                }
            }catch(System.NullReferenceException e){}
            
        }

        if (Input.GetKeyDown(KeyCode.Z))
            m_joyconL.SetRumble(160, 320, 0.6f, 200);
        //右ジョイコンが接続されていなければ例外処理
        try{
            if (Input.GetKeyDown(KeyCode.X))
                m_joyconR.SetRumble(160, 320, 0.6f, 200);
        }catch(System.NullReferenceException e){}
        

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
        if(!gameMaster02.debug){
            if (!m_joycons.Any(c => !c.isLeft))
            {
                GUILayout.Label("Joy-con (R) が接続されていません");
                return;
            }
        }
        GUILayout.BeginHorizontal(GUILayout.Width(960));
        int j = 0;
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
            left[j].update(And(Shaked(gyro, accel), 1));
            right[j].update(And(Shaked(gyro, accel), 2));
            //ジョイコンでの木の切断
            if(isLeft){
                ChkJoyconShaked(katamuki, j);
            }
            // if(Shaked(gyro,accel)&1==1){
            //
            // }else if(Shaked())
            if(gameMaster02.debug){
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
            j++;
        }

        GUILayout.EndHorizontal();
    }

    void ChkJoyconShaked(Quaternion katamuki, int j){
        if (left[j].stat())//ジョイコンでの木の切断
        {
            //ジョイコンが表の状態
            if(katamuki.y <= 0.5 && katamuki.y >= -0.5)
            {
                if (gameMaster02.Course_number == 2 || gameMaster02.Course_number == 3 || gameMaster02.Course_number == 4)
                {
                    //Debug.Log("shaked_left");
                    //音を鳴らす
                    se.PlayOneShot(gameMaster02.swingAudio);
                    if (chance == false)
                    {
                        //Debug.Log("ワンモア");
                        gameMaster02.PerfectReward = false;

                    }
                    else if (chance == true && gameMaster02.Cut == false)
                    {
                        gameMaster02.nextTree();
                        gameMaster02.Cut = true;
                        m_joyconL.SetRumble(160, 320, 10f, 200);

                    }
                }
            }
            //ジョイコンが裏の状態
            else
            {
                if (gameMaster02.Course_number == 5 || gameMaster02.Course_number == 0 || gameMaster02.Course_number == 1)
                {
                    se.PlayOneShot(gameMaster02.swingAudio);
                    if (chance == false)
                    {
                       
                        gameMaster02.PerfectReward = false;

                    }
                    else if (chance == true && gameMaster02.Cut == false)
                    {
                        gameMaster02.nextTree();
                        gameMaster02.Cut = true;
                        m_joyconL.SetRumble(160, 320, 10f, 200);

                    }
                }
            }
            
        }
        else if (right[j].stat())
        {
            if(katamuki.y <=0.5 && katamuki.y >= -0.5){
                if (gameMaster02.Course_number == 5 || gameMaster02.Course_number == 0 || gameMaster02.Course_number == 1)
                {
                    se.PlayOneShot(gameMaster02.swingAudio);
                    if (chance == false)
                    {
                        
                        gameMaster02.PerfectReward = false;

                    }
                    else if (chance == true && gameMaster02.Cut == false)
                    {
                        gameMaster02.nextTree();
                        gameMaster02.Cut = true;
                        m_joyconL.SetRumble(160, 320, 10f, 200);

                    }
                }
            }else{
                if (gameMaster02.Course_number == 2 || gameMaster02.Course_number == 3 || gameMaster02.Course_number == 4)
                {
                    se.PlayOneShot(gameMaster02.swingAudio);
                    if (chance == false)
                    {
                        
                        gameMaster02.PerfectReward = false;

                    }
                    else if (chance == true && gameMaster02.Cut == false)
                    {
                        gameMaster02.nextTree();
                        gameMaster02.Cut = true;
                        m_joyconL.SetRumble(160, 320, 10f, 200);

                    }
                }
            }
            
        }
    }

    public int Shaked(Vector3 gyro, Vector3 accel)
    {
        //加速度をビット列で返す。左：1、右：2、上：4,下：8として返す。
        bool shaked_x = false;
        bool shaked_y = false;
        int ret = 0;
        if (Math.Abs(accel.x)>=2){shaked_x=true;}
        if (Math.Abs(accel.y)>=2){shaked_y=true;}
        if (gyro.z <= -5 && shaked_x)
        {
            ret += 1;
        }
        if (gyro.z >= 5 && shaked_x)
        {
            ret += 2;
        }
        if (gyro.y >= 3 && shaked_y)
        {
            ret += 4;
        }
        if (gyro.y <= -3 && shaked_y)
        {
            ret += 8;
        }
        return ret;
    }

    public bool And(int shake_ret, int vec)
    {
        bool ret = false;
        if ((shake_ret & vec) == vec)
        {
            ret = true;
        }
        return ret;
    }

    public class Pulse
    {
        private bool state;
        private bool sw;
        public Pulse()
        {
            this.state = false;
            this.sw = false;
        }
        public void update(bool state)
        {
            if (state)
            {
                if (!this.sw)
                {
                    this.state = true;
                    this.sw = true;
                }
                else
                {
                    this.state = false;
                }
            }
            else
            {
                if (this.sw)
                {
                    this.sw = false;
                    Debug.Log("");
                }
            }
        }
        public bool stat()
        {
            return this.state;
        }
    }


}
