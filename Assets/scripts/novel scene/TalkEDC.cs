using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class TalkEDC : MonoBehaviour
{
    public GameObject text_object = null;
    public GameObject name_object = null;
    TalkFromFile talktest;

    // Start is called before the first frame update
    void Start()
    {
        Text talk = text_object.GetComponent<Text>();
        Text name = name_object.GetComponent<Text>();
        talktest = new TalkFromFile("metcityendc");
        talk.text = talktest.getTalk();
        name.text = talktest.getChara();

    }

    // Update is called once per frame
    void Update()
    {
        ChatTextOP();
    }

    void ChatTextOP()
    {
        Text talk = text_object.GetComponent<Text>();
        Text name = name_object.GetComponent<Text>();

        if (Input.GetMouseButtonUp(0))
        {
            talktest.nextText();
            talk.text = talktest.getTalk();
            name.text = talktest.getChara();
        }
    }

}
