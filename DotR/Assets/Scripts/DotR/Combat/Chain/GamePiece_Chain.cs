using UnityEngine;
using System.Collections;
using System;

public class GamePiece_Chain : GamePiece {
    public override bool IsAvailable() {
        return true;
    }

    public override void OnColorSet() {
    }

    //////////////////////////////////////////
    /// ListenForMessages()
    //////////////////////////////////////////
    public override void ListenForMessages( bool i_bAdd ) {
        base.ListenForMessages( i_bAdd );

        if ( i_bAdd ) {
            Messenger.AddListener( "ResetChain", OnResetChain );
        }
        else {
            Messenger.RemoveListener( "ResetChain", OnResetChain );
        }
    }

    public void OnPointerDown() {
        Messenger.Broadcast<GamePiece>( "GamePieceClicked", this );
    }

    public void OnPointerHover() {
        Messenger.Broadcast<GamePiece>( "GamePieceHover", this );
    }

    public void OnPointerUp() {
        Messenger.Broadcast<GamePiece>( "GamePieceReleased", this );
    }

    public void BeingChained() {
        Color color = Image.color;
        color.a = Constants.GetConstant<float>( "ChainedAlpha" );
        Image.color = color;
    }

    public void OnResetChain() {
        Color color = Image.color;
        color.a = 265f;
        Image.color = color;
    }

    public void ChainComplete() {
        // get the next color in the cycle
        AbilityColors eColor = GetColor();
        AbilityColors eColorNext = GameBoard_Chain.Instance.GetNextColor( eColor );
        Color colorNext = GameBoard_Chain.Instance.GetAbilityColor( eColorNext );

        // set that as this piece's color
        SetColor( eColorNext, colorNext );
    }
}
