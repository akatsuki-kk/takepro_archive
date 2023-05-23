using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartButton : MonoBehaviour
{
  public GameObject fade;
  public FadeController fadeC;
  string[] scenes = {"Lumberjack", "Fishing", ""};
    // Start is called before the first frame update
    void Start()
    {
      

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnClick(){
      FadeController.SceneFadeOut(1.0f,scenes[Global.minigameID]);
      //while(fadeC.isFadeOut){}
    }
}
