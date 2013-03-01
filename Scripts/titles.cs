using UnityEngine;
using System.Collections;

public class titles : MonoBehaviour {

    Texture screenFadeT;
    Color fadeColor;
    ScreenState screenFade;
    float fadeSpeed = 0.3f;
    MenuNGUI menuNgui;

	// Use this for initialization
	void Start ()
    {
        fadeColor = Color.black;
        fadeColor.a = 1.0f;
        ScreenFade();
        StartCoroutine(ShowTitles());
        menuNgui = GameObject.Find("MenuNgui").GetComponent<MenuNGUI>();
	}
	

	// Update is called once per frame
    void Update()
    {
        ScreenFade();
    }

    void OnGUI()
    {
        GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), screenFadeT);
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
        }
        Texture2D texture = new Texture2D(1,1);
        texture.SetPixel(1,1, fadeColor);
        texture.Apply();
        screenFadeT = texture;
    }

    IEnumerator ShowTitles()
    {
        screenFade = ScreenState.lightUp;
        yield return new WaitForSeconds(8.0f);
        screenFade = ScreenState.fade;
        yield return new WaitForSeconds(4.0f);
        Destroy(menuNgui);
        Application.LoadLevel(0);
    }
}
