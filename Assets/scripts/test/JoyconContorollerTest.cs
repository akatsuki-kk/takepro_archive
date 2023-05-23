using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class JoyconContorollerTest : MonoBehaviour
{

    bool chance = true;

    bool kakusokudo = false;
    bool kasokudo = false;
    Pulse[] ShakeStats = new Pulse[6];
    public static readonly Joycon.Button[] m_buttons =
        Enum.GetValues(typeof(Joycon.Button)) as Joycon.Button[];

    private List<Joycon> m_joycons;
    private Joycon m_joyconL;

    public Joycon.Button? m_pressedButtonL;
    //判定用のオブジェクト
    public Joycube joycube;


    Quaternion orientation;
    Vector3 accel;
    Vector3 gyro;
    Quaternion katamuki;
    Quaternion Cubeq;
    



    private void Start()
    {
        m_joycons = JoyconManager.Instance.j;
        for(int i=0; i<6;i++){
            ShakeStats[i] = new Pulse();
        }
        if (m_joycons == null || m_joycons.Count <= 0) return;

        m_joyconL = m_joycons.Find(c => c.isLeft);

    }

    private void Update()
    {
        // chance = gameMaster02.chance;
        m_pressedButtonL = null;
        Cubeq = joycube.getJoycubeRot();



        if (m_joycons == null || m_joycons.Count <= 0) return;

        foreach (var button in m_buttons)
        {
            if (m_joyconL.GetButton(button))
            {
                m_pressedButtonL = button;
            }

        }

        if (Input.GetKeyDown(KeyCode.Z))

            m_joyconL.SetRumble(160, 320, 0.6f, 200);

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



        GUILayout.BeginHorizontal(GUILayout.Width(960));

        foreach (var joycon in m_joycons)
        {

            var isLeft = joycon.isLeft;
            var name = "Joy-Con (L)";
            var key = "Z キー";
            var button = m_pressedButtonL; var stick = joycon.GetStick();
            gyro = joycon.GetGyro();
            accel = joycon.GetAccel();
            orientation = joycon.GetVector();
            katamuki = new Quaternion(orientation.x, orientation.y, orientation.z, orientation.w);
            for(int i=0; i< 6; i++){
                ShakeStats[i].update(Shaked(gyro, accel, Cubeq) == i+1);
            }
            string[] logs = {"shaked_upleft", "shaked_left", "shaked_downleft", "shaked_upright", "shaked_right", "shaked_downright"};
            
            for(int i=0; i<6; i++){
                if(ShakeStats[i].stat()){
                    Debug.Log(logs[i]);
                }
            }

            // if(Shaked(gyro,accel)&1==1){
            //
            // }else if(Shaked())

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

        GUILayout.EndHorizontal();
    }

    public int Shaked(Vector3 gyro, Vector3 accel, Quaternion q)
    {
        //振った向きを6方向で返す。(1=左上、2=左、3=左下、4=右上、5=右、6=右下)
        int ret = 0;
        // if (accel.y>=2){shaked_x=true;}
        if (gyro.z <= -5)
        {
            if(q.z >= 30){
                ret = 3;
            }else if (q.z <= -30){
                ret = 2;
            }else{
                ret = 1;
            }
        }
        else if (gyro.z >= 5)
        {
            if(q.z >= 30){
                ret = 4;
            }else if(q.z <= -30){
                ret = 6;
            }else{
                ret = 5;
            }
            
        }
        
        return ret;
    }

    public Vector3 getAccel()
    {
        return accel;
    }

    public Quaternion getOrientation()
    {
        return orientation;
    }

    public Vector3 getGyro()
    {
        return gyro;
    }

    public Quaternion getKatamuki()
    {
        return katamuki;
    }

    public Joycon.Button? getButton()
    {
        return m_pressedButtonL;
    }

    public Joycon.Button[] getButtons(){
        return m_buttons;
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
