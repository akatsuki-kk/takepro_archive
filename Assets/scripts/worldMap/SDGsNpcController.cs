using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SDGsNpcController : MonoBehaviour
{
    public string CharacterName;

    public GameObject Hukidasi;
    GameObject player;
    static public bool haveTalk = true;

    [SerializeField] private WorldSceneManager worldSceneManager;
    [SerializeField] private string sceneName;

    float distance = 0;
    [SerializeField] private GameObject GameMaster;
    private bool inTalkArea = false;

    [SerializeField] private float TalkAreaDistance;
    [SerializeField] private float ViewHukidashiDistance;
    /// <summary>�摜��ύX����X�v���C�g�I�u�W�F�N�g�B</summary>
    [SerializeField] private Sprite NearSprite;
    /// <summary>�ύX��̉摜�����X�v���C�g�B</summary>
    [SerializeField] private Sprite FarSprite;
    private Transform camra;
    // Start is called before the first frame update
    void Start()
    {
        Hukidasi.transform.localPosition = new Vector3(-0.188f, 1.052f, 0.351f);
        CharacterName = gameObject.name;
        camra = Camera.main.transform;
        player = GameObject.Find("Player");

        if (Hukidasi == true)
        {
            
            Hukidasi.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()
    {
        var cameraPosition = camra.position;
        cameraPosition.y = Hukidasi.transform.position.y;
        Hukidasi.transform.LookAt(cameraPosition);
        
        distance = Vector3.Distance(Hukidasi.transform.position, player.transform.position);
        viewHukidasi();

        if (distance < TalkAreaDistance && inTalkArea == false)
        {
            inTalkArea = true;
            GameMaster.GetComponent<WorldMapMaster>().sDGsNpcController = this.gameObject.GetComponent<SDGsNpcController>();

        }
        else if (distance >= TalkAreaDistance && inTalkArea)
        {
            inTalkArea = false;
            GameMaster.GetComponent<WorldMapMaster>().sDGsNpcController = null;
        }
    }

    public void SceneTalk()
    {
        WorldMapMaster.NowGameState = WorldMapMaster.GameState.Attention;
        worldSceneManager.characterNameLabel.text = CharacterName;
        worldSceneManager.sceneName = sceneName;
    }
 



    private void viewHukidasi()
    {
        if (haveTalk)
        {
            if (distance < ViewHukidashiDistance)
            {
                var spriteRenderer = Hukidasi.GetComponent<SpriteRenderer>();
                Hukidasi.SetActive(true);

                if (distance < TalkAreaDistance)
                {
                    spriteRenderer.sprite = NearSprite;
                }
                else if (distance >= TalkAreaDistance)
                {
                    spriteRenderer.sprite = FarSprite;
                }

            }
            else if (distance >= ViewHukidashiDistance)
            {
                Hukidasi.SetActive(false);
            }
        }
        else
        {
            Hukidasi.SetActive(false);
        }

    }
}
