using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class stickContoroller : MonoBehaviour
{
    Vector3 localPotion = new Vector3(-7, -10, 0);
    bool upStick = true;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        localPotion = this.transform.position;

        if (upStick == true)
        { 
            //Debug.Log("up");
            localPotion.y += 0.08f;
            if (localPotion.y >= -0.1f)
            {
                upStick = false;
            }
            this.transform.position = localPotion;
        }
        else if (upStick == false)
        {
            //Debug.Log("down");
            localPotion.y -= 0.08f;
            if (localPotion.y <= -9.9f)
            {
                upStick = true;
            }
            this.transform.position = localPotion;
        }

    }

    
}
