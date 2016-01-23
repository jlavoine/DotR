using UnityEngine;
using UnityEngine.UI;
using System.Collections;

//////////////////////////////////////////
/// GamePiece_Draft
/// Represents one piece in the game board.
//////////////////////////////////////////

public class GamePiece_Draft : GamePiece {
    // the button object on this piece
    private Button m_button;
    protected Button Button {
        get {
            if ( m_button == null )
                m_button = gameObject.GetComponent<Button>();

            return m_button;
        }
    }

    //////////////////////////////////////////
    /// IsAvailable()
    //////////////////////////////////////////
    public override bool IsAvailable() {
        bool bAvailable = Button.interactable;
        return bAvailable;
    }

    //////////////////////////////////////////
    /// SetColor()
    /// Sets the color of this game piece.
    //////////////////////////////////////////
    public override void OnColorSet() {
        // this is essentially an init, so make sure this piece is interactable
        Button.interactable = true;
    }

    //////////////////////////////////////////
    /// PickPiece()
    /// Method for when this piece is picked
    /// by a player.
    //////////////////////////////////////////
    public void PickPice() {
        // shut this button off since it's been picked
        Button.interactable = false;

        // send out a message that this piece has been picked
        Messenger.Broadcast<GamePiece_Draft>( "GamePiecePicked", this );
    }
}
