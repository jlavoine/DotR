using UnityEngine;
using UnityEngine.UI;
using System.Collections;

//////////////////////////////////////////
/// PropertyView_Blessing
/// Property view specifically designed
/// to show the status of a blessing.
//////////////////////////////////////////

public class PropertyView_Blessing : PropertyView {
    // image for this blessing
    public Image Image;

    // blessing key
    public string BlessingKey;

    // is this view on or off?
    private BlessingStates m_eState = BlessingStates.Off;

    //////////////////////////////////////////
    /// UpdateView()
    //////////////////////////////////////////
    public override void UpdateView() {
        // see if the blessing is actually on the character
        CharacterModel model = (CharacterModel) ModelToView;
        bool bHas = model.HasEffect( BlessingKey );
       
        // change the icon, if neccessary
        if ( bHas && m_eState == BlessingStates.Off )
            SetState( BlessingStates.On );
        else if ( bHas == false && m_eState == BlessingStates.On )
            SetState( BlessingStates.Off );
    }

    //////////////////////////////////////////
    /// SetState()
    /// Sets the graphic for the blessing based
    /// on the incoming state.
    //////////////////////////////////////////
    private void SetState( BlessingStates i_eState ) {
        m_eState = i_eState;

        // set the sprite
        string strSprite = BlessingKey + "_" + i_eState.ToString();
        Image.sprite = Resources.Load<Sprite>( strSprite );

        // set the alpha
        string strAlphaKey = "BlessingAlpha_" + i_eState.ToString();
        float fAlpha = Constants.GetConstant<float>( strAlphaKey );
        Color color = Image.color;
        color.a = fAlpha;
        Image.color = color;
    }
}
