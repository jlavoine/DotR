using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//////////////////////////////////////////
/// ChainManager
/// Manager in charge of handling all things
/// related to a player chaining game
/// pieces.
//////////////////////////////////////////

public class ChainManager : Singleton<ChainManager> {

    // list of colors in the current chain
    private List<GamePiece_Chain> m_listCurrentChain = new List<GamePiece_Chain>();
    public bool IsChain() {
        return m_listCurrentChain.Count > 0;
    }
    public List<GamePiece_Chain> CurrentChain {
        get {
            return m_listCurrentChain;
        }
    }

    //////////////////////////////////////////
    /// Awake()
    //////////////////////////////////////////
    void Awake() {
        // create the game board
        ListenForMessages( true );
    }

    //////////////////////////////////////////
    /// Start()
    //////////////////////////////////////////
    void Start() {
    }

    //////////////////////////////////////////
    /// OnDestroy()
    //////////////////////////////////////////
    private void OnDestroy() {
        ListenForMessages( false );
    }

    //////////////////////////////////////////
    /// ListenForMessages()
    /// Subscribes or unsubscribes from messages.
    //////////////////////////////////////////
    private void ListenForMessages( bool i_bSubscribe ) {
        if ( i_bSubscribe ) {
            Messenger.AddListener<GamePiece>( "GamePieceClicked", OnGamePieceClicked );
            Messenger.AddListener<GamePiece>( "GamePieceHover", OnGamePieceHover );
            Messenger.AddListener<GamePiece>( "GamePieceReleased", OnGamePieceReleased );
        }
        else {
            Messenger.RemoveListener<GamePiece>( "GamePieceClicked", OnGamePieceClicked );
            Messenger.RemoveListener<GamePiece>( "GamePieceHover", OnGamePieceHover );
            Messenger.RemoveListener<GamePiece>( "GamePieceReleased", OnGamePieceReleased );
        }
    }


    //////////////////////////////////////////
    /// ResetChain()
    //////////////////////////////////////////
    public void ResetChain() {
        // if there is no chain, don't do anything
        if ( IsChain() == false )
            return;

        //Debug.Log( "Resetting chain!" );
        m_listCurrentChain = new List<GamePiece_Chain>();

        Messenger.Broadcast( "ResetChain" );
    }

    //////////////////////////////////////////
    /// OnGamePieceClicked()
    //////////////////////////////////////////
    private void OnGamePieceClicked( GamePiece i_piece ) {
        // add the incoming piece to the chain
        AddToChain( i_piece );        
    }

    //////////////////////////////////////////
    /// OnGamePieceHover()
    //////////////////////////////////////////
    private void OnGamePieceHover( GamePiece i_piece ) {
        // only add to the chain if the chain has been started
        if ( IsChain() == false )
            return;

        // add the incoming piece to the chain
        AddToChain( i_piece );
    }

    //////////////////////////////////////////
    /// AddToChain()
    /// Adds the incoming piece to the active
    /// chain.
    //////////////////////////////////////////
    private void AddToChain( GamePiece i_piece ) {
        GamePiece_Chain piece = (GamePiece_Chain) i_piece;

        // if the incoming piece is already in the chain (i.e. player back tracked) it's an auto failure
        if ( m_listCurrentChain.Contains( piece ) ) {
            ResetChain();
            return;
        }

        // add piece to our list
        m_listCurrentChain.Add( piece );
        Messenger.Broadcast<List<GamePiece_Chain>>( "ChainChanged", m_listCurrentChain );
        
        // let the piece know it's being chained
        piece.BeingChained();

        // verify the chain to make sure it's legit
        VerifyChain();

        // if there's still a chain after verification, play a sound
        if ( IsChain() ) {
            // change the pitch of the sound we play based on the length of the chain
            float fPitchIncrease = Constants.GetConstant<float>( "ChainPitchIncrease" );
            float fPitch = 1f + (fPitchIncrease * m_listCurrentChain.Count-1);
            Hashtable hash = new Hashtable();            
            hash.Add( "pitch", fPitch );
 
            // play the sound
            AudioManager.Instance.PlayClip( "Potential2", hash );
        }
    }


    //////////////////////////////////////////
    /// VerifyChain()
    /// Checks to make sure the current chain
    /// is legit. If it's not, it will reset
    /// the chain.
    //////////////////////////////////////////
    private void VerifyChain() {
        /*string strMessage = "Verifying the following chain: ";        
        for ( int i = 0; i < m_listCurrentChain.Count; ++i ) {
            strMessage += m_listCurrentChain[i].GetColor().ToString() + " ";
        }
        Debug.Log( strMessage );*/

        CharacterModel modelPlayer = ModelManager.Instance.GetModel( "Cleric" );
        bool bVerified = modelPlayer.VerifyChain( m_listCurrentChain );

        // if the chain is not verified, reset it
        if ( bVerified == false ) {
            ResetChain();
        }
    }

    //////////////////////////////////////////
    /// OnGamePieceReleased()
    //////////////////////////////////////////
    private void OnGamePieceReleased( GamePiece i_piece ) {
        if ( IsChain() == false )
            return;

        // get the ability from the chain
        CharacterModel modelPlayer = ModelManager.Instance.GetModel( "Cleric" );
        ProtoAbilityData dataAbility = modelPlayer.GetAbilityFromChain( m_listCurrentChain );

        // if no ability existed, reset everything
        if ( dataAbility != null ) {
            // otherwise, we got a valid ability...let's do stuff!

            // let all the valid pieces know they were part of a complete chain
            foreach ( GamePiece_Chain piece in m_listCurrentChain )
                piece.ChainComplete();

            // queue up the ability
            Messenger.Broadcast<ProtoAbilityData, ProtoCharacterData>( "QueueActionWithCharacter", dataAbility, modelPlayer.GetData() );

            // end the round now so the action is processed (HACK)
            Messenger.Broadcast( "RoundEnded" );

            // one turn is all the player gets
            Messenger.Broadcast( "TurnEnded" );

            // let the enemy take a turn! (HACK FOR NOW)
            Messenger.Broadcast( "MonsterTurn" );
        }

        // after everything has been processed, reset the chain
        ResetChain();
    }
}
