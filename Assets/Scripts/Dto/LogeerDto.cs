using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ActionType
{
    public static Dictionary<string, string> ActionDictionary = new Dictionary<string, string>()
    {
        { "climb", "c" },
        { "kill", "k" },
        { "useBambooJuice", "bj" },
        { "collectBambooJuice", "cbj" },
        { "collectPLA", "cp" },
        { "dead", "d" },
        { "exBambooJuice", "exbj" }
    };
}

public static class TGameLogNote
{
    public static Dictionary<string, string> NoteDictionary = new Dictionary<string, string>()
    {
        { "" , "" },

        { "JM001", "mikey run into an enemy" },
        { "JM002", "mikey is cautch by follower" },
        { "JM003", "mikey died of missing feet" },
        { "JM004", "mikey ate what he shouldn't have eaten" },
        
        { "magnet", "magnet" },
        { "tripleJump", "tripleJump" },
        { "highJump", "highJump" },
        { "tank", "tank" },
    };
}

public class GameLog
{
    public string a; //Action
    public int s; //Score
    public int uf; //UserFloor
    public int of; //Occurrrence Floor
    public int oi; // Occurrence Idx
    public string n; // Note
    public long unt; // Unity Time Based Utc
}
