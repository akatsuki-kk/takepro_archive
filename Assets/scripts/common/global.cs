using UnityEngine;
using UnityEngine.UI;

public class Global{

    //ミニゲームidを保持する変数
    public static int minigameID{get;set;}=0;
    public static string rule_description{get;set;}="";
    public static string ctrl_description{get;set;}="";
    //主人公性別(男=0,女=1)
    public static int actor_sex{get;set;}=0;
    //釣りミニゲーム難易度調整変数
    public static int fishing_diff{get;set;}=0;
    //ED分岐にかかわる変数
    public static int ED{get;set;}=0;
    //環境用変数
    public static int world_diff { get; set; } = 0;

    /// <summary>
    /// ０が普通
    /// １が良い
    /// ２が悪い
    /// </summary>

    //ワールドマッププレイヤー座標
    public static Vector3 playerPos { get; set; } = new Vector3(-38, 0, 205);
    public static Vector3 playerAngle { get; set; } = new Vector3(0, 90, 0);


    public static void FillScreen(GameObject a)
    {
        SpriteRenderer sr = a.GetComponent<SpriteRenderer>();

        // カメラの外枠のスケールをワールド座標系で取得
        float worldScreenHeight = Camera.main.orthographicSize * 2f;
        float worldScreenWidth = worldScreenHeight / Screen.height * Screen.width;

        // スプライトのスケールもワールド座標系で取得
        float width = sr.sprite.bounds.size.x;
        float height = sr.sprite.bounds.size.y;

        //  両者の比率を出してスプライトのローカル座標系に反映
        a.transform.localScale = new Vector3(worldScreenWidth / width, worldScreenHeight / height);

        // カメラの中心とスプライトの中心を合わせる
        Vector3 camPos = Camera.main.transform.position;
        camPos.z = 0;
        a.transform.position = camPos;
    }
  
}


