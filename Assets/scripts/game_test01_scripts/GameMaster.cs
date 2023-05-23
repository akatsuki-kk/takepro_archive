using System.Net.Mime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
public class GameMaster : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject Border;
    public GameObject Stick;
    public int Score;
    public bool chance = false;
    public GameObject ScoreText;
    public GameObject Hinoki;

    public GameObject CutCollision0R;
    public GameObject CutCollision1R;
    public GameObject CutCollision2R;
    public GameObject CutCollision0L;
    public GameObject CutCollision1L;
    public GameObject CutCollision2L;
    public GameObject TreeCollision;


    public GameObject Course;

    bool Col0R = false;
    bool Col1R = false;
    bool Col2R = false;
    bool Col0L = false;
    bool Col1L = false;
    bool Col2L = false;
    bool TreeCol = false;

    public bool Cut = false;

    float StickYpos;
    float BorderYpos;
    float BorderTop;
    float BorderBotom;

    GameObject[] treeNames = new GameObject[2];

    bool[] ColL = new bool[3];
    bool[] ColR = new bool[3];

    bool[] Col = new bool[7];
    bool[] Success = new bool[3];

    Quaternion[] CoursePos = new Quaternion[]
    {
        Quaternion.Euler(0f, 0f, 0f),
        Quaternion.Euler(0f, 0f, 25f),
        Quaternion.Euler(0f, 0f, 165f),
        Quaternion.Euler(0f, 0f, 180f),
        Quaternion.Euler(0f, 0f, 205f),
        Quaternion.Euler(0f, 0f, 335f),
    };



    string[] treeTypes = new string[]
    {
        "Normal"
    };

    Vector3[] treePos = new Vector3[]
    {
        new Vector3(0,0,0)
    };



    void Start()
    {


        Debug.Log(Border);

        BorderTop = Border.transform.position.y + (Border.transform.localScale.y / 2f) - 5f;
        BorderBotom = Border.transform.position.y - (Border.transform.localScale.y / 2f) - 5f;

       // getCollision();

        ColR[0] = Col0R;
        ColR[1] = Col1R;
        ColR[2] = Col2R;
        ColL[0] = Col0L;
        ColL[1] = Col1L;
        ColL[2] = Col2L;

        bool[] Col ={ Col0R,Col1R,Col2R,Col0L,Col1L,Col2L,TreeCol };
        treeNames[0] = Hinoki;
        Debug.Log(treeNames[0]);
        //summonTree(Hinoki,treeTypes[0], treePos[0], Quaternion.identity);
        //summonCourse(new Vector3(0f, -2.5f, 0f), CoursePos[5]);
    }

    // Update is called once per frame
    void Update()
    {
        //getCollision();
        StickYpos = Stick.transform.position.y;
        if (StickYpos <= BorderTop && StickYpos >= BorderBotom)
        {
            chance = true;
        }
        else
        {
            chance = false;
        }

        if (Col0R == true || Col1R == true || Col2R == true || Col0L == true || Col1L == true || Col2L == true || TreeCol == true)
        {
            foreach (bool i in Col)
            {

            }
        }

    }



    public void DeterminationOfTiming()
    {

    }
}
