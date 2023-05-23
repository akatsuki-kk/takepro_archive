using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SousaButton : MonoBehaviour
{
    public GameObject DescImg;
    public Text DescTxt;
    public Image DescImage;
    public Sprite ctrl;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClick()
    {
        DescImage = DescImg.GetComponent<Image>();
        DescImage.sprite=ctrl;
        DescTxt.text = Global.ctrl_description;
    }
}
