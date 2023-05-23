using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseAnimController : MonoBehaviour
{
    public FishingController FC;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        GetComponent<Animator>().SetInteger("nowGamestate", FC.NowGameState);
    }
}
