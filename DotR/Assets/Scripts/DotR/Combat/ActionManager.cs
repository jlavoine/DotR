using UnityEngine;
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
            Messenger.AddListener<AbilityData>( "QueueAction", QueueAction );
            Messenger.AddListener<AbilityData, ProtoCharacterData>( "QueueActionWithCharacter", QueueActionWithCharacter );
            Messenger.AddListener( "RoundEnded", ExecuteActions );
        }
        else {
            Messenger.RemoveListener<AbilityData>( "QueueAction", QueueAction );
            Messenger.RemoveListener<AbilityData, ProtoCharacterData>( "QueueActionWithCharacter", QueueActionWithCharacter );
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
    private void QueueAction( AbilityData i_dataAbility ) {
        // char whose turn it is
        ProtoCharacterData charCurrent = TurnManager.Instance.GetCurrentCharacter();

        QueueActionWithCharacter( i_dataAbility, charCurrent );
    }
    private void QueueActionWithCharacter( AbilityData i_dataAbility, ProtoCharacterData i_dataCharacter ) {
        // create the queued action and add it to our queue
        QueuedAction action = new QueuedAction( i_dataCharacter, i_dataAbility );
        m_queueActions.Enqueue( action );

       // Debug.Log( i_dataCharacter.Name + " is queuing " + i_dataAbility.Name );
    }

    //////////////////////////////////////////
    /// ExecuteActions()
    /// Empties the action queue and executes
    /// each action.
    //////////////////////////////////////////
    private void ExecuteActions() {
        //Debug.Log( "Executing actions!" );
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
        CharacterModel modelTarget = GetTargetModel( i_action.GetData().Target, i_action );
        CharacterModel modelAggressor = ModelManager.Instance.GetModel( i_action.GetOwnerID() );

        //Debug.Log( "Processing " + i_action.GetData().Name + " on " + modelTarget.Name );

        // check to see if any effects on the aggressor contribute to increased power        
        int nPowerBonus = modelAggressor.GetTotalModification( "AllDamage" );
        int nDamage = i_action.GetData().Power + nPowerBonus;

        // check for valid bonus damage
        nDamage = CheckForBonuses( i_action.GetData(), nDamage, modelTarget, modelAggressor );

        // now do defenses -- for now, just handle one
        if ( i_action.GetData().DamageTypes.Count > 0 ) {
            DamageTypes eDamageType = i_action.GetData().DamageTypes[0];
            int nDefense = modelTarget.GetTotalModification( eDamageType.ToString() + "Defense" );

            if ( nDamage > 0 ) {
                // something is reducing the defense
                nDamage = Mathf.Max( nDamage - nDefense, 0 );
            }
            else {
                // something is augmenting the heal
                nDamage = nDamage + nDefense;                   
            }
        }

        // for now, we're just altering the hp of the target
        modelTarget.AlterHP( nDamage );

        // handle applied effects, if any
        foreach ( AppliedEffectData effect in i_action.GetData().AppliedEffects ) {
            // get the model the effect should apply to
            CharacterModel modelEffectTarget = GetTargetModel( effect.Target, i_action );

            // apply the effect!
            modelEffectTarget.ApplyEffect( effect );
        }

        // handle remove effects, if any
        foreach ( RemovedEffectData removal in i_action.GetData().RemovedEffects ) {
            // get the model the removal should apply to
            CharacterModel modelEffectRemoval = GetTargetModel( removal.Target, i_action );

            // remove the effect!
            modelEffectRemoval.RemoveEffect( removal );
        }
    }

    //////////////////////////////////////////
    /// CheckForBonuses()
    /// Checks for bonuses and alters the
    /// incoming damage based on them, if
    /// necessary.
    //////////////////////////////////////////
    private int CheckForBonuses( AbilityData i_dataAbility, int i_nDamage, CharacterModel i_charTarget, CharacterModel i_charAggressor ) {
        // get the list of bonuses on this ability
        List<BonusData> listBonuses = i_dataAbility.Bonuses;

        // go through each bonus data and add its bonus to the total damage
        foreach ( BonusData dataBonus in listBonuses ) {
            i_nDamage += dataBonus.GetBonusAmount( i_charAggressor, i_charTarget );
        }

        return i_nDamage;
    }

    //////////////////////////////////////////
    /// GetTargetModel()
    /// Returns the character model that the
    /// incoming action targets.
    //////////////////////////////////////////
    private CharacterModel GetTargetModel( CombatTargets i_eTarget, QueuedAction i_action ) {
        // let's just brute force this for now
        string strTargetID;
        if ( i_eTarget == CombatTargets.Self ) {
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
