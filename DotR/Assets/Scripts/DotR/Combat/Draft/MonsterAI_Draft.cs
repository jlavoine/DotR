using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//////////////////////////////////////////
/// MonsterAI_Draft
/// Rudimentary AI script for prototype.
//////////////////////////////////////////

public class MonsterAI_Draft : MonoBehaviour {

    //////////////////////////////////////////
    /// Awake()
    //////////////////////////////////////////
    void Awake() {
        // listen for messages
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
    private void ListenForMessages( bool i_bListen ) {
        if ( i_bListen )
            Messenger.AddListener<int>( "MonsterTurn", MakeMoves );
        else
            Messenger.RemoveListener<int>( "MonsterTurn", MakeMoves );
    }

    //////////////////////////////////////////
    /// MakeMoves()
    /// The AI will make some moves based on
    /// the game board.
    //////////////////////////////////////////
    private void MakeMoves( int i_nMoves ) {
        //Debug.Log( "AI going to take " + i_nMoves );
        // kick off the coroutine version of this method
        StartCoroutine( MakeMoves_( i_nMoves ) );
    }
    private IEnumerator MakeMoves_( int i_nMoves ) {
        // how long should the AI wait between moves?
        float fWait = Constants.GetConstant<float>( "MonsterWaitTime" );

        // wait before making first move
        yield return new WaitForSeconds( fWait );

        // for now, just pick randomly from amongst available pieces
        List<GamePiece_Draft> listAvailablePieces = Gameboard_Draft.Instance.GetAvailablePieces();
        List<GamePiece_Draft> listPickedPieces = ListUtils.GetRandomElements<GamePiece_Draft>( listAvailablePieces, i_nMoves );
        foreach ( GamePiece_Draft piece in listPickedPieces ) {
            // pick a piece
            piece.PickPice();

            // wait a bit
            yield return new WaitForSeconds( fWait );
        }
    }
}
