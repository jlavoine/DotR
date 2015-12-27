using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;

//////////////////////////////////////////
/// GameBoard
/// Going to try and make this a base 
/// class for all game boards.
//////////////////////////////////////////

public class GameBoard : Singleton<GameBoard> {
    // list of colors game pieces can be
    public List<Color> PieceColors;
    public List<AbilityColors> ValidColors;
    public Color GetAbilityColor( AbilityColors i_eColor ) {
        return PieceColors[(int) i_eColor];
    }

    // character views for this prototype combat
    public List<CharacterView> Characters;    
    public CharacterView GetViewFromType( CharacterTypes i_eType ) {
        if ( i_eType == CharacterTypes.Player )
            return Characters[0] ;
        else
            return Characters[1];
    }

    // data for characters currently being used
    private List<ProtoCharacterData> m_listData = new List<ProtoCharacterData>();
    public ProtoCharacterData GetDataFromType( CharacterTypes i_eType ) {
        return m_listData[(int)i_eType-1];
    }

    // list of game pieces on the board
    private List<GamePiece> m_listPieces;
    public List<GamePiece> GamePieces {
        get {
            if ( m_listPieces == null ) {
                GamePiece[] arrayPieces = gameObject.GetComponentsInChildren<GamePiece>();
                m_listPieces = new List<GamePiece>( arrayPieces );
            }

            return m_listPieces;
        }
    }
    public List<GamePiece> GetAvailablePieces() {
        List<GamePiece> listAvailablePieces = new List<GamePiece>();
        foreach ( GamePiece piece in GamePieces ) {
            if ( piece.IsAvailable() )
                listAvailablePieces.Add( piece );
        }

        return listAvailablePieces;
    }

    //////////////////////////////////////////
    /// Awake()
    //////////////////////////////////////////
    void Awake() {
        // create the game board
        SetUpBoard();

        // set up the characters
        SetUpCharacters();
    }

    //////////////////////////////////////////
    /// Start()
    //////////////////////////////////////////
    void Start() {
        // listen for messages
        SetUpMessages( true );

        // start the game!
        Messenger.Broadcast<CharacterTypes>( "GameStarted", CharacterTypes.Player );
    }

    //////////////////////////////////////////
    /// OnDestroy()
    //////////////////////////////////////////
    private void OnDestroy() {
        SetUpMessages( false );
    }

    //////////////////////////////////////////
    /// SetUpMessages()
    /// Subscribes or unsubscribes from messages.
    //////////////////////////////////////////
    private void SetUpMessages(bool i_bSubscribe) {
        if (i_bSubscribe) {
            Messenger.AddListener<GamePiece>( "GamePiecePicked", OnGamePiecePicked );
            Messenger.AddListener( "ResetBoard", SetUpBoard );
        }
        else {
            Messenger.RemoveListener<GamePiece>( "GamePiecePicked", OnGamePiecePicked );
            Messenger.RemoveListener( "ResetBoard", SetUpBoard );
        }
    }

    //////////////////////////////////////////
    /// SetUpBoard()
    //////////////////////////////////////////
    private void SetUpBoard() {
        // first get all of the pieces
        GamePiece[] arrayPieces = gameObject.GetComponentsInChildren<GamePiece>();
        List<GamePiece> listPieces = new List<GamePiece>( arrayPieces );

        foreach ( GamePiece piece in listPieces ) {
            // choose a random color
            AbilityColors eColor = ListUtils.GetRandomElement<AbilityColors>( ValidColors );
            Color color = PieceColors[(int) eColor];

            // set that color onto the piece
            piece.SetColor( eColor, color );
        }
    }

    //////////////////////////////////////////
    /// SetUpCharacters()
    //////////////////////////////////////////
    private void SetUpCharacters() {
        // set the player and monster views
        ProtoCharacterData charPlayer = IDL_ProtoCharacters.GetCharacter( "Cleric" );
        CharacterView viewPlayer = GetViewFromType( CharacterTypes.Player );
        viewPlayer.Init( charPlayer );

        ProtoCharacterData charMonster = IDL_ProtoCharacters.GetCharacter( "Goblin" );
        CharacterView viewMonster = GetViewFromType( CharacterTypes.AI );
        viewMonster.Init( charMonster );

        // add the data to our list
        m_listData.Add( charPlayer );
        m_listData.Add( charMonster );
    }

    //////////////////////////////////////////
    /// OnMoveMade()
    /// Callback for when a player choses a
    /// game piece on the board.
    //////////////////////////////////////////
    private void OnGamePiecePicked( GamePiece i_piece ) {
        // what resource did this game piece represent?
        AbilityColors eResource = i_piece.GetColor();

        // grant a resource to whomever is the current player
        CharacterTypes eType = TurnManager.Instance.GetCurrentCharacter();
        CharacterView viewCurrent = GetViewFromType( eType );
        string strMessageKey = "GainResource_" + viewCurrent.GetID();
        Messenger.Broadcast<AbilityColors>( strMessageKey, eResource );

        // a valid move has been taken, so send out a message
        Messenger.Broadcast( "MoveMade" );
    }
}
