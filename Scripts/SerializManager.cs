using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using JsonFx.Json;

public class LevelQuest
{
    public LevelNumber[] levelNumb;
}

public class LevelNumber
{
    public string[] quests;
}


public class SerializManager 
{
    static string questsTextFile;
    static string configFile;
    static public LevelQuest levelQuests;


    public static void SaveConfig(Dictionary<string, float> conf)
    {
        foreach (KeyValuePair<string, float> c in conf)
        {
            PlayerPrefs.SetFloat(c.Key, c.Value);
        }
    }

    public static Dictionary<string, float> LoadConfig(Dictionary<string, float> conf)
    {
        Dictionary<string, float> curConfig = new Dictionary<string, float>();
        foreach (KeyValuePair<string, float> c in conf)
        {
            curConfig.Add(c.Key, PlayerPrefs.GetFloat(c.Key));
        }
        return curConfig;
    }

    public static LevelQuest LoadQuests()
    {
        //WWW www = new WWW("jar:file://" + Application.dataPath + "!/assets/quests.json");
        byte[] bytes = ((TextAsset)Resources.Load("quests", typeof(TextAsset))).bytes;
        MemoryStream s = new MemoryStream(bytes);
        StreamReader r = new StreamReader(s);
        //StreamReader r = new StreamReader(Application.dataPath + "/StreamingAssets/quests.json");
        questsTextFile = r.ReadToEnd();
        r.Close();
        JsonReader reader = new JsonReader();
        levelQuests = reader.Read<LevelQuest>(questsTextFile);
        return levelQuests;
    }

}
