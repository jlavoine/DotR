using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;

//////////////////////////////////////////
/// Gameboard_Draft
/// Going to try and make this a base 
/// class for all game boards.
//////////////////////////////////////////

public class GameBoard_Chain : Singleton<GameBoard_Chain> {
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
            return Characters[0];
        else
            return Characters[1];
    }

    // data for characters currently being used
    private List<ProtoCharacterData> m_listData = new List<ProtoCharacterData>();
    public ProtoCharacterData GetDataFromType( CharacterTypes i_eType ) {
        return m_listData[(int) i_eType - 1];
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

        PlayerData ex = PlayerData.GetExample();
        List<PlayerData> listEx = new List<PlayerData>();
        listEx.Add( ex );
        string json = SerializationUtils.Serialize( listEx );
        Debug.Log( json );

        //EffectData eff = IDL_Effects.GetData( "BLESSING_REGEN" );
        //Debug.Log( "Hows this: " + eff.Name );
    }

    //////////////////////////////////////////
    /// Start()
    //////////////////////////////////////////
    void Start() {
        // set up the characters
        SetUpCharacters();

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
    private void SetUpMessages( bool i_bSubscribe ) {
        if ( i_bSubscribe ) {
            Messenger.AddListener<GamePiece_Draft>( "GamePiecePicked", OnGamePiecePicked );
            //Messenger.AddListener( "RoundEnded", SetUpBoard );
        }
        else {
            Messenger.RemoveListener<GamePiece_Draft>( "GamePiecePicked", OnGamePiecePicked );
            //Messenger.RemoveListener( "RoundEnded", SetUpBoard );
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
        // TODO: Load from player data!
        ProtoCharacterData charPlayer = IDL_ProtoCharacters.GetCharacter( "Finthis" );
        CharacterView viewPlayer = GetViewFromType( CharacterTypes.Player );
        viewPlayer.Init( charPlayer );

        ProtoCharacterData charMonster = IDL_ProtoCharacters.GetCharacter( "Goblin" );
        CharacterView viewMonster = GetViewFromType( CharacterTypes.AI );
        viewMonster.Init( charMonster );

        // add the data to our list
        m_listData.Add( charPlayer );
        m_listData.Add( charMonster );

        // let the game know characters have been set up
        Messenger.Broadcast( "CharactersSetUp" );
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

    //////////////////////////////////////////
    /// GetNextColor()
    /// Based on the incoming color, returns the
    /// next color in the cycle.
    //////////////////////////////////////////
    public AbilityColors GetNextColor( AbilityColors i_eColor ) {
        if ( i_eColor == AbilityColors.Red )
            return AbilityColors.Black;
        else if ( i_eColor == AbilityColors.Black )
            return AbilityColors.Blue;
        else if ( i_eColor == AbilityColors.Blue )
            return AbilityColors.Yellow;
        else // yellow
            return AbilityColors.Red;
    }
}
