using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultController : MonoBehaviour
{
    public bool resultEnd;
    bool runfirst=false;
    public FishingController fishC;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(resultEnd){
            if(!runfirst){
                fishC.SESource.Stop();
                fishC.SESource.PlayOneShot(fishC.SoundEffects[7]);
                runfirst = true;
            }
        }
    }
}
