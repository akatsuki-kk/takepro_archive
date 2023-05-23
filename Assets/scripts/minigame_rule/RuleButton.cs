using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RuleButton : MonoBehaviour
{
    public GameObject DescImg;
    public Text DescTxt;
    public Image DescImage;
    public Sprite rule;
    
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
        DescImage.sprite=rule;
        DescTxt.text = Global.rule_description;
    }
}
