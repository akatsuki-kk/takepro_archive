using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugChangeWorldtype : MonoBehaviour
{
    public void cllapse()
    {
        Global.world_diff = 2;
        FadeController.SceneFadeOut(1, "maptutorial");
    }

    public void peace()
    {
        Global.world_diff = 1;
        FadeController.SceneFadeOut(1, "maptutorial");
    }
}
