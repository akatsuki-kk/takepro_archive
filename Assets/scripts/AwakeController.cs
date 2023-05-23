using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AwakeController : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject DontDestroyObject;
    void Start()
    {
        DontDestroyOnLoad(DontDestroyObject);
        SceneManager.LoadScene(1);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
