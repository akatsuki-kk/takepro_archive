using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class PauseMenuController : MonoBehaviour
{
    public int button_id;
    public Sprite[] button_list = new Sprite[3]; 
    public GameObject selfButton;
    public PauseStartController PSC;
    Image m_selfbutton;
    
    // Start is called before the first frame update
    void Start()
    {
        m_selfbutton = selfButton.GetComponent<Image>();
        m_selfbutton.sprite = button_list[0];
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Mouse_hovered()
    {
        m_selfbutton.sprite = button_list[1];
    }

    public void Mouse_exited()
    {
        m_selfbutton.sprite = button_list[0];
    }

    public void Back_Game()
    {
        m_selfbutton.sprite = button_list[0];
        PSC.OnClick();
    }

    public void Back_Menu()
    {
        m_selfbutton.sprite = button_list[0];
        FadeController.SceneFadeOut(1.0f,"Minigame_rule_placeholder");
    }

    public void Restart()
    {
        m_selfbutton.sprite = button_list[0];
        FadeController.SceneFadeOut(1.0f,"Lumberjack");
    }
}
