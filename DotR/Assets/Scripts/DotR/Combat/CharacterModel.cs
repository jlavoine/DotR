using UnityEngine;
using System.Collections;

//////////////////////////////////////////
/// CharacterModel
/// Represents a character's data.
//////////////////////////////////////////

public class CharacterModel : DefaultModel {
    // what key should this model uses?
    public string Name;

    // data this model uses
    private ProtoCharacterData m_data;

    //////////////////////////////////////////
    /// Awake()
    //////////////////////////////////////////
    void Awake() {
        // init data asap
        m_data = IDL_ProtoCharacters.GetCharacter( Name );

        // set various things
        SetProperty( "HP", m_data.HP );
    }

    //////////////////////////////////////////
    /// AlterHP()
    /// Changes this model's HP by the incoming
    /// amount.
    //////////////////////////////////////////
    public void AlterHP( int i_nAmount ) {
        int nHP = GetPropertyValue<int>( "HP" );
        nHP += i_nAmount;
        SetProperty( "HP", nHP );
    }
}
