using UnityEngine;
using System.Collections;

public enum ScreenState
{
    lightUp,
    fade,
    off,
    on
}

public class Screenfade : MonoBehaviour 
{
    Texture screenFadeT;
    Color fadeColor;
    static public ScreenState screenFade = ScreenState.on;
    float fadeSpeed = 0.5f;

	// Use this for initialization
	void Start () 
    {
        fadeColor = Color.black;
        StartCoroutine(DelayStart());
	}

    IEnumerator DelayStart()
    {
        yield return new WaitForSeconds(0.5f);
        screenFade = ScreenState.lightUp;
        ScreenFade();
    }

    void OnGUI()
    {
        GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), screenFadeT);
    }

	// Update is called once per frame
	void Update () 
    {
        ScreenFade();
	}

    void ScreenFade()
    {
        switch (screenFade)
        {
            case ScreenState.fade:
                if (fadeColor.a <= 1.0f)
                    fadeColor.a += fadeSpeed * Time.deltaTime;
                break;
            case ScreenState.lightUp:
                if (fadeColor.a >= 0.0f)
                    fadeColor.a -= fadeSpeed * Time.deltaTime;
                break;
            case ScreenState.off:
                fadeColor.a = 0.0f;
                break;
            case ScreenState.on:
                fadeColor.a = 1.0f;
                break;
        }
        Texture2D texture = new Texture2D(1, 1);
        texture.SetPixel(1, 1, fadeColor);
        texture.Apply();
        screenFadeT = texture;
    }

}
