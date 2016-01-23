using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

//////////////////////////////////////////
/// CharacterView_Chain
/// View for a player in chain combat.
//////////////////////////////////////////

public class CharacterView_Chain : CharacterView {
    //////////////////////////////////////////
    /// Init()
    /// Inits this UI with the incoming
    /// character data.
    //////////////////////////////////////////
    public override void Init( ProtoCharacterData i_data ) {
        base.Init( i_data );

        // create ability views
        for ( int i = 0; i < AbilityViews.Count; ++i ) {
            AbilityViews[i].Init( i_data.Abilities[i] );
        }
    }

    //////////////////////////////////////////
    /// ListenForMessages()
    //////////////////////////////////////////
    protected override void ListenForMessages( bool i_bListen ) {
    }
}
