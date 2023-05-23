using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Joycube : MonoBehaviour
{
    public JoyconContorollerTest joycon_cont;
    Vector3 gyro;
    Vector3 accel;
    Quaternion katamuki;
    Quaternion orientation;
    Joycon.Button? m_pressedButtonL;

    public Quaternion memoried_katamuki;
    public GameObject Cube;
    // Start is called before the first frame update
    void Start()
    {
        setPlaceInit(joycon_cont);
    }

    // Update is called once per frame
    void Update()
    {
        gyro = joycon_cont.getGyro();
        accel = joycon_cont.getAccel();
        katamuki = joycon_cont.getKatamuki();
        m_pressedButtonL = joycon_cont.getButton();
        // Cube.transform.rotation = getKatamuki(joycon_cont);
        Vector3 s = Cube.transform.position;
        Quaternion b = Cube.transform.rotation;
        // b.x += -gyro.y * 0.01f;
        // b.y -= -gyro.z * 0.01f;
        b.z += -gyro.x * 0.01f;
        s.x -= -gyro.z * 0.1f;
        // s.y -= -gyro.y * 0.1f;
        Cube.transform.rotation = b;
        Cube.transform.position = s;
        if (Input.GetKeyDown(KeyCode.C))
        {
            Cube.transform.position = new Vector3(0,0,0);
            Cube.transform.rotation = new Quaternion(0,0,0,0);
        }
        // Cube.transform.position = new Vector3(gyro.x, gyro.y, 0.0f);
    }

    Quaternion getKatamuki(JoyconContorollerTest t){
        Quaternion k;
        Quaternion m = memoried_katamuki;
        Quaternion r;
        k = t.getKatamuki();
        r = new Quaternion(k.x, k.y, k.z, k.w);
        r = r * Quaternion.AngleAxis(90.0f, new Vector3(1.0f,0.0f,0.0f));
        Debug.Log(r.ToString());
        return r;
    }

    void setPlaceInit(JoyconContorollerTest t)
    {
        memoried_katamuki = t.getKatamuki();
    }

    float convert_r(float t)
    {
        float r;
        if (t >= 1.0f)
        {
            r = 1.0f - Math.Abs(t - 1.0f);
        }
        else if(t<=-1.0)
        {
            r = -1.0f + Math.Abs(t + 1.0f);
        }else{
            r = t;
        }
        return r;
    }
    
    public Quaternion getJoycubeRot()
    {
        return Cube.transform.rotation;
    }

}
