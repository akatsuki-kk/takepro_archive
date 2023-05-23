using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FishPauseButtonController : MonoBehaviour
{
    bool isPause = false;
    public FishingController FC;
    public TimeController timeC;
    public GameObject PauseGroup;
    public Sprite[] PauseImage = new Sprite[2];
    int before_state;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void PauseButton(){
        if(isPause){
            isPause=false;
            GetComponent<Image>().sprite = PauseImage[0];
            FC.showSprites();
            FC.NowGameState = before_state;
            FC.SESource.PlayOneShot(FC.SoundEffects[1]);
            PauseGroup.SetActive(false);
            timeC.move();
        }else{
            isPause=true;
            GetComponent<Image>().sprite = PauseImage[1];
            before_state = FC.NowGameState;
            FC.hideSprites();
            FC.NowGameState = 10;
            FC.select_num = 0;
            FC.SESource.PlayOneShot(FC.SoundEffects[6]);
            PauseGroup.SetActive(true);
            timeC.stop();
        }
    }
}
