using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GyoeiController : MonoBehaviour
{
    int move_pattern;
    // Start is called before the first frame update
    void Start()
    {
        Animator anim = GetComponent<Animator>();
        move_pattern = UnityEngine.Random.Range(0,3);
        anim.SetInteger("move_pattern",move_pattern);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
