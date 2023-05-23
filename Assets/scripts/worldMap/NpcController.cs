using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcController : MonoBehaviour
{
    public GameObject Hukidasi;
    GameObject player;
    static public bool haveTalk = true;

    [SerializeField] private TalkUIController talkUIController;
    public TextAsset textFile;
    [SerializeField] private GameObject GameMaster;

    private bool settalk = true;
    //public bool endTalk = false;

    float distance = 0;
    private bool inTalkArea = false;
    [SerializeField] private float TalkAreaDistance;
    [SerializeField] private float ViewHukidashiDistance;

  

    /// <summary>画像を変更するスプライトオブジェクト。</summary>
    [SerializeField] private Sprite NearSprite;

    /// <summary>変更後の画像を持つスプライト。</summary>
    [SerializeField] private Sprite FarSprite;


    private Transform camra;
    // Start is called before the first frame update
    void Start()
    {
        Hukidasi.transform.localPosition = new Vector3(-0.188f, 1.052f, 0.351f);
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
            GameMaster.GetComponent<WorldMapMaster>().npcController = this.gameObject.GetComponent<NpcController>();

        }
        else if (distance >= TalkAreaDistance && inTalkArea)
        {
            inTalkArea = false;
            GameMaster.GetComponent<WorldMapMaster>().npcController = null;
        }

    }

    public void playerTalk()
    {
        if (distance < TalkAreaDistance && haveTalk)
        {
            talkUIController.textFile = textFile;
            talkUIController.InitializeTalk();
            WorldMapMaster.NowGameState = WorldMapMaster.GameState.Talk;
            settalk = true;
        }
    }



    private void viewHukidasi()
    {
        if (haveTalk)
        {
            if (distance < ViewHukidashiDistance)
            {
                var spriteRenderer = Hukidasi.GetComponent<SpriteRenderer>();
                Hukidasi.SetActive(true);

                if(distance < TalkAreaDistance)
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
