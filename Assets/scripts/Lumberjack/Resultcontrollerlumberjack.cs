using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resultcontrollerlumberjack : MonoBehaviour
{
    public bool resultEnd;
    bool runfirst = false;
    public GameMaster02 gm2;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (resultEnd)
        {
            if (!runfirst)
            {
                //fishC.SESource.Stop();
                //fishC.SESource.PlayOneShot(fishC.SoundEffects[7]);
                gm2.se.Stop();
                gm2.se.PlayOneShot(gm2.resultAudio);
                runfirst = true;

            }
        }
    }
}

