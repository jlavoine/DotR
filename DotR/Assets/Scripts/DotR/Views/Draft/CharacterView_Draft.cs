using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

//////////////////////////////////////////
/// CharacterView_Draft
/// View for a character in combat.
//////////////////////////////////////////

public class CharacterView_Draft : CharacterView {
    //////////////////////////////////////////
    /// Init()
    /// Inits this UI with the incoming
    /// character data.
    public override void Init(ProtoCharacterData i_data) {
        base.Init( i_data );

        // create ability views
        for (int i = 0; i < AbilityViews.Count; ++i) {
            AbilityViews[i].Init(i_data.Abilities[i]);
        }        
    }

    //////////////////////////////////////////
    /// ListenForMessages()
    //////////////////////////////////////////
    protected override void ListenForMessages( bool i_bListen ) {
        // create unique key for messages for this character
        string strKey = "_" + GetID();

        if ( i_bListen ) {
            Messenger.AddListener<AbilityColors>( "GainResource" + strKey, OnResourceGained );
        }
        else {
            Messenger.RemoveListener<AbilityColors>( "GainResource" + strKey, OnResourceGained );
        }
    }
  

    //////////////////////////////////////////
    /// OnResourceGained()
    /// Callback for when this character gains
    /// a resource.
    //////////////////////////////////////////
    private void OnResourceGained( AbilityColors i_eResource ) {
        // loop through all the abilities this char has until we find one that uses the incoming resource
        foreach ( AbilityView_Draft ability in AbilityViews ) {
            AbilityColors eResource = ability.GetResourceUsed();
            if ( eResource == i_eResource )
                ability.GainResource();
        }
    }
}
