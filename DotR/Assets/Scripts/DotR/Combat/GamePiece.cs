using UnityEngine;
using UnityEngine.UI;
using System.Collections;

//////////////////////////////////////////
/// GamePiece
/// Represents one piece in the game board.
//////////////////////////////////////////

public class GamePiece : MonoBehaviour {
    // the pieces color
    private AbilityColors m_eColor;
    public AbilityColors GetColor() {
        return m_eColor;
    }

    // the button object on this piece
    private Button m_button;
    protected Button Button {
        get {
            if ( m_button == null )
                m_button = gameObject.GetComponent<Button>();

            return m_button;
        }
    }
    public bool IsAvailable() {
        bool bAvailable = Button.interactable;
        return bAvailable;
    }

    //////////////////////////////////////////
    /// Start()
    //////////////////////////////////////////
    void Start () {
	}

    //////////////////////////////////////////
    /// SetColor()
    /// Sets the color of this game piece.
    //////////////////////////////////////////
    public void SetColor( AbilityColors i_eColor, Color i_color ) {
        // set the color on the image for this element
        Button.GetComponent<Image>().color = i_color;

        // save our color
        m_eColor = i_eColor;

        // this is essentially an init, so make sure this piece is interactable
        m_button.interactable = true;
    }

    //////////////////////////////////////////
    /// PickPiece()
    /// Method for when this piece is picked
    /// by a player.
    //////////////////////////////////////////
    public void PickPice() {
        // shut this button off since it's been picked
        m_button.interactable = false;

        // send out a message that this piece has been picked
        Messenger.Broadcast<GamePiece>( "GamePiecePicked", this );
    }
}
