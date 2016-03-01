using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;

//////////////////////////////////////////
/// Gameboard_Draft
/// Going to try and make this a base 
/// class for all game boards.
//////////////////////////////////////////

public class Gameboard_Draft : Singleton<Gameboard_Draft> {
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
    private List<GamePiece_Draft> m_listPieces;
    public List<GamePiece_Draft> GamePieces {
        get {
            if ( m_listPieces == null ) {
                GamePiece_Draft[] arrayPieces = gameObject.GetComponentsInChildren<GamePiece_Draft>();
                m_listPieces = new List<GamePiece_Draft>( arrayPieces );
            }

            return m_listPieces;
        }
    }
    public List<GamePiece_Draft> GetAvailablePieces() {
        List<GamePiece_Draft> listAvailablePieces = new List<GamePiece_Draft>();
        foreach ( GamePiece_Draft piece in GamePieces ) {
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
    protected override void OnDestroy() {
        base.OnDestroy();
        SetUpMessages( false );
    }

    //////////////////////////////////////////
    /// SetUpMessages()
    /// Subscribes or unsubscribes from messages.
    //////////////////////////////////////////
    private void SetUpMessages(bool i_bSubscribe) {
        if (i_bSubscribe) {
            Messenger.AddListener<GamePiece_Draft>( "GamePiecePicked", OnGamePiecePicked );
            Messenger.AddListener( "RoundEnded", SetUpBoard );
        }
        else {
            Messenger.RemoveListener<GamePiece_Draft>( "GamePiecePicked", OnGamePiecePicked );
            Messenger.RemoveListener( "RoundEnded", SetUpBoard );
        }
    }

    //////////////////////////////////////////
    /// SetUpBoard()
    //////////////////////////////////////////
    private void SetUpBoard() {
        // first get all of the pieces
        GamePiece_Draft[] arrayPieces = gameObject.GetComponentsInChildren<GamePiece_Draft>();
        List<GamePiece_Draft> listPieces = new List<GamePiece_Draft>( arrayPieces );

        foreach ( GamePiece_Draft piece in listPieces ) {
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
        ProtoCharacterData charPlayer = IDL_ProtoCharacters.GetCharacter( "Finthis" );
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
    private void OnGamePiecePicked( GamePiece_Draft i_piece ) {
        // what resource did this game piece represent?
        AbilityColors eResource = i_piece.GetColor();

        // grant a resource to whomever is the current player
        DefaultModel dataChar = TurnManager.Instance.GetCurrentCharacter();
        string strMessageKey = "GainResource_" + dataChar.GetPropertyValue<string>("Name");
        Messenger.Broadcast<AbilityColors>( strMessageKey, eResource );

        // a valid move has been taken, so send out a message
        Messenger.Broadcast( "MoveMade" );
    }
}
