using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FadeController : MonoBehaviour{
    
    static float fadeSpeed = 0.02f;
    static float alpha;

    static Canvas fadeCanvas;
    static Image fadeImage;

    public static bool isFadeOut=false;
    public static bool isFadeIn=false;
    public static bool isSFadeOut=false;
    public static string nextScene;
    public static float fadeTime = 1.0f;

    static void Init()
    {
        GameObject FadeCanvasObject = new GameObject("CanvasFade");
        fadeCanvas = FadeCanvasObject.AddComponent<Canvas>();
        FadeCanvasObject.AddComponent<GraphicRaycaster>();
        fadeCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
        FadeCanvasObject.AddComponent<FadeController>();

        fadeCanvas.sortingOrder=100;
        fadeImage = new GameObject("ImageFade").AddComponent<Image>();
        fadeImage.transform.SetParent(fadeCanvas.transform, false);
        fadeImage.rectTransform.anchoredPosition = Vector3.zero;
        fadeImage.rectTransform.sizeDelta = new Vector2(9999,9999);
    }

    // Update is called once per frame
    void Update()
    {
        if(isFadeIn){
            alpha -= Time.deltaTime / fadeTime;
            fadeImage.color = new Color(0,0,0,alpha);
            if(alpha <= 0.0f){
                alpha = 0.0f;
                fadeImage.enabled = false;
                isFadeIn = false;
            }
        }

        if(isFadeOut || isSFadeOut){
            alpha += Time.deltaTime / fadeTime;
            fadeImage.color = new Color(0,0,0,alpha);
            if(alpha >= 1){
                isFadeOut = false;
                alpha=1.0f;
                if(isSFadeOut){
                    SceneManager.LoadScene(nextScene);
                    isSFadeOut = false;
                }
            }
            
        }
    }

    public static void FadeIn(float fadetime)
    {
        if (fadeImage==null) Init();
        fadeTime=fadetime;
        isFadeIn = true;
        fadeImage.color = Color.black;
    }

    public static void FadeOut(float fadetime)
    {
        if (fadeImage==null) Init();
        fadeImage.enabled = true;
        fadeTime=fadetime;
        isFadeOut=true;
        fadeImage.color=Color.clear;
    }

    public static void SceneFadeOut(float fadetime, string Scenetext)
    {
        if (fadeImage==null) Init();
        fadeImage.enabled = true;
        nextScene = Scenetext;
        fadeImage.color=Color.clear;
        fadeTime=fadetime;
        isSFadeOut=true;
    }
}


