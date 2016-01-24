using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//////////////////////////////////////////
/// MonsterAI_Chain
/// Rudimentary AI script for prototype.
//////////////////////////////////////////

public class MonsterAI_Chain : MonoBehaviour {
    // queue of monster actions
    private Queue<ProtoAbilityData> m_queueActions = new Queue<ProtoAbilityData>();

    //////////////////////////////////////////
    /// Awake()
    //////////////////////////////////////////
    void Awake() {
        // listen for messages
        ListenForMessages( true );        
    }

    //////////////////////////////////////////
    /// LoadAbilities()
    /// Loads and queues the monster's abilities.
    //////////////////////////////////////////
    void LoadAbilities() {
        // add all monster abilities to the queue of actions
        CharacterModel modelMonster = ModelManager.Instance.GetModel( "Goblin" );
        ProtoCharacterData data = modelMonster.GetData();
        foreach ( ProtoAbilityData ability in data.Abilities ) {
            // put it in the queue
            m_queueActions.Enqueue( ability );

            // send a message so that views can react to the queuing
            Messenger.Broadcast<ProtoAbilityData>( "MonsterQueuedAbility", ability );
        }
    }

    //////////////////////////////////////////
    /// OnDestroy()
    //////////////////////////////////////////
    void OnDestroy() {
        ListenForMessages( false );
    }

    //////////////////////////////////////////
    /// ListenForMessages()
    //////////////////////////////////////////
    private void ListenForMessages( bool i_bListen ) {
        if ( i_bListen ) {
            Messenger.AddListener( "MonsterTurn", UseAction );
            Messenger.AddListener( "CharactersSetUp", LoadAbilities );
        }
        else {
            Messenger.RemoveListener( "MonsterTurn", UseAction );
            Messenger.RemoveListener( "CharactersSetUp", LoadAbilities );
        }
    }

    //////////////////////////////////////////
    /// UseAction()
    /// Uses the monster's next action.
    //////////////////////////////////////////
    private void UseAction() {
        // take the first ability off the queue
        ProtoAbilityData dataAbility = m_queueActions.Dequeue();

        // send the ability to the action manager
        CharacterModel modelMonster = ModelManager.Instance.GetModel( "Goblin" );
        Messenger.Broadcast<ProtoAbilityData, ProtoCharacterData>( "QueueActionWithCharacter", dataAbility, modelMonster.GetData() );

        // now re-add the ability to the back of the monster's queue and send a message of the re-queue for the view
        m_queueActions.Enqueue( dataAbility );        
        Messenger.Broadcast<ProtoAbilityData>( "MonsterQueuedAbility", dataAbility );

        // end the round now so the action is processed by the action manager (HACK)
        Messenger.Broadcast( "RoundEnded" );

        // one turn is all the player gets
        Messenger.Broadcast( "TurnEnded" );
    }
}
