using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OPback : MonoBehaviour
{
    int kyaracolor;
    public Image hero;
    public Sprite hero_space;

    // Start is called before the first frame update
    void Start()
    {
        //back = this.GetComponent<Image>();
        //mettocity = this.GetComponent<Sprite>();
        //city = this.GetComponent<Sprite>();
        //black = this.GetComponent<Sprite>();

    }

    // Update is called once per frame
    void Update()
    {
        Kyarargb();
    }

    void Kyarargb()
    {
        //back = this.GetComponent<Image>();
        //mettocity = this.GetComponent<Sprite>();
        //city = this.GetComponent<Sprite>();
        //black = this.GetComponent<Sprite>();・

        if (Input.GetMouseButtonUp(0))
        {
            kyaracolor++;
        }

        switch (kyaracolor)
        {
            case 30:
                hero.sprite = hero_space;
                break;
        }
    }
}
