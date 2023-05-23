using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TalkUIController : MonoBehaviour
{
    [SerializeField]
    private Text textLabel;

    [SerializeField]
    private Text characterNameLabel;

    public TextAsset textFile;


    private string textData;
    private string[] splitText;

    // 改良
    private int currentNum = 0;

    void Start()
    {
        //デバック中
        InitializeTalk();
    }

    // 改良
    // スペースキーを押すたびごとに表示される文字列を切り替える。

    public void InitializeTalk()
    {
        currentNum = 0;
        textData = textFile.text;
        splitText = textData.Split(char.Parse("\n"));
        //最初にキャラ名を読み込む 最初の行が名前
        characterNameLabel.text = splitText[currentNum];
        currentNum += 1;


        textLabel.text = splitText[currentNum];
        currentNum += 1;
    }

    public void Message()
    {
        if (WorldMapMaster.NowGameState == WorldMapMaster.GameState.Talk)
        {

            if (currentNum != splitText.Length)
            {
                    textLabel.text = splitText[currentNum];
                    currentNum += 1;
            }
            else
            {
                WorldMapMaster.NowGameState = WorldMapMaster.GameState.Play;
            }
        }
    }

    private void Update()
    {
 
        
    }
}
