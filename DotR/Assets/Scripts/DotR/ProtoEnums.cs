using UnityEngine;
using System.Collections;

public enum CombatTargets {
    Self,
    Opponent
}

public enum AbilityColors {
    Red,
    Black,
    Blue,
    Yellow
}

public enum CharacterTypes {
    None,
    Player,
    AI
}

public enum TheaterCombatStates {
    Idle,
    ToCombat,
    FromCombat
}

public enum EffectCategories {
    Blessing,
    Positive,
    Negative
}

public enum BlessingStates {
    On,
    Off
}

public enum ModificationTypes {
    Flat,
    Percentage
}

public enum DamageTypes {
    Physical,
    Magical
}

public enum BonusTypes {
    ForEvery,
    AtLeast
}