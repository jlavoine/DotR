using System.Collections.Generic;

//////////////////////////////////////////
/// PerkBenefit
/// Immutable data for a single benefit in
/// a perk.
//////////////////////////////////////////

public class PerkBenefit {
    // the stat this benefit affects
    public string Stat;

    // list of bonuses this benefit applies (for each level)
    public List<int> BonusList;
}
