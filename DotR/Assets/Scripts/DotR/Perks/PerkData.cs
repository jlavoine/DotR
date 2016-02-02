using System.Collections.Generic;

//////////////////////////////////////////
/// PerkData
/// Immutable data for a perk.
//////////////////////////////////////////

public class PerkData : GenericData  {
    // dictionary of perks and levels required for this perk
    public Dictionary<string, int> PerkRequirements;

    // the stat this perk affects
    public string Stat;

    // list of bonuses this perk applies (for each level)
    public List<int> BonusList;

    // list of XP requirements for this perk (for each level)
    public List<int> ExpList;

    public static PerkData GetExample() {
        PerkData ex = new PerkData();

        ex.ID = "PERK_1";
        ex.PerkRequirements = new Dictionary<string, int>();
        ex.PerkRequirements.Add( "Perk_2", 3 );
        ex.Stat = "HP";
        ex.BonusList = new List<int>();
        ex.BonusList.Add( 5 );
        ex.BonusList.Add( 6 );
        ex.ExpList = new List<int>();
        ex.ExpList.Add( 100 );

        return ex;
    }

    public PerkData() {
        PerkRequirements = new Dictionary<string, int>();
    }
}
