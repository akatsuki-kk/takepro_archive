using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// テキストデータ作成
// <br>:改行
// <yn>2つの選択肢がある文章。1つ目の選択肢と2つ目の選択肢を|（絶対値）で区切る。
// <y>1つ目の選択肢を選んだ後の文章
// <n>2つ目の選択肢を選んだ後の文章

public class TalkFromFile : MonoBehaviour
{
    List<string> charactors;
    List<string[]> talkdatas;
    string current_chara;
    string current_talk;
    int index;
    string filename;
    public TalkFromFile(string fname)
    {
        charactors = new List<string>();
        talkdatas = new List<string[]>();
        filename = fname;
        readText(filename);
        current_talk=talkdatas[0][1];
        current_chara=talkdatas[0][0].Trim();
    }

    void start()
    {
        
    }

    void readText(string fname)
    {
        var talkrowdata = Resources.Load("datas/"+fname) as TextAsset;
        var chararowdata = Resources.Load("datas/charactor") as TextAsset;
        string[] talkdata = talkrowdata.text.Split("\n");
        string[] charadata = chararowdata.text.Split("\n");
        foreach(string chara in charadata){
            charactors.Add(chara.Trim());
        }
        int i=0;
        foreach(string p in talkdata)
        {
            string[] talk= new string[2];
            string pp = p.Trim();
            if(pp==""){break;}
            Debug.Log("i:"+i+" "+pp);
            int n;
            int.TryParse(p.Split(":")[0], out n);
            talk[0]=charadata[n];
            talk[1]=p.Split(":")[1];
            talk[1]=talk[1].Replace("<br>","\n");
            talkdatas.Add(talk);
            
            i+=1;
        }
    }

    public void nextText(){
        index += 1;
        try{
            current_chara=talkdatas[index][0].Trim();
            current_talk=talkdatas[index][1];
        }catch(System.IndexOutOfRangeException e){
            current_chara="";
            current_talk="";
        }catch(System.ArgumentOutOfRangeException e){
            current_chara="";
            current_talk="";
        }
        
    }

    public string getChara(){
        return current_chara;
    }

    public string getTalk(){
        return current_talk;
    }

    public List<string> getCharas(){
        return charactors;
    }

    public int getIndex(){
        return index;
    }
}
