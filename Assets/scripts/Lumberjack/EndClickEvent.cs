using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndClickEvent : MonoBehaviour
{
    public int typeN;
    public GameMaster02 master;
    // Start is called before the first frame update
    void Start()
    {
        typeN=0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClick()
    {
        if(typeN==0)
        {
            GameObject TimeUp = master.End.transform.Find("TimeUp").gameObject;
            TimeUp.SetActive(false);
            typeN=1;
            if(master.ScoreRate<1){
                Global.ED=0;
            }else if(master.WoodCnt<master.n){
                Global.ED=1;
            }else{
                Global.ED=2;
            }

        }else if(typeN==1){
            FadeController.SceneFadeOut(1.0f,"ED");
        }
    }
}
