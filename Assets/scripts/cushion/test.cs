using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class test : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Invoke("Test", 3);

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Test()
    {
        SceneManager.LoadScene("Fishing");
    }

}