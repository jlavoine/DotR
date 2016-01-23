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