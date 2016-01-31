using UnityEngine;
using System.Collections;

//////////////////////////////////////////
/// VictoryManager
/// Manager in charge of monitoring 
/// health statuses and declaring victory
/// or defeat for the player.
//////////////////////////////////////////

public class VictoryManager : Singleton<VictoryManager> {
    // game over screen
    public GameObject GameOverScreen;

    // is the game over?
    private bool m_bGameOver;
    public bool IsGameOver() {
        return m_bGameOver;
    }

    //////////////////////////////////////////
    /// Start()
    //////////////////////////////////////////
    void Start () {
        ListenForMessages( true );
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
    private void ListenForMessages( bool i_bSubscribe ) {
        if ( i_bSubscribe ) {
            Messenger.AddListener( "RoundEnded", OnRoundEnded );
        }
        else {
            Messenger.RemoveListener( "RoundEnded", OnRoundEnded );
        }
    }

    //////////////////////////////////////////
    /// OnRoundEnded()
    /// Callback for whenever a round ends.
    //////////////////////////////////////////
    private void OnRoundEnded() {
        // if the game is somehow already over, bail
        if ( IsGameOver() ) {
            Debug.LogError( "Game is already over, but somehow things still processing in the Victory Manager." );
            return;
        }

        // a round is ended, so check if the game should end in defeat or victory
        CharacterModel modelPlayer = ModelManager.Instance.GetModel( "Cleric" );
        CharacterModel modelMonster = ModelManager.Instance.GetModel( "Goblin" );

        if ( modelPlayer.IsDead() )
            GameOver( false );
        else if ( modelMonster.IsDead() )
            GameOver( true );
    }

    //////////////////////////////////////////
    /// GameOver()
    /// Effectively ends the game in victory
    /// or defeat.
    //////////////////////////////////////////
    private void GameOver( bool i_bVictory ) {
        m_bGameOver = true;
        GameObject goGameOver = gameObject.InstantiateUI( GameOverScreen );
        GameOverScreen script = goGameOver.GetComponent<GameOverScreen>();
        script.Init( i_bVictory );
    }
}
