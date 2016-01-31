using System.Collections.Generic;

//////////////////////////////////////////
/// ProtoAbilityData
/// Data for a prototype ability.
//////////////////////////////////////////

public class ProtoAbilityData
{
    // name of the ability
    public string Name;

    // brief description of the ability
    public string Desc;

    // amount of cost required to activate the ability
    public int Cost;

    // color the ability requires
    public AbilityColors RequiredColor;

    // required colors for the chain version
    public List<AbilityColors> RequiredColors;

    // target of the ability
    public CombatTargets Target;

    // power of the ability
    public int Power;

    // list of bonuses this ability has
    public List<BonusData> Bonuses;

    // damage types of the ability
    public List<DamageTypes> DamageTypes;

    // effects to be applied (if any)
    public List<AppliedEffectData> AppliedEffects;

    // effects to be removed (if any)
    public List<RemovedEffectData> RemovedEffects;

    public ProtoAbilityData() {
        // since these are optional, let's make sure these variables are instantiated
        AppliedEffects = new List<AppliedEffectData>();
        RemovedEffects = new List<RemovedEffectData>();
        DamageTypes = new List<DamageTypes>();
        Bonuses = new List<BonusData>();
    }

    //////////////////////////////////////////
    /// VerifyChain()
    /// Returns whether or not the incoming
    /// list of colors can be matched to this
    /// ability.
    //////////////////////////////////////////
    public bool VerifyChain( List<AbilityColors> i_listColors ) {
        // if the chain so far is longer than the ability's required colors, then they are not a match
        if ( i_listColors.Count > RequiredColors.Count )
            return false;

        // otherwise, let's check to see if there's a match
        for ( int i = 0; i < i_listColors.Count; ++i ) {
            if ( i_listColors[i] != RequiredColors[i] ) {
                return false;
            }
        }

        // if there was a match throughout every color in the chain, verified!
        return true;
    }
}
