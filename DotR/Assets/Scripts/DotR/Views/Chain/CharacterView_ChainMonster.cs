using UnityEngine;
using System.Collections;
using System;

public class CharacterView_ChainMonster : CharacterView {
    //////////////////////////////////////////
    /// Init()
    /// Inits this UI with the incoming
    /// character data.
    //////////////////////////////////////////
    public override void Init( ProtoCharacterData i_data ) {
        base.Init( i_data );

        // create ability views
        //for ( int i = 0; i < m_listAbilities.Count; ++i ) {
          //  m_listAbilities[i].Init( i_data.Abilities[i] );
        //}
    }

    //////////////////////////////////////////
    /// ListenForMessages()
    //////////////////////////////////////////
    protected override void ListenForMessages( bool i_bListen ) {
        if ( i_bListen ) {
            Messenger.AddListener<ProtoAbilityData>( "MonsterQueuedAbility", OnQueuedAbility );
        }
        else {
            Messenger.RemoveListener<ProtoAbilityData>( "MonsterQueuedAbility", OnQueuedAbility );
        }
    }

    //////////////////////////////////////////
    /// OnQueuedAbility()
    /// Callback for when the monster AI queues
    /// an ability, for this view to update the
    /// visual queue.
    //////////////////////////////////////////
    private void OnQueuedAbility( ProtoAbilityData i_dataAbility ) {
        // first see if any ability views are unset. If they aren't, init them with this ability.
        foreach ( AbilityView view in AbilityViews ) {
            if ( view.IsSet() == false ) {
                view.Init( i_dataAbility );
                return;
            }
        }

        // if none of the views are unset, it means we need to bump everything down one
        for ( int i = 0; i < AbilityViews.Count-1; ++i ) {
            ProtoAbilityData dataNext = AbilityViews[i + 1].GetAbilityData();
            AbilityViews[i].Init( dataNext );
        }
        AbilityViews[AbilityViews.Count - 1].Init( i_dataAbility );
    }
}
