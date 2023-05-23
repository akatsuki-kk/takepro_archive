using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MinimapCotroller : MonoBehaviour
{
    public int iconNum = 0;
    [SerializeField] private GameObject CityIcon;
    [SerializeField] private GameObject TuriIcon;

    [SerializeField] private Sprite CityNomal;
    [SerializeField] private Sprite CityOver;

    [SerializeField] private Sprite TuriNomal;
    [SerializeField] private Sprite TuriOver;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        var cityiconImage = CityIcon.GetComponent<Image>();
        var turiiconImage = TuriIcon.GetComponent<Image>();
        if (iconNum == 0)
        {
            cityiconImage.sprite = CityOver;
            turiiconImage.sprite = TuriNomal;
        }
        else if(iconNum == 1)
        {
            cityiconImage.sprite = CityNomal;
            turiiconImage.sprite = TuriOver;
        }
    }

    public void loadMinigameScene(string sceneName)
    {
        FadeController.SceneFadeOut(1, sceneName);
    }

}
