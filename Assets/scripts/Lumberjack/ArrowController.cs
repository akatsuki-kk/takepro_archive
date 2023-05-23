using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowController : MonoBehaviour
{
    Vector3 localPotion = new Vector3(-7, -10, 0);
    bool upStick = true;
    bool playflag = false;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(playflag)
        {
            localPotion = this.transform.position;

        if (upStick == true)
        {
            //Debug.Log("up");
            localPotion.y += 0.08f;
            if (localPotion.y >= 2.74f)
            {
                upStick = false;
            }
            this.transform.position = localPotion;
        }
        else if (upStick == false)
        {
            //Debug.Log("down");
            localPotion.y -= 0.08f;
            if (localPotion.y <= -4.24f)
            {
                upStick = true;
            }
            this.transform.position = localPotion;
        }
        }
    }
    
    public void move()
    {
        playflag = true;
    }

    public void stop()
    {
        playflag = false;
    }



}
