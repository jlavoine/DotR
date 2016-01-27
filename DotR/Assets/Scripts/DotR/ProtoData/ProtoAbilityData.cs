﻿using System.Collections.Generic;

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
    }
}
