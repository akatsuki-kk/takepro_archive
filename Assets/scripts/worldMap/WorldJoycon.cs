using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldJoycon : MonoBehaviour
{

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
        joycon_setup();
    }

    // Update is called once per frame
    void Update()
    {
        if (m_joyconR.GetButton(Joycon.Button.DPAD_RIGHT))
        {
            Debug.Log("家ーい");
        }
    }

    void joycon_setup()
    {
        m_joycons = JoyconManager.Instance.j;
        if (m_joycons == null || m_joycons.Count <= 0) return;

        m_joyconL = m_joycons.Find(c => c.isLeft);
        m_joyconR = m_joycons.Find(c => !c.isLeft);
    }

    void joycon_update()
    {
        m_pressedButtonL = null;
        m_pressedButtonR = null;

        if (m_joycons == null || m_joycons.Count <= 0) return;

        foreach (var button in m_buttons)
        {
            //左ジョイコンが接続されていなければ例外処理
            try
            {
                if (m_joyconL.GetButton(button))
                {
                    m_pressedButtonL = button;
                }
            }
            catch (System.NullReferenceException e) { }
            //右ジョイコンが接続されていなければ例外処理
            try
            {
                if (m_joyconR.GetButton(button))
                {
                    m_pressedButtonR = button;
                }
            }
            catch (System.NullReferenceException e) { }

        }
    }

    void pause()
    {
        try
        {
  

        }
        catch (System.NullReferenceException e) { }
    }
}
