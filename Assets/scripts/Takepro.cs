using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Takepro : MonoBehaviour
{
  int rotSpeed;
  bool rotflag = false;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
      if (Input.GetMouseButtonDown(0)){
        if(rotflag){
          this.rotSpeed=0;
          rotflag = false;
        }else{
          this.rotSpeed=10;
          rotflag = true;
        }
      }
      transform.Rotate(0,0,this.rotSpeed);
    }
}
