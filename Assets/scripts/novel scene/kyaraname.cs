using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class kyaraname : MonoBehaviour
{

    public GameObject kyara_object = null;
    int kyaratest = 0;

    // Start is called before the first frame update
    void Start()
    {
        Text kyara = kyara_object.GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {

        KyaraChat();
    }

    void KyaraChat()
    {
        Text kyara = kyara_object.GetComponent<Text>();

        if (Input.GetMouseButtonUp(0))
        {
            kyaratest++;
        }

        switch (kyaratest)
        {
            case 0:
                kyara.text = "主人公";
                break;

            case 1:
                kyara.text = "ナビメット";
                break;

            case 2:
                kyara.text = "主人公";
                break;

            case 3:
                kyara.text = "主人公";
                break;

            case 4:
                kyara.text = "";
                break;

            case 5:
                kyara.text = "主人公";
                break;

            case 6:
                kyara.text = "主人公";
                break;

            case 7:
                kyara.text = "ナビメット";
                break;

            case 8:
                kyara.text = "主人公";
                break;

            case 9:
                kyara.text = "ナビメット";
                break;

            case 10:
                kyara.text = "主人公";
                break;

            case 11:
                kyara.text = "ナビメット";
                break;

            case 12:
                kyara.text = "主人公";
                break;

            case 13:
                kyara.text = "ナビメット";
                break;

            case 14:
                kyara.text = "主人公";
                break;

            case 15:
                kyara.text = "ナビメット";
                break;

            case 16:
                kyara.text = "主人公";
                break;

            case 17:
                kyara.text = "ナビメット";
                break;

            case 18:
                kyara.text = "主人公";
                break;

            case 19:
                kyara.text = "ナビメット";
                break;

            case 20:
                kyara.text = "ナビメット";
                break;
        }
    }
}
