using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WorldMapMaster : MonoBehaviour
{

    private static readonly Joycon.Button[] m_buttons =
       Enum.GetValues(typeof(Joycon.Button)) as Joycon.Button[];
    private List<Joycon> m_joycons;
    private Joycon m_joyconL;
    private Joycon m_joyconR;
    private Joycon.Button? m_pressedButtonL;
    private Joycon.Button? m_pressedButtonR;

    public float CharaXinput = 0;
    public float CharaZinput = 0;

    private  float[] stick;
    private int iconNum;



    public  enum GameState
    {
        Start,
        Play,
        Map,
        Pause,
        Attention,
        Talk,
        End
    }


    [SerializeField] GameObject player;
    public NpcController npcController;
    public SDGsNpcController sDGsNpcController;
    [SerializeField] TalkUIController talkUIController;
    [SerializeField] WorldSceneManager WorldSceneManager;
    [SerializeField] MinimapCotroller minimapCotroller;

    [SerializeField] Material peaceSky;
    [SerializeField] Material collapseSky;
    [SerializeField] GameObject lightGameObject;
    [SerializeField] AudioSource se;
    public AudioClip select;
    public AudioClip cansel;


    public static GameState NowGameState;
    // Start is called before the first frame update
    void Start()
    {
        joycon_setup();
        iconNum = minimapCotroller.iconNum;

        player.transform.position = Global.playerPos;
        player.transform.localEulerAngles = Global.playerAngle;
        WorldSetUP();
        NowGameState = GameState.Play;
    }

    // Update is called once per frame
    void Update()
    {
        //joycon_update();
        if (NowGameState == GameState.Play)
        {
            try
            {
                stick = m_joyconL.GetStick();
                CharaXinput = stick[0];
                CharaZinput = stick[1];
            }
            catch (System.NullReferenceException e)
            {
                CharaXinput = Input.GetAxis("Horizontal");
                CharaZinput = Input.GetAxis("Vertical");
            }

            if (npcController != null)
            {
                try
                {
                    if (m_joyconR.GetButtonDown(Joycon.Button.DPAD_RIGHT))
                    {
                        se.PlayOneShot(select);
                        npcController.playerTalk();
                        Global.playerPos = player.transform.position;
                        Global.playerAngle = player.transform.localEulerAngles;
                    }
                }
                catch (System.NullReferenceException e)
                {
                    if (Input.GetKeyDown(KeyCode.F))
                    {
                        se.PlayOneShot(select);
                        npcController.playerTalk();
                        Global.playerPos = player.transform.position;
                        Global.playerAngle = player.transform.localEulerAngles;
                    }
                }
            }

            if (sDGsNpcController != null)
            {
                try
                {
                    if (m_joyconR.GetButtonDown(Joycon.Button.DPAD_RIGHT))//Input.GetKeyDown(KeyCode.F))
                    {
                        se.PlayOneShot(select);
                        sDGsNpcController.SceneTalk();
                        Global.playerPos = player.transform.position;
                        Global.playerAngle = player.transform.localEulerAngles;
                    }
                }
                catch (System.NullReferenceException e)
                {
                    if (Input.GetKeyDown(KeyCode.F))
                    {
                        se.PlayOneShot(select);
                        sDGsNpcController.SceneTalk();
                        Global.playerPos = player.transform.position;
                        Global.playerAngle = player.transform.localEulerAngles;
                    }
                }

            }

            try
            {
                if (m_joyconR.GetButtonDown(Joycon.Button.DPAD_LEFT))
                {
                    NowGameState = GameState.Map;
                    Global.playerPos = player.transform.position;
                    Global.playerAngle = player.transform.localEulerAngles;
                }
            }
            catch (System.NullReferenceException e)
            {
                if (Input.GetKeyDown(KeyCode.Y))
                {
                    NowGameState = GameState.Map;
                    Global.playerPos = player.transform.position;
                    Global.playerAngle = player.transform.localEulerAngles;
                }
            }

        }

        else if (NowGameState == GameState.Talk)
        {
            try
            {
                if (m_joyconR.GetButtonDown(Joycon.Button.DPAD_RIGHT))
                {
                    se.PlayOneShot(select);
                    talkUIController.Message();
                }
                if (m_joyconR.GetButtonDown(Joycon.Button.DPAD_DOWN))
                {
                    se.PlayOneShot(cansel);
                    NowGameState = GameState.Play;
                    talkUIController.InitializeTalk();
                }

            }
            catch (System.NullReferenceException e)
            {
                if (Input.GetKeyDown(KeyCode.F))
                {
                    se.PlayOneShot(select);
                    talkUIController.Message();
                }
                if (Input.GetKeyDown(KeyCode.B))
                {
                    se.PlayOneShot(cansel);
                    NowGameState = GameState.Play;
                    talkUIController.InitializeTalk();
                }
            }

        }
        else if (NowGameState == GameState.Attention)
        {
            try
            {
                if (m_joyconR.GetButtonDown(Joycon.Button.DPAD_RIGHT))
                {
                    WorldSceneManager.loadScene();
                    Global.playerPos = player.transform.position;
                    Global.playerAngle = player.transform.localEulerAngles;
                }
                if (m_joyconR.GetButtonDown(Joycon.Button.DPAD_DOWN))
                {
                    se.PlayOneShot(select);
                    se.PlayOneShot(cansel);
                    NowGameState = GameState.Play;
                }

            }
            catch (System.NullReferenceException e)
            {
                if (Input.GetKeyDown(KeyCode.F))
                {
                    WorldSceneManager.loadScene();
                    Global.playerPos = player.transform.position;
                    Global.playerAngle = player.transform.localEulerAngles;
                }
                if (Input.GetKeyDown(KeyCode.B))
                {
                    se.PlayOneShot(cansel);
                    NowGameState = GameState.Play;
                }
            }
        }
        else if (NowGameState == GameState.Map)
        {
            try
            {
                iconNum = minimapCotroller.iconNum;
                if (m_joyconL.GetButtonDown(Joycon.Button.DPAD_LEFT))
                {
                    se.PlayOneShot(select);
                    iconNum -= 1;
                    minimapCotroller.iconNum = iconNum % 2;
                }
                else if (m_joyconL.GetButtonDown(Joycon.Button.DPAD_RIGHT))
                {
                    se.PlayOneShot(select);
                    iconNum += 1;
                    minimapCotroller.iconNum = iconNum % 2;
                }
                else if (m_joyconR.GetButtonDown(Joycon.Button.DPAD_RIGHT))
                {
                    se.PlayOneShot(select);
                    if (iconNum == 1)
                    {
                        Global.minigameID = 1;
                        minimapCotroller.loadMinigameScene("ryousitalk");
                        Global.playerPos = player.transform.position;
                        Global.playerAngle = player.transform.localEulerAngles;
                    }
               
                }
                else if (m_joyconR.GetButtonDown(Joycon.Button.DPAD_LEFT))
                {
                    se.PlayOneShot(select);
                    NowGameState = GameState.Play;

                }

            }
            catch (System.NullReferenceException e)
            {
                iconNum = minimapCotroller.iconNum;
                if (Input.GetKeyDown(KeyCode.LeftArrow))
                {
                    iconNum -= 1;
                    minimapCotroller.iconNum = iconNum % 2;
                }
                else if (Input.GetKeyDown(KeyCode.RightArrow))
                {
                    iconNum += 1;
                    minimapCotroller.iconNum = iconNum % 2;
                }
                else if (Input.GetKeyDown(KeyCode.F))
                {
                    se.PlayOneShot(select);
                    if (iconNum == 1)
                    {
                        minimapCotroller.loadMinigameScene("ryousitalk");
                        Global.playerPos = player.transform.position;
                        Global.playerAngle = player.transform.localEulerAngles;
                    }
                }
                else if (Input.GetKeyDown(KeyCode.Y))
                {
                    se.PlayOneShot(select);
                    NowGameState = GameState.Play;

                }
            }
        }
    }

    private void WorldSetUP()
    {
        Debug.Log(Global.world_diff);
        var garbage = GameObject.Find("Garbage");
        if(Global.world_diff == 1)
        {
            var color = new Color(241.0f / 255, 209.0f / 255, 117.0f / 255, 255.0f / 255);
            garbage.SetActive(false);
            RenderSettings.skybox = peaceSky;
            lightGameObject.GetComponent<Light>().color = color;
        }
        else if(Global.world_diff == 2)
        {
            var color = new Color(127.0f / 255, 53.0f / 255, 135.0f / 255, 255.0f / 255);
            garbage.SetActive(true);
            RenderSettings.skybox = collapseSky;
            lightGameObject.GetComponent<Light>().color = color;
        }
    }

    public void collapseButton()
    {
        Global.world_diff = 1;
        Global.fishing_diff = 1;
        NowGameState = GameState.Play;
        WorldSetUP();
    }

    public void peaceButton()
    {
        Global.world_diff = 0;
        Global.fishing_diff = 0;
        NowGameState = GameState.Play;
        WorldSetUP();
    }
    void joycon_setup()
    {
        m_joycons = JoyconManager.Instance.j;
        if (m_joycons == null || m_joycons.Count <= 0) return;

        m_joyconL = m_joycons.Find(c => c.isLeft);
        m_joyconR = m_joycons.Find(c => !c.isLeft);
    }

    void joycon_update()
    {
        m_pressedButtonL = null;
        m_pressedButtonR = null;

        if (m_joycons == null || m_joycons.Count <= 0) return;

        foreach (var button in m_buttons)
        {
            //左ジョイコンが接続されていなければ例外処理
            try
            {
                if (m_joyconL.GetButton(button))
                {
                    m_pressedButtonL = button;
                }
            }
            catch (System.NullReferenceException e) { }
            //右ジョイコンが接続されていなければ例外処理
            try
            {
                if (m_joyconR.GetButton(button))
                {
                    m_pressedButtonR = button;
                }
            }
            catch (System.NullReferenceException e) { }

        }
    }
}
