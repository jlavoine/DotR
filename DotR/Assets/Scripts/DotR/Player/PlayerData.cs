using System.Collections.Generic;

//////////////////////////////////////////
/// PlayerData
/// Using this to store player data, at
/// least for now.
//////////////////////////////////////////

public class PlayerData : GenericData {
    // name of the player
    public string Name;

    // player's class
    public string Class;

    // available XP
    public int XP;

    // list of character abilities
    public List<string> Abilities;

    // perks mapped to their level
    public Dictionary<string, int> Perks;

    public static PlayerData GetExample() {
        PlayerData ex = new PlayerData();

        ex.ID = "P1";
        ex.Name = "Finthis";
        ex.Class = "Cleric";
        ex.XP = 1000;
        ex.Abilities = new List<string>() { "A", "B" };
        ex.Perks = new Dictionary<string, int>();

        return ex;
    }

    public PlayerData() {
        Perks = new Dictionary<string, int>();
    }
}
