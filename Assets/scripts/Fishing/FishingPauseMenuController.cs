using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishingPauseMenuController : MonoBehaviour
{
    public FishPauseButtonController FPBC;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void returnGame(){
        FPBC.PauseButton();
    }

    public void restart(){
        FadeController.SceneFadeOut(1.0f,"Fishing");
    }

    public void back_menu(){
        FadeController.SceneFadeOut(1.0f,"Minigame_rule_placeholder");
    }
}
