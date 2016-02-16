using System.Collections.Generic;

//////////////////////////////////////////
/// PerkData
/// Immutable data for a perk.
//////////////////////////////////////////

public class PerkData : GenericData  {
    // dictionary of perks and levels required for this perk
    public Dictionary<string, int> PerkRequirements;

    // list of benefits for this perk
    public List<PerkBenefit> Benefits;

    // list of XP requirements for this perk (for each level)
    public List<int> ExpList;

    public static PerkData GetExample() {
        PerkData ex = new PerkData();

        ex.ID = "PERK_1";
        ex.PerkRequirements = new Dictionary<string, int>();
        ex.PerkRequirements.Add( "Perk_2", 3 );
        ex.Benefits = new List<PerkBenefit>();
        PerkBenefit benefit = new PerkBenefit();
        benefit.Stat = "HP";
        benefit.BonusList = new List<int>() { 3, 4, 5 };
        ex.Benefits.Add( benefit );
        ex.ExpList = new List<int>();
        ex.ExpList.Add( 100 );

        return ex;
    }

    public PerkData() {
        PerkRequirements = new Dictionary<string, int>();
    }

    //////////////////////////////////////////
    /// GetCostToTrain()
    /// Returns the cost to train this perk
    /// based on the incoming level.
    //////////////////////////////////////////
    public int GetCostToTrain( int i_nLevel ) {
        int nCost = -1;

        if ( ExpList.Count > i_nLevel )
            nCost = ExpList[i_nLevel];

        return nCost;
    }
}
