using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class kyaralighteda : MonoBehaviour
{
    public GameObject testa, testb = null;
    int kyaracolor;

    // Start is called before the first frame update
    void Start()
    {
        Image kyarar = testa.GetComponent<Image>();
        Image kyaral = testb.GetComponent<Image>();

        kyaral.material.color = new Color32(255, 255, 255, 255);
        kyarar.material.color = new Color32(255, 255, 255, 255);
    }

    // Update is called once per frame
    void Update()
    {
        Kyarargb();
    }

    void Kyarargb()
    {
        Image kyarar = testa.GetComponent<Image>();

        Image kyaral = testb.GetComponent<Image>();

        if (Input.GetMouseButtonUp(0))
        {
            kyaracolor++;
        }

        switch (kyaracolor)
        {

            case 0:
                kyaral.color = new Color32(114, 114, 114, 255);
                kyarar.color = new Color32(255, 255, 255, 255);
                break;

            case 1:
                kyaral.color = new Color32(255, 255, 255, 255);
                kyarar.color = new Color32(114, 114, 114, 255);
                break;

            case 2:
                kyaral.color = new Color32(114, 114, 114, 255);
                kyarar.color = new Color32(255, 255, 255, 255);
                break;

            case 3:
                kyaral.color = new Color32(255, 255, 255, 255);
                kyarar.color = new Color32(114, 114, 114, 255);
                break;

            case 4:
                kyaral.color = new Color32(114, 114, 114, 255);
                kyarar.color = new Color32(255, 255, 255, 255);
                break;

            case 5:
                kyaral.color = new Color32(114, 114, 114, 255);
                kyarar.color = new Color32(255, 255, 255, 255);
                break;

            case 6:
                kyaral.color = new Color32(255, 255, 255, 255);
                kyarar.color = new Color32(114, 114, 114, 255);
                break;

            case 7:
                kyaral.color = new Color32(114, 114, 114, 255);
                kyarar.color = new Color32(255, 255, 255, 255);
                break;

            case 8:
                kyaral.color = new Color32(114, 114, 114, 255);
                kyarar.color = new Color32(255, 255, 255, 255);
                break;

            case 9:
                kyaral.color = new Color32(255, 255, 255, 255);
                kyarar.color = new Color32(114, 114, 114, 255);
                break;

            case 10:
                kyaral.color = new Color32(114, 114, 114, 0);
                kyarar.color = new Color32(114, 114, 114, 0);
                break;

            case 11:
                kyaral.color = new Color32(255, 255, 255, 255);
                kyarar.color = new Color32(114, 114, 114, 0);
                break;

            case 12:
                kyaral.color = new Color32(255, 255, 255, 255);
                kyarar.color = new Color32(114, 114, 114, 0);
                break;

            case 13:
                kyaral.color = new Color32(114, 114, 114, 0);
                kyarar.color = new Color32(114, 114, 114, 0);
                break;

            case 14:
                kyaral.color = new Color32(114, 114, 114, 0);
                kyarar.color = new Color32(255, 255, 255, 255);
                break;

            case 15:
                kyaral.color = new Color32(114, 114, 114, 0);
                kyarar.color = new Color32(255, 255, 255, 255);
                break;

            case 16:
                kyaral.color = new Color32(114, 114, 114, 0);
                kyarar.color = new Color32(255, 255, 255, 255);
                break;

            case 17:
                kyaral.color = new Color32(114, 114, 114, 0);
                kyarar.color = new Color32(255, 255, 255, 255);
                break;

        }
    }
}