using UnityEngine;
using UnityEngine.UI;
using System.Collections;

//////////////////////////////////////////
/// GamePiece_Draft
/// Represents one piece in the game board.
//////////////////////////////////////////

public abstract class GamePiece : MonoBehaviour {
    // the pieces color
    private AbilityColors m_eColor;
    public AbilityColors GetColor() {
        return m_eColor;
    }

    // image piece color shows
    private Image m_image;
    protected Image Image {
        get {
            if ( m_image == null )
                m_image = gameObject.GetComponentInChildren<Image>();

            return m_image;
        }
    }

    public abstract bool IsAvailable();
    public abstract void OnColorSet();

    //////////////////////////////////////////
    /// Awake()
    //////////////////////////////////////////
    public void Awake() {
        ListenForMessages( true );
    }

    //////////////////////////////////////////
    /// OnDestroy()
    //////////////////////////////////////////
    public void OnDestroy() {
        ListenForMessages( false );
    }

    //////////////////////////////////////////
    /// ListenForMessages()
    //////////////////////////////////////////
    public virtual void ListenForMessages( bool i_bAdd ) { }

    //////////////////////////////////////////
    /// SetColor()
    /// Sets the color of this game piece.
    //////////////////////////////////////////
    public void SetColor( AbilityColors i_eColor, Color i_color ) {
        // set the color on the image for this element
        Image.color = i_color;

        // save our color
        m_eColor = i_eColor;
    }
}
