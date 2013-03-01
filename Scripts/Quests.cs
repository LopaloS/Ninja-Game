using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Quests : MonoBehaviour {

    public enum typeQuest
    {
        item,
        killing,
        escape
    }

    static public Quests[] quests;

    public typeQuest quest;
    string currQuest;
    public GameObject desiredItem;
    public GameObject desiredCorpse;
    string desiredCorpseName;


    public float escapeDistance = 20.0f;
    bool questDone = false;
    static public bool allQuestDone = false;
    static public ScreenState screenFade;

    MenuNGUI menuNgui;
    GameObject Player;
    List<GameObject> playerItems;

	// Use this for initialization
	void Start () {
        currQuest = quest.ToString();
        Player = GameObject.FindGameObjectWithTag("Player");
        FindQuests();
        StartCoroutine(QuestUpdate());
        if (currQuest == typeQuest.killing.ToString())
            desiredCorpseName = desiredCorpse.name;
        menuNgui = GameObject.Find("MenuNgui").GetComponent<MenuNGUI>();
	}
	
	// Update is called once per frame
	IEnumerator QuestUpdate() 
    {
        while (true)
        {
            yield return new WaitForSeconds(0.3f);
            switch (currQuest)
            {
                case "item":
                    VerifyItem();
                    break;
                case "killing":
                    VerifyKilling();
                    break;
                case "escape":
                    VerifyEscape();
                    break;
            }

            if (NumbDoneQuests() == quests.Length)
            {
                allQuestDone = true;
                Screenfade.screenFade = ScreenState.fade;
                yield return new WaitForSeconds(2.0f);
                Screenfade.screenFade = ScreenState.on;
                menuNgui.LoadNextLevel();
            }
        }
	}


    void VerifyItem()
    {
        if (playerItems == null)
            playerItems = Player.GetComponentInChildren<PlayerInteraction>().items;
        foreach (GameObject item in playerItems)
        {
            if (item == desiredItem)
            {
                questDone = true;
                break;
            }
        }
    }

    void VerifyKilling()
    {
        if (GameObject.Find(desiredCorpseName) == null)
            questDone = true;
    }

    void VerifyEscape()
    {
        if (Player == null)
            return;
        if (Vector3.Distance(transform.position, Player.transform.position)
            > escapeDistance)   
        {
            if (NumbDoneQuests() == quests.Length - 1)
                questDone = true;
        }
    }

    int NumbDoneQuests()
    {
        int counterAssignments = 0;
        foreach(Quests q in quests)
        {
            if (q.questDone)
                counterAssignments++;
        }
        return counterAssignments;
    }

    void FindQuests()
    {
        quests = FindObjectsOfType(typeof(Quests)) as Quests[];
    }
}
