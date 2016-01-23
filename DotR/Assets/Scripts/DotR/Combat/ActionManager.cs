﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//////////////////////////////////////////
/// ActionManager
/// Handles the queuing and firing of
/// combat actions.
//////////////////////////////////////////

public class ActionManager : MonoBehaviour {
    // queue of actions
    private Queue<QueuedAction> m_queueActions;

    //////////////////////////////////////////
    /// Awake()
    //////////////////////////////////////////
    void Awake() {
        // sub to messages
        ListenForMessages( true );

        // reset the queue
        ResetQueue();
    }

    //////////////////////////////////////////
    /// OnDestroy()
    //////////////////////////////////////////
    void OnDestroy() {
        // unsub from messages
        ListenForMessages( false );
    }

    //////////////////////////////////////////
    /// ListenForMessages()
    /// Subscribes or unsubscribes from messages.
    //////////////////////////////////////////
    private void ListenForMessages( bool i_bSubscribe ) {
        if ( i_bSubscribe ) {
            Messenger.AddListener<ProtoAbilityData>( "QueueAction", QueueAction );
            Messenger.AddListener<ProtoAbilityData, ProtoCharacterData>( "QueueActionWithCharacter", QueueActionWithCharacter );
            Messenger.AddListener( "RoundEnded", ExecuteActions );
        }
        else {
            Messenger.RemoveListener<ProtoAbilityData>( "QueueAction", QueueAction );
            Messenger.RemoveListener<ProtoAbilityData, ProtoCharacterData>( "QueueActionWithCharacter", QueueActionWithCharacter );
            Messenger.RemoveListener( "RoundEnded", ExecuteActions );
        }
    }

    //////////////////////////////////////////
    /// ResetQueue()
    /// Clears the action queue.
    //////////////////////////////////////////
    private void ResetQueue() {
        m_queueActions = new Queue<QueuedAction>();
    }

    //////////////////////////////////////////
    /// QueueAction()
    /// Queues an action for whichever character
    /// is currently active.
    //////////////////////////////////////////
    private void QueueAction( ProtoAbilityData i_dataAbility ) {
        // char whose turn it is
        ProtoCharacterData charCurrent = TurnManager.Instance.GetCurrentCharacter();

        QueueActionWithCharacter( i_dataAbility, charCurrent );
    }
    private void QueueActionWithCharacter( ProtoAbilityData i_dataAbility, ProtoCharacterData i_dataCharacter ) {
        // create the queued action and add it to our queue
        QueuedAction action = new QueuedAction( i_dataCharacter, i_dataAbility );
        m_queueActions.Enqueue( action );

        Debug.Log( i_dataCharacter.Name + " is queuing " + i_dataAbility.Name );
    }

    //////////////////////////////////////////
    /// ExecuteActions()
    /// Empties the action queue and executes
    /// each action.
    //////////////////////////////////////////
    private void ExecuteActions() {
        Debug.Log( "Executing actions!" );
        //StartCoroutine( ExecuteActions_() );

        while ( m_queueActions.Count > 0 ) {
            QueuedAction action = m_queueActions.Dequeue();

            // process the action -- first get the character who it will affect
            ProcessAction( action );
        }
    }
    private IEnumerator ExecuteActions_() {
        // how much time to wait between actions?
        float fWait = Constants.GetConstant<float>( "ActionWaitTime" );
        // while there are actions to execute, do it!
        while ( m_queueActions.Count > 0 ) {
            QueuedAction action = m_queueActions.Dequeue();

            // process the action -- first get the character who it will affect
            ProcessAction( action );

            // wait before doing the next action
            yield return new WaitForSeconds( fWait );
        }
    }

    //////////////////////////////////////////
    /// ProcessAction()
    /// Processes the incoming action, doing
    /// whatever it's supposed to do.
    //////////////////////////////////////////
    private void ProcessAction( QueuedAction i_action ) {
        // get the target the action affects
        CharacterModel modelTarget = GetTargetModel( i_action );

        Debug.Log( "Processing " + i_action.GetData().Name + " on " + modelTarget.Name );

        // for now, we're just altering the hp of the target
        modelTarget.AlterHP( i_action.GetData().Power );
    }

    //////////////////////////////////////////
    /// GetTargetModel()
    /// Returns the character model that the
    /// incoming action targets.
    //////////////////////////////////////////
    private CharacterModel GetTargetModel( QueuedAction i_action ) {
        // let's just brute force this for now
        string strTargetID;
        if ( i_action.GetData().Target == CombatTargets.Self ) {
            // if the action target is self, use whoever owns the ID
            strTargetID = i_action.GetOwnerID();
        }
        else {
            // if the target is oppened, use the proper target...TODO fix this!
            strTargetID = i_action.GetOwnerID() == "Cleric" ? "Goblin" : "Cleric";            
        }

        return ModelManager.Instance.GetModel( strTargetID );
    }
}
