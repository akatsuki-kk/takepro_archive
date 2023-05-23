using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RumbleController : MonoBehaviour
{
    public FishingController FishingC;
    //振動をアニメーションから制御する
    public bool Rumble1;
        bool firstcall;
    public bool Rumble2;
    public bool isLeft;
    public int rumbletime = 1; 


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //単発振動
        if(Rumble1){
            if(!firstcall){
                if(isLeft){
                    try{
                        FishingC.m_joyconL.SetRumble(160, 320, 0.6f, 5);
                    }catch(System.NullReferenceException e){}
                }else{
                    try{
                        FishingC.m_joyconR.SetRumble(160, 320, 0.6f, 5);
                    }catch(System.NullReferenceException e){}
                }
                firstcall = true;
            }
        }else{
            firstcall = false;
        }

        //連続振動
        if(Rumble2){
            if(isLeft){
                try{
                    FishingC.m_joyconL.SetRumble(160, 320, 0.6f, 5);
                }catch(System.NullReferenceException e){}
            }else{
                try{
                    FishingC.m_joyconR.SetRumble(160, 320, 0.6f, 5);
                }catch(System.NullReferenceException e){}
            }
        }
    }
}
