using System.Collections;
using System.Collections.Generic;

//////////////////////////////////////////
/// ProtoCharacterData
/// Prototype data for a character.
//////////////////////////////////////////

public class ProtoCharacterData {
    // character name
    public string Name;

    // character's HP
    public int HP;

    // list of character abilities
    public List<ProtoAbilityData> Abilities;

    // type of character (player or AI) -- how this is set will probably change
    public CharacterTypes CharacterType;
}
