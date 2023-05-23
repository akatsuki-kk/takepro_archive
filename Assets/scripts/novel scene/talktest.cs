using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class talktest : MonoBehaviour
{
    public GameObject text_object = null;
    public int test = 0;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        ChatText();
    }

    void ChatText()
    {
        Text talk = text_object.GetComponent<Text>();

        if (Input.GetMouseButtonUp(0))
        {
            test++;
        }

        switch (test)
        {
            case 0:
                talk.text = "「君たちの星が危ないって...どういうこと？」";
                break;

            case 1:
                talk.text = "「とにかく大変なんだメット！ついてきて欲しいんだメット〜！！！」";
                break;

            case 2:
                talk.text = "「わ！ちょっと引っ張らないで！」";
                break;

        }
    }
}