using UnityEngine;
using UnityEngine.UI;
using System.Collections;

//////////////////////////////////////////
/// TurnManager
/// In charge of deciding whose turn it is
/// and providing the player feedback in
/// regards to this.
//////////////////////////////////////////

public class TurnManager : Singleton<TurnManager> {
    // who started the game?
    private CharacterTypes m_eStartingCharacter;

    // current character whose turn it is
    private CharacterTypes m_eActiveCharacter;
    private ProtoCharacterData m_dataActiveCharacter;
    public CharacterTypes GetCurrentCharacter() {
        return m_eActiveCharacter;
    }

    // current round and turn variables
    private int m_nRound;
    private int m_nTurn;
    private int m_nMovesLeft;
    private void ResetMovesLeft() {
        m_nMovesLeft = Constants.GetConstant<int>( "Moves_Turn_" + m_nTurn );
    }

    // text display some helpful info to the player about whose turn it is
    public Text TurnText;

    //////////////////////////////////////////
    /// Awake()
    //////////////////////////////////////////
    void Awake() {
        // reset some values
        m_nRound = 1;
        m_nTurn = 1;
        ResetMovesLeft();

        // sub to messages
        ListenForMessages( true );
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
            Messenger.AddListener<CharacterTypes>( "GameStarted", OnGameStarted );
            Messenger.AddListener( "MoveMade", OnMoveMade );
        }
        else {
            Messenger.RemoveListener<CharacterTypes>( "GameStarted", OnGameStarted );
            Messenger.RemoveListener( "MoveMade", OnMoveMade );
        }
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
    /// OnMoveMade()
    /// Callback for when a character makes a
    /// move.
    //////////////////////////////////////////
    private void OnMoveMade() {
        // a move has been made, so decrement # of moves left
        m_nMovesLeft--;

        // if there are 0 moves left, we need to flip whose turn it is and update/reset counters
        if ( m_nMovesLeft <= 0 ) {
            // if the non-starting character has taken the last turn in a round, increment round and reset the turn
            int nMaxTurns = Constants.GetConstant<int>( "TurnsPerRound" );
            if ( m_nTurn == nMaxTurns && m_eStartingCharacter != m_eActiveCharacter ) {
                m_nRound++;                             // new round!
                m_nTurn = 1;                            // next round starts at turn 1
                Messenger.Broadcast( "ResetBoard" );    // reset the game board
            }
            else if ( m_eStartingCharacter != m_eActiveCharacter ) {
                // otherwise if the non-starting character took their turn, just increment the round
                m_nTurn++;
            }

            // reset the # of moves the active char can take
            ResetMovesLeft();

            // swap the active char
            CharacterTypes eNewChar = m_eActiveCharacter == CharacterTypes.Player ? CharacterTypes.AI : CharacterTypes.Player;
            SetActiveCharacter( eNewChar );
        }
        else
            UpdateUI(); // update the UI if a move was taken but there are still moves left before a turn ends
    }

    //////////////////////////////////////////
    /// SetActiveCharacter()
    /// Sets the active character to the
    /// incoming type.
    //////////////////////////////////////////
    private void SetActiveCharacter( CharacterTypes i_eType ) {
        // make sure the active character isn't being set numerous times in a row
        if (m_eActiveCharacter == i_eType ) {
            Debug.LogError( "Warning, character getting set to current twice: " + i_eType );
            return;
        }

        // set the active char
        m_eActiveCharacter = i_eType;
        m_dataActiveCharacter = GameBoard.Instance.GetDataFromType( i_eType );

        // update the UI since the character changed
        UpdateUI();

        // if it's the AI's turn we have to send out a special message
        if ( m_eActiveCharacter == CharacterTypes.AI )
            Messenger.Broadcast<int>( "MonsterTurn", m_nMovesLeft );
    }

    //////////////////////////////////////////
    /// UpdateUI()
    /// Updates the UI for this manager based
    /// on whose turn it is and how many moves
    /// they have left to make.
    //////////////////////////////////////////
    private void UpdateUI() {
        // get the name to display of whose turn it is
        string strName = m_dataActiveCharacter.Name;

        // format the help text with the current character's turn and # of moves left to make
        string strTurnText = StringTableManager.Get( "TURN_TEXT" );
        strTurnText = DrsStringUtils.Replace( strTurnText, "NAME", strName );
        strTurnText = DrsStringUtils.Replace( strTurnText, "NUM", m_nMovesLeft );

        TurnText.text = strTurnText;
    }
}
