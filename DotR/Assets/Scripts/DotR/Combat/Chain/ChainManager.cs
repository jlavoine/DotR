﻿using UnityEngine;
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
    public bool IsChainStarted() {
        return m_listCurrentChain.Count > 0;
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

    public void ResetChain() {
        if ( IsChainStarted() == false )
            return;

        Debug.Log( "Resetting chain!" );
        m_listCurrentChain = new List<GamePiece_Chain>();

        Messenger.Broadcast( "ResetChain" );
    }

    //////////////////////////////////////////
    /// OnGamePieceClicked()
    //////////////////////////////////////////
    private void OnGamePieceClicked( GamePiece i_piece ) {
        AddToChain( i_piece );        

        // verify the chain to make sure it's legit
        VerifyChain();
    }

    //////////////////////////////////////////
    /// OnGamePieceHover()
    //////////////////////////////////////////
    private void OnGamePieceHover( GamePiece i_piece ) {
        // only add to the chain if the chain has been started
        if ( IsChainStarted() == false )
            return;

        AddToChain( i_piece );

        // verify the chain to make sure it's legit
        VerifyChain();
    }

    private void AddToChain( GamePiece i_piece ) {
        GamePiece_Chain piece = (GamePiece_Chain) i_piece;

        m_listCurrentChain.Add( piece );
        
        piece.BeingChained();
    }


    //////////////////////////////////////////
    /// VerifyChain()
    /// Checks to make sure the current chain
    /// is legit. If it's not, it will reset
    /// the chain.
    //////////////////////////////////////////
    private void VerifyChain() {
        string strMessage = "Verifying the following chain: ";        
        for ( int i = 0; i < m_listCurrentChain.Count; ++i ) {
            strMessage += m_listCurrentChain[i].GetColor().ToString() + " ";
        }
        Debug.Log( strMessage );

        CharacterModel modelPlayer = ModelManager.Instance.GetModel( "Cleric" );
        bool bVerified = modelPlayer.VerifyChain( m_listCurrentChain );

        if ( bVerified == false ) {
            ResetChain();
        }
    }

    //////////////////////////////////////////
    /// OnGamePieceReleased()
    //////////////////////////////////////////
    private void OnGamePieceReleased( GamePiece i_piece ) {
        if ( IsChainStarted() == false )
            return;

        // get the ability from the chain
        CharacterModel modelPlayer = ModelManager.Instance.GetModel( "Cleric" );
        ProtoAbilityData dataAbility = modelPlayer.GetAbilityFromChain( m_listCurrentChain );

        // if no ability existed, reset everything
        if ( dataAbility == null ) {
            ResetChain();
            return;
        }
        else {
            // otherwise, we got a valid ability...let's do stuff!

            // let all the valid pieces know they were part of a complete chain
            foreach ( GamePiece_Chain piece in m_listCurrentChain )
                piece.ChainComplete();

            // reset our list
            m_listCurrentChain = new List<GamePiece_Chain>();

            // queue up the ability
            Messenger.Broadcast<ProtoAbilityData, ProtoCharacterData>( "QueueActionWithCharacter", dataAbility, modelPlayer.GetData() );

            // end the round now so the action is processed (HACK)
            Messenger.Broadcast( "RoundEnded" );

            // let the enemy take a turn! (HACK FOR NOW)
            Messenger.Broadcast( "MonsterTurn" );
        }
    }
}