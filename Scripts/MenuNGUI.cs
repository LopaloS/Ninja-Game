using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MenuNGUI : MonoBehaviour
{
    public GameObject buttStartGame;
    public GameObject buttResume;
    public GameObject mainMenu;
    public GameObject options;
    public GameObject background;
    public GameObject joystick;
    public GameObject task;
    public GameObject buttTask;
    public GameObject loadingLabel;

    public GameObject sensSlider;
    public GameObject taskLabel;
    Dictionary<string, float> config;

    GameObject Player;
    PlayerControll playerControllScript;
    TPScamera cameraScript;
    HUD playerHUD;

    public UISlider touchSensitivity;
    UILabel taskText;

    int lastLevel;
    bool isLoading = false;
    bool isLoaded = false;
    
    bool startLevel = false;
    bool gameMode = false;
    bool optionsMode = false;
    bool menuMode = true;

	// Use this for initialization
	void Start () 
    {
        DontDestroyOnLoad(this);
        Screen.showCursor = true;
        lastLevel = Application.loadedLevel;
        touchSensitivity = sensSlider.GetComponent<UISlider>();
        taskText = taskLabel.GetComponent<UILabel>();
        config = new Dictionary<string, float>();
        config.Add("touchSens", touchSensitivity.sliderValue);
        config = SerializManager.LoadConfig(config);
        touchSensitivity.sliderValue = config["touchSens"];

	}

    void GetPlayerScripts()
    {
        if (joystick == null)
            joystick = GameObject.Find("Single Joystick");
        if (Player != null) return;
        Player = GameObject.FindGameObjectWithTag("Player");
        playerControllScript = Player.GetComponentInChildren<PlayerControll>();
        cameraScript = Player.GetComponentInChildren<TPScamera>();
        playerHUD = Player.GetComponentInChildren<HUD>();
    }

    void Update() 
    {
        ShowLoadIcon();

        if (Input.GetKeyDown(KeyCode.Escape))
            ShowMenu();
        if (cameraScript != null)
            cameraScript.mouseSensitivity = 
                touchSensitivity.sliderValue * 3;
        if(Application.loadedLevel > 0)
            NGUITools.SetActive(background, false);
        if(!Application.isLoadingLevel)
            NGUITools.SetActive(background, true);
        if(lastLevel != Application.loadedLevel)
            PrintTask();
        if (isLoaded)
        {
            startLevel = true;
            isLoaded = false;
            ShowTask();
        }
        print(startLevel);
        print(isLoaded);
    }

    void ShowLoadIcon()
    {
        if (isLoading)
        {
            if (!loadingLabel.activeSelf)
            {
                loadingLabel.SetActive(true);
            }
        }
        else
        {
            if (loadingLabel.activeSelf)
            {
                loadingLabel.SetActive(false);
            }
        }
    }

    void PrintTask()
    {
        if (Application.loadedLevel == 0)
            return;
        if (Application.loadedLevelName == "Titles")
            return;
        string text = "";
        LevelNumber levelText = 
            SerializManager.LoadQuests().levelNumb[Application.loadedLevel - 1];
        int i = 0;
        foreach (string s in levelText.quests)
        {
            ++i;
            text += i.ToString() + ". " + s;
            text += "\r\n";
        }
        taskText.text = text;
        lastLevel = Application.loadedLevel;
    }

	void StartGame()
    {
        gameMode = true;
        menuMode = false;
        //Screen.showCursor = false;
        LoadNextLevel();
        NGUITools.SetActive(mainMenu, false);
    }

    void Resume()
    {
        menuMode = false;
        Screen.showCursor = false;
        Time.timeScale = 1;
        playerControllScript.enabled = true;
        cameraScript.enabled = true;
        playerHUD.enabled = true;
        joystick.SetActive(true);
        NGUITools.SetActive(mainMenu, false);
    }

    public void ShowMenu()
    {
        NGUITools.SetActive(mainMenu, true);
        NGUITools.SetActive(options, false);
        NGUITools.SetActive(task, false);
        if (gameMode)
        {
            if (Player == null)
            {
                GetPlayerScripts();
            }
            Time.timeScale = 0;
            Screen.showCursor = true;
            playerControllScript.enabled = false;
            cameraScript.enabled = false;
            playerHUD.enabled = false;
            joystick.SetActive(false);
            NGUITools.SetActive(background, false);
            NGUITools.SetActive(buttResume, true);
            NGUITools.SetActive(buttStartGame , false);
            NGUITools.SetActive(buttTask, true);
        }

        else if (!gameMode)
        {
            NGUITools.SetActive(buttResume, false);
            NGUITools.SetActive(buttStartGame, true);
            NGUITools.SetActive(buttTask, false);
        }
    }

    void Options()
    {
        NGUITools.SetActive(mainMenu, false);
        NGUITools.SetActive(options, true);
    }

    void ShowTask()
    {
        if (Application.loadedLevelName == "Titles")
            return;
        NGUITools.SetActive(mainMenu, false);
        NGUITools.SetActive(task, true);
    }

    void Back()
    {
        if (startLevel)
        {
            NGUITools.SetActive(task, false);
            Screen.showCursor = false;
            startLevel = false;
        }
        else
        {
            ShowMenu();
        }
    }

    void Quit()
    {
        config.Remove("touchSens");
        config.Add("touchSens", touchSensitivity.sliderValue);
        SerializManager.SaveConfig(config);
        Application.Quit();
    }

    public void LoadNextLevel()
    {
        isLoading = true;
        ShowLoadIcon();
        Application.LoadLevel(Application.loadedLevel + 1);
        isLoading = false;
        isLoaded = true;
        Screen.showCursor = true;
    }
}
