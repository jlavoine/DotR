using UnityEngine;
using System.Collections.Generic;
using System;

//////////////////////////////////////////
/// CharacterView_ChainMonster
/// This is a monster's character view for
/// the chain game. This works similar to
/// other views, but the abilities of the
/// monster are showed on a series of cards
/// to indicate what is coming up next.
//////////////////////////////////////////

public class CharacterView_ChainMonster : CharacterView {
    // this is an overflow queue for abilities the monster has queued up, but the view has no rooom to display
    private Queue<ProtoAbilityData> m_queueOverflow = new Queue<ProtoAbilityData>();

    //////////////////////////////////////////
    /// Init()
    /// Inits this UI with the incoming
    /// character data.
    //////////////////////////////////////////
    public override void Init( ProtoCharacterData i_data ) {
        base.Init( i_data );
    }

    //////////////////////////////////////////
    /// ListenForMessages()
    //////////////////////////////////////////
    protected override void ListenForMessages( bool i_bListen ) {
        if ( i_bListen ) {
            Messenger.AddListener<ProtoAbilityData, bool>( "MonsterQueuedAbility", OnQueuedAbility );
        }
        else {
            Messenger.RemoveListener<ProtoAbilityData, bool>( "MonsterQueuedAbility", OnQueuedAbility );
        }
    }

    //////////////////////////////////////////
    /// OnQueuedAbility()
    /// Callback for when the monster AI queues
    /// an ability, for this view to update the
    /// visual queue.
    //////////////////////////////////////////
    private void OnQueuedAbility( ProtoAbilityData i_dataAbility, bool i_bLoading ) {
        // if this ability is being loading into the view...
        if ( i_bLoading ) {
            // first see if any ability views are unset. If they aren't, init them with this ability.
            foreach ( AbilityView view in AbilityViews ) {
                if ( view.IsSet() == false ) {
                    view.Init( i_dataAbility );
                    return;
                }
            }

            // if the view is "full" of abilities, stick this ability in an overflow queue
            m_queueOverflow.Enqueue( i_dataAbility );
        }
        else {
            // else this ability is being queued in the middle of battle...

            // first enqueue the new abilities
            m_queueOverflow.Enqueue( i_dataAbility );

            // then bump all the abilities down one view
            for ( int i = 0; i < AbilityViews.Count - 1; ++i ) {
                ProtoAbilityData dataNext = AbilityViews[i + 1].GetAbilityData();
                AbilityViews[i].Init( dataNext );
            }

            // init the backmost ability with whatever was next from the overflow queue
            ProtoAbilityData dataNextAbility = m_queueOverflow.Dequeue();
            AbilityViews[AbilityViews.Count - 1].Init( dataNextAbility );
        }
    }
}
