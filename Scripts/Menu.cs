using UnityEngine;
using System.Collections;

public class Menu : MonoBehaviour {
    GUIStyle menuButtonStyle;
    GUIStyle menuLogoStyle;
    Rect startGamePos;
    Rect optionsPos;
    Rect quitPos;
    Rect logoPos;
    Rect mouseSensPos;

    float buttonHeight;
    float buttonWidth;

    public Texture backGround;

    public float mouseSensitivity = 3.0f;

    GameObject Player;
    PlayerControll playerControllScript;
    TPScamera cameraScript;

    bool gameMode = false;
    bool optionsMode = false;
    bool menuMode = true;

	// Use this for initialization
	void Start () {
        DontDestroyOnLoad(this);

        buttonHeight = Screen.height * 0.1f;
        buttonWidth = Screen.width * 0.3f;

        menuLogoStyle = new GUIStyle();
        menuButtonStyle = new GUIStyle();
        menuButtonStyle.normal.textColor = menuLogoStyle.normal.textColor
            = new Color(217, 0, 0, 255);
        menuButtonStyle.font = menuLogoStyle.font = 
            Resources.Load("still time") as UnityEngine.Font;
        menuButtonStyle.fontSize = 30;
        menuLogoStyle.fontSize = Screen.height / 7;
        menuButtonStyle.clipping = menuLogoStyle.clipping = TextClipping.Overflow;


        logoPos = new Rect((Screen.width / 2) - 50 , 0, 1, 1);
        startGamePos = new Rect((Screen.width - buttonWidth) / 2,
            Screen.height * 0.5f, buttonWidth, buttonHeight);
        optionsPos = new Rect((Screen.width - buttonWidth) / 2,
            Screen.height * 0.7f, buttonWidth, buttonHeight);
        quitPos = new Rect((Screen.width - buttonWidth) / 2,
            Screen.height * 0.9f, buttonWidth, buttonHeight);
        mouseSensPos = new Rect((Screen.width - 30) / 2, 100, 250, 25);
	}

    void GetPlayerScripts()
    {
        if (Player != null) return;
        Player = GameObject.FindGameObjectWithTag("Player");
        playerControllScript = Player.GetComponentInChildren<PlayerControll>();
        cameraScript = Player.GetComponentInChildren<TPScamera>();
    }
	

	void OnGUI () {
        
        if (Input.GetKeyDown(KeyCode.Escape) || menuMode)
        {
            menuMode = true;
            Screen.showCursor = true;
            if(!gameMode)
            GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), backGround);         
            if (!optionsMode)
            {
                GUI.Label(logoPos, "NINJA", menuLogoStyle);

                if (!gameMode)
                {
                    if (GUI.Button(startGamePos, "Start Game"))
                    {
                        menuMode = false;
                        gameMode = true;
                        Screen.showCursor = false;
                        Application.LoadLevel(1);
                    }
                }
                else
                {
                    if (GUI.Button(startGamePos, "Resume"))
                    {
                        menuMode = false;
                        Screen.showCursor = false;
                        Time.timeScale = 1;
                        playerControllScript.enabled = true;
                        cameraScript.enabled = true;
                    }

                    if (Player == null)
                    {
                        GetPlayerScripts();
                    }
                    Time.timeScale = 0;
                    playerControllScript.enabled = false;
                    cameraScript.enabled = false;
                    cameraScript.mouseSensitivity = mouseSensitivity;
                }

                if (GUI.Button(optionsPos, "Options"))
                {
                    optionsMode = true;
                    return;
                }

                if (GUI.Button(quitPos, "Quit"))
                {
                    Application.Quit();
                }
            }
            else
            {
                mouseSensitivity = GUI.HorizontalSlider(mouseSensPos, 
                   mouseSensitivity, 1.0f, 10.0f);
                Rect mouseSensLable = new Rect(mouseSensPos.x, mouseSensPos.y - 60, 1, 1); 
                GUI.Label(mouseSensLable, "Mouse Sensitivity", menuButtonStyle);
                if (Input.GetKeyDown(KeyCode.Escape) ||
                    GUI.Button(new Rect((Screen.width - 150) / 2, Screen.height - 150, 150, 75), "Back"))
                {
                    optionsMode = false;
                }
            }
        }
       
	}
}
