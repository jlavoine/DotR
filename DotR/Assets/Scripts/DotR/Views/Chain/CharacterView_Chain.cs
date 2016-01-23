using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

//////////////////////////////////////////
/// CharacterView_Chain
/// View for a player in chain combat.
//////////////////////////////////////////

public class CharacterView_Chain : CharacterView {
    // content that holds abilities
    public GameObject AbilityContent;

    // ability view prefab
    public GameObject AbilityPrefab;

    //////////////////////////////////////////
    /// Init()
    /// Inits this UI with the incoming
    /// character data.
    //////////////////////////////////////////
    public override void Init( ProtoCharacterData i_data ) {
        base.Init( i_data );

        // create ability views
        for ( int i = 0; i < i_data.Abilities.Count; ++i ) {
            // add the view object
            GameObject goView = Instantiate<GameObject>( AbilityPrefab );
            goView.transform.SetParent( AbilityContent.transform );

            // init the view
            AbilityView_Chain view = goView.GetComponent<AbilityView_Chain>();
            view.Init( i_data.Abilities[i] );
        }
    }

    //////////////////////////////////////////
    /// ListenForMessages()
    //////////////////////////////////////////
    protected override void ListenForMessages( bool i_bListen ) {
    }
}
