using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class WorldSceneManager : MonoBehaviour
{
    public Text textLabel;
    public Text characterNameLabel;
    public string sceneName;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void loadScene()
    {
        SceneManager.LoadScene(sceneName);
    }
}
