using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiManager : MonoBehaviour
{



    [SerializeField] private GameObject MapUI;
    [SerializeField] private GameObject TalkUI;
    [SerializeField] private GameObject ChangeSceneUI;
    [SerializeField] private GameObject StartDebug;
    

    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()
    {
       

        if (WorldMapMaster.NowGameState == WorldMapMaster.GameState.Start)
        {
            MapUI.SetActive(false);
            TalkUI.SetActive(false);
            ChangeSceneUI.SetActive(false);
            //StartDebug.SetActive(true);
        }
        else if (WorldMapMaster.NowGameState == WorldMapMaster.GameState.Play)
        {
            MapUI.SetActive(false);
            TalkUI.SetActive(false);
            ChangeSceneUI.SetActive(false);
            //StartDebug.SetActive(false);
        }

        else if (WorldMapMaster.NowGameState == WorldMapMaster.GameState.Map)
        {
            MapUI.SetActive(true);
            TalkUI.SetActive(false);
            ChangeSceneUI.SetActive(false);
            //StartDebug.SetActive(false);
        }
        else if (WorldMapMaster.NowGameState == WorldMapMaster.GameState.Talk)
        {
            MapUI.SetActive(false);
            TalkUI.SetActive(true);
            ChangeSceneUI.SetActive(false);
            //StartDebug.SetActive(false);
        }
        else if (WorldMapMaster.NowGameState == WorldMapMaster.GameState.Attention)
        {
            MapUI.SetActive(false);
            TalkUI.SetActive(false);
            ChangeSceneUI.SetActive(true);
            //StartDebug.SetActive(false);
        }
    }


}
