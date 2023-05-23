using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class JoyconGlobalController : MonoBehaviour
{
    public readonly Joycon.Button[] m_buttons =
        Enum.GetValues(typeof(Joycon.Button)) as Joycon.Button[];

    public List<Joycon> m_joycons;
    public Joycon m_joyconL;
    public Joycon m_joyconR;
    public Joycon.Button? m_pressedButtonL;
    public Joycon.Button? m_pressedButtonR;
    public Quaternion orientation;
    public Vector3 accel;
    public bool ConnectingMode=false;
    public bool L=false;
    public bool R=false;

    // Start is called before the first frame update
    void Start()
    {
        m_joycons = JoyconManager.Instance.j;

        if (m_joycons == null || m_joycons.Count <= 0) return;

        m_joyconL = m_joycons.Find(c => c.isLeft);
        m_joyconR = m_joycons.Find(c => !c.isLeft);
        if (m_joycons.Any(c => c.isLeft)){
            L = true;
        }
        if (m_joycons.Any(c => !c.isLeft)){
            R = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
    }


    public bool chkJoyconConnect()
    {
        if(L && R){
            return true;
        }else{
            return false;
        }
    }


}
