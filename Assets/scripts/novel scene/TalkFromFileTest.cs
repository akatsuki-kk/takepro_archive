using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// テキストデータ作成
// <br>:改行
// <yn>2つの選択肢がある文章。1つ目の選択肢と2つ目の選択肢を|（絶対値）で区切る。
// <y>1つ目の選択肢を選んだ後の文章
// <n>2つ目の選択肢を選んだ後の文章

public class TalkFromFileTest : MonoBehaviour
{
    public GameObject talkui;
    public GameObject nameui;
    string[,] talksdata;
    Text talka;
    Text namea;
    int index;

    // Start is called before the first frame update
    void Start()
    {
        talksdata = readText("talk");
        talka = talkui.GetComponent<Text>();
        namea = nameui.GetComponent<Text>();
        index = 0;
        talka.text=talksdata[0,1];
        namea.text=talksdata[0,0];
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public string[,] readText(string fname)
    {
        var talkrowdata = Resources.Load("datas/"+fname) as TextAsset;
        var chararowdata = Resources.Load("datas/"+fname+"_c") as TextAsset;
        string[] talkdata = talkrowdata.text.Split("\n");
        string[] charadata = chararowdata.text.Split("\n");
        string[,] talks = new string[talkdata.Length,2];
        int i=0;
        foreach(string p in talkdata)
        {
            string pp = p.Trim();
            if(pp==""){break;}
            Debug.Log("i:"+i+" "+pp);
            int n;
            int.TryParse(p.Split(":")[0], out n);
            talks[i,0]=charadata[n];
            talks[i,1]=p.Split(":")[1];
            talks[i,1]=talks[i,1].Replace("<br>","\n");
            
            i+=1;
        }
        return talks;

    }

    public void nextText(){
        index += 1;
        try{
            talka.text=talksdata[index,1];
            namea.text=talksdata[index,0];
        }catch(System.IndexOutOfRangeException e){

        }
        
    }
}
