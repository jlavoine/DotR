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

    // effects to be applied (if any)
    public List<AppliedEffectData> AppliedEffects;

    public ProtoAbilityData() {
        // since applied effects are optional, let's make sure the list is not null
        AppliedEffects = new List<AppliedEffectData>();
    }
}
