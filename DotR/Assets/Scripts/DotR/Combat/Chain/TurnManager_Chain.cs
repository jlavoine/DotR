using UnityEngine;
using System.Collections;

public class TurnManager_Chain : Singleton<TurnManager_Chain> {
    // who started the game?
    private CharacterTypes m_eStartingCharacter;

    // current character whose turn it is
    private CharacterTypes m_eActiveCharacter;
    private ProtoCharacterData m_dataActiveCharacter;
    public ProtoCharacterData GetCurrentCharacter() {
        return m_dataActiveCharacter;
    }

    //////////////////////////////////////////
    /// Awake()
    //////////////////////////////////////////
    void Awake() {
        // sub to messages
        ListenForMessages( true );
    }

    //////////////////////////////////////////
    /// OnDestroy()
    //////////////////////////////////////////
    protected override void OnDestroy() {
        base.OnDestroy();

        // unsub from messages
        ListenForMessages( false );
    }

    //////////////////////////////////////////
    /// ListenForMessages()
    /// Subscribes or unsubscribes from messages.
    //////////////////////////////////////////
    private void ListenForMessages( bool i_bSubscribe ) {
        if ( i_bSubscribe ) {
            Messenger.AddListener<CharacterTypes>( "GameStarted", OnGameStarted );
            Messenger.AddListener( "TurnEnded", OnTurnEnded );
        }
        else {
            Messenger.RemoveListener<CharacterTypes>( "GameStarted", OnGameStarted );
            Messenger.RemoveListener( "TurnEnded", OnTurnEnded );
        }
    }

    //////////////////////////////////////////
    /// OnTurnEnded()
    /// Callback for when the current turn
    /// has ended.
    //////////////////////////////////////////
    private void OnTurnEnded() {
        // send out a message that this character's turn is ending
        Messenger.Broadcast( "TurnOver_" + m_dataActiveCharacter.Name );

        // set the active character -- right now it's either the AI or player's turn
        CharacterTypes eActiveCharacter = m_eActiveCharacter == CharacterTypes.AI ? CharacterTypes.Player : CharacterTypes.AI;
        SetActiveCharacter( eActiveCharacter );
    }

    //////////////////////////////////////////
    /// OnGameStarted()
    /// Callback for when the game starts. The
    /// incoming character type gets to go 
    /// first.
    //////////////////////////////////////////
    private void OnGameStarted( CharacterTypes i_eType ) {
        m_eStartingCharacter = i_eType;

        SetActiveCharacter( i_eType );
    }

    //////////////////////////////////////////
    /// SetActiveCharacter()
    /// Sets the active character to the
    /// incoming type.
    //////////////////////////////////////////
    private void SetActiveCharacter( CharacterTypes i_eType ) {
        // make sure the active character isn't being set numerous times in a row
        if ( m_eActiveCharacter == i_eType ) {
            Debug.LogError( "Warning, character getting set to current twice: " + i_eType );
            return;
        }

        // set the active char
        m_eActiveCharacter = i_eType;
        m_dataActiveCharacter = GameBoard_Chain.Instance.GetDataFromType( i_eType );
    }
}
