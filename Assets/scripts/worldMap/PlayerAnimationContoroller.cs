using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationContoroller : MonoBehaviour
{

    Animator animator;

    public string Walk = "Walk";
    string nowAnime = "";
    string oldAnime = "";
    // Start is called before the first frame update
    void Start()
    {

        animator = GetComponent<Animator>();
        nowAnime = Walk;
        oldAnime = Walk;
    }

    // Update is called once per frame
    void Update()
    {
        animator.Play(nowAnime);

    }
}
