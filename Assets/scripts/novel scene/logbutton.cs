using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class logbutton : MonoBehaviour
{
    int buttonchange = 0;
    public GameObject log_image, log_text;
    public GameObject ClickEventTrigger;
    public TalkController TalkC;
    private Image log_image2;
    private Text log_text2;
    private Image button_image;
    public Sprite start1, stop1;

    // Start is called before the first frame update
    void Start()
    {
        log_image = GameObject.Find("LogImage");
        log_text = GameObject.Find("LogText");
        log_image2 = log_image.GetComponent<Image>();
        log_text2 = log_text.GetComponent<Text>();
        button_image = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClick()
    {
        if (buttonchange == 0)
        {
            log_image2.enabled=true;
            log_text2.enabled=true;
            TalkC.isLogMenu = true;
            ClickEventTrigger.SetActive(false);
            buttonchange = 1;
            button_image.sprite = start1;
        }
        else if (buttonchange == 1)
        {
            log_image2.enabled=false;
            log_text2.enabled=false;
            TalkC.isLogMenu = false;
            ClickEventTrigger.SetActive(true);
            buttonchange = 0;
            button_image.sprite = stop1;
        }

    }
}
