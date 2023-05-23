using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Example : MonoBehaviour
{

    bool chance = true;
    public GameMaster02 gameMaster02;

    bool kakusokudo = false;
    bool kasokudo = false;
    Pulse left = new Pulse();
    Pulse right = new Pulse();
    private static readonly Joycon.Button[] m_buttons =
        Enum.GetValues(typeof(Joycon.Button)) as Joycon.Button[];

    private List<Joycon> m_joycons;
    private Joycon m_joyconL;
    // private Joycon m_joyconR;
    private Joycon.Button? m_pressedButtonL;
    // private Joycon.Button? m_pressedButtonR;

    Quaternion orientation;
    Vector3 accel;


    private void Start()
    {
        m_joycons = JoyconManager.Instance.j;

        if (m_joycons == null || m_joycons.Count <= 0) return;

        m_joyconL = m_joycons.Find(c => c.isLeft);
        // m_joyconR = m_joycons.Find(c => !c.isLeft);
    }

    private void Update()
    {
        chance = gameMaster02.chance;
        m_pressedButtonL = null;
        // m_pressedButtonR = null;

        // orientation = m_joyconR.GetVector();
        // accel = m_joyconR.GetAccel();



        if (m_joycons == null || m_joycons.Count <= 0) return;

        foreach (var button in m_buttons)
        {
            if (m_joyconL.GetButton(button))
            {
                m_pressedButtonL = button;
            }
            // if (m_joyconR.GetButton(button))
            // {
            //     m_pressedButtonR = button;
            // }
        }

        if (Input.GetKeyDown(KeyCode.Z))
            // {
            m_joyconL.SetRumble(160, 320, 0.6f, 200);
        // }
        // if (Input.GetKeyDown(KeyCode.X))
        // {
        //     m_joyconR.SetRumble(160, 320, 0.6f, 200);
        // }

        // if (m_pressedButtonR == Joycon.Button.SHOULDER_2)
        // {
        //     Debug.Log(orientation.eulerAngles);
        //     transform.GetComponent<Rigidbody2D>().AddForce(accel * 10f);
        // }
        //
        // if (m_pressedButtonR == Joycon.Button.SHOULDER_1)
        // {
        //     //Debug.Log(orientation.eulerAngles);
        //     transform.position = new Vector3(7, 0, 0);
        // }


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

        // if (!m_joycons.Any(c => !c.isLeft))
        // {
        //     GUILayout.Label("Joy-Con (R) が接続されていません");
        //     return;
        // }

        GUILayout.BeginHorizontal(GUILayout.Width(960));

        foreach (var joycon in m_joycons)
        {
            // var isLeft = joycon.isLeft;
            // var name = isLeft ? "Joy-Con (L)" : "Joy-Con (R)";
            // var key = isLeft ? "Z キー" : "X キー";
            // var button = isLeft ? m_pressedButtonL : m_pressedButtonR;
            var isLeft = joycon.isLeft;
            var name = "Joy-Con (L)";
            var key = "Z キー";
            var button = m_pressedButtonL; var stick = joycon.GetStick();
            var gyro = joycon.GetGyro();
            var accel = joycon.GetAccel();
            var orientation = joycon.GetVector();
            var katamuki = new Quaternion(orientation.z, orientation.y, orientation.x, orientation.w);
            left.update(And(Shaked(gyro, accel), 1));
            right.update(And(Shaked(gyro, accel), 2));
            if (left.stat())　//ジョイコンでの木の切断
            {
                if (katamuki.x <= -0.5)
                {
                    Debug.Log("shaked_downleft");
                }
                else if (katamuki.x >= 0.5)
                {
                    Debug.Log("shaked_upleft");
                }
                else
                {
                    Debug.Log("shaked_left");
                }

                //if (chance == false)
                //{
                //    Debug.Log("ワンモア");
                //    gameMaster02.PerfectReward = false;

                //}
                //else if (chance == true && gameMaster02.Cut == false)
                //{
                //    gameMaster02.nextTree();
                //    gameMaster02.Cut = true;

                //}
            }
            else if (right.stat())
            {
                if (katamuki.x <= -0.5)
                {
                    Debug.Log("shaked_upright");
                }
                else if (katamuki.x >= 0.5)
                {
                    Debug.Log("shaked_downright");
                }
                else
                {
                    Debug.Log("shaked_right");
                }

                //if (chance == false)
                //{
                //    Debug.Log("ワンモア");
                //    gameMaster02.PerfectReward = false;

                //}
                //else if (chance == true && gameMaster02.Cut == false)
                //{
                //    gameMaster02.nextTree();
                //    gameMaster02.Cut = true;

                //}
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

    public int Shaked(Vector3 gyro, Vector3 accel)
    {
        //加速度をビット列で返す。左：1、右：2、上：4,下：8として返す。
        bool shaked_x = false;
        bool shaked_y = false;
        int ret = 0;
        shaked_x = true;
        // if (accel.y>=2){shaked_x=true;}
        if (gyro.z <= -5 && shaked_x)
        {
            ret += 1;
        }
        if (gyro.z >= 5 && shaked_x)
        {
            ret += 2;
        }
        if (gyro.y >= 3)
        {
            ret += 4;
        }
        if (gyro.y <= -3)
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
