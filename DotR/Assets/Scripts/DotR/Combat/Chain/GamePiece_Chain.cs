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

    //////////////////////////////////////////
    /// OnPointerDown()
    //////////////////////////////////////////
    public void OnPointerDown() {
        Messenger.Broadcast<GamePiece>( "GamePieceClicked", this );
    }

    //////////////////////////////////////////
    /// OnPointerHover()
    //////////////////////////////////////////
    public void OnPointerHover() {
        Messenger.Broadcast<GamePiece>( "GamePieceHover", this );
    }

    //////////////////////////////////////////
    /// OnPointerUp()
    //////////////////////////////////////////
    public void OnPointerUp() {
        Messenger.Broadcast<GamePiece>( "GamePieceReleased", this );
    }

    //////////////////////////////////////////
    /// BeingChained()
    /// Called on a game piece when it is
    /// being chained as part of an ability.
    //////////////////////////////////////////
    public void BeingChained() {
        // change the color of the game piece
        Color color = Image.color;
        color.a = Constants.GetConstant<float>( "ChainedAlpha" );
        Image.color = color;
    }

    //////////////////////////////////////////
    /// OnResetChain()
    /// Called on a game piece when a chain
    /// is being reset.
    //////////////////////////////////////////
    public void OnResetChain() {
        // reset the color to normal
        Color color = Image.color;
        color.a = 265f;
        Image.color = color;
    }

    //////////////////////////////////////////
    /// ChainComplete()
    /// Called on a game piece when it is
    /// part of an active chain that is
    /// completed.
    //////////////////////////////////////////
    public void ChainComplete() {
        // get the next color in the cycle
        AbilityColors eColor = GetColor();
        AbilityColors eColorNext = GameBoard_Chain.Instance.GetNextColor( eColor );
        Color colorNext = GameBoard_Chain.Instance.GetAbilityColor( eColorNext );

        // set that as this piece's color
        SetColor( eColorNext, colorNext );
    }
}
