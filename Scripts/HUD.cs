using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HUD : MonoBehaviour 
{
    public GUIStyle HUDStyle;
    public Texture visibleTeture;
    public Texture healthBar;
    public Texture healthBarFrame;
    public Texture joyRing;
    public Texture attackButt;
    public Texture jumpButt;
    public Texture actionButt;

    Rect attackButtPos;
    Rect jumpButtPos;
    Rect actionButtPos;
    Rect healthBarFramePos;
    Rect healthBarPos;
    Rect visibleIconPos;
    Rect joyRingPos;

    float buttSiz;

    float healthBarMax;
    bool visible = false;
    GameObject[] NPC;
    List<AI> npcAI;

    Health health;

    public float updateInterval = 0.5F;
    private double lastInterval;
    private int frames = 0;
    private double fps;

    public GameObject Joy;

	// Use this for initialization
	void Start () 
    {
        buttSiz = Screen.width * 0.05f;

        healthBarPos = new Rect(Screen.width * 0.025f, 15, 
            Screen.width * 0.3f, Screen.height * 0.05f);
        healthBarFramePos = new Rect(Screen.width * 0.025f, 15, 
            Screen.width * 0.3f, Screen.height * 0.05f);
        healthBarMax = healthBarPos.width;
        visibleIconPos = new Rect(Screen.width * 0.025f, Screen.height * 0.1f, buttSiz, buttSiz);
        joyRingPos = new Rect(Screen.width * 0.05f, Screen.height * 0.95f - joyRing.height,
            joyRing.height, joyRing.width);
        attackButtPos = new Rect(Screen.width - 80, Screen.height * 0.65f, buttSiz, buttSiz);
        jumpButtPos = new Rect(Screen.width - 100, Screen.height * 0.75f, buttSiz, buttSiz);
        actionButtPos = new Rect(Screen.width - 130, Screen.height * 0.85f, buttSiz, buttSiz);

        lastInterval = Time.realtimeSinceStartup;
        health = GetComponentInChildren<Health>();
	}

	
    void OnGUI()
    {
        GUI.DrawTexture(healthBarPos, healthBar);
        GUI.DrawTexture(healthBarFramePos, healthBarFrame);
        GUI.DrawTexture(joyRingPos, joyRing);
        GUI.DrawTexture(attackButtPos, attackButt);
        GUI.DrawTexture(jumpButtPos, jumpButt);
        GUI.DrawTexture(actionButtPos, actionButt);

        if (visible)
            GUI.DrawTexture(visibleIconPos, visibleTeture);
        //GUI.Label(new Rect(Screen.width - 150, 10, 200, 100), "joyPos: " +
          //  Joy.GetComponent<Joystick>().joyPos);
  
        GUI.Label(new Rect(Screen.width - 100, 40, 200, 100), "FPS " + fps.ToString("f2"));
    }

    void CalcFPS()
    {
        ++frames;
         double timeNow = Time.realtimeSinceStartup;
         if (timeNow > lastInterval + updateInterval)
         {
             fps = frames / (timeNow - lastInterval);
             frames = 0;
             lastInterval = timeNow;
         }
    }

    void Update()
    {
        GetInput();
        GetAIScript();
        CalcFPS();
        healthBarPos.width = ((health.hitPoints * 0.01f) * healthBarMax);
        foreach (AI a in npcAI)
        {
            if (a == null)
            {
                visible = false;
                return;
            }
            if (a.DetermineInRange())
            {
                visible = true;
                break;
            }
            else
                visible = false;
        }
    }

    void GetInput()
    {
        foreach (Touch touch in Input.touches)
        {
            Vector2 touchPos = Vector2.zero;
            if(touch.phase == TouchPhase.Began)
                touchPos = touch.position;
            touchPos.y = Screen.height - touch.position.y;
            if(attackButtPos.Contains(touchPos))
                BroadcastMessage("SwipeSword");
            if(jumpButtPos.Contains(touchPos))
                BroadcastMessage("Jump");
            if(actionButtPos.Contains(touchPos))
                BroadcastMessage("Interact");
        }
    }

    void GetAIScript()
    {
        NPC = GameObject.FindGameObjectsWithTag("NPC");
        if (NPC.Length == 0)
            visible = false;
        npcAI = new List<AI>();
        foreach (GameObject a in NPC)
        {
            npcAI.Add(a.GetComponentInChildren<AI>());
        }
    }

}
