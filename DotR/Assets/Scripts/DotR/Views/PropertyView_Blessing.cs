using UnityEngine;
using UnityEngine.UI;
using ModelShark;

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

    // uninited blessings need to set their tooltip
    private bool m_bInit = false;

    //////////////////////////////////////////
    /// UpdateView()
    //////////////////////////////////////////
    public override void UpdateView() {
        // see if the blessing is actually on the character
        CharacterModel model = (CharacterModel) ModelToView;
        bool bHas = model.HasEffect( BlessingKey );
        EffectData effect = IDL_Effects.GetData( BlessingKey );
       
        // change the icon, if neccessary
        if ( bHas && m_eState == BlessingStates.Off )
            SetState( BlessingStates.On, effect );
        else if ( bHas == false && m_eState == BlessingStates.On )
            SetState( BlessingStates.Off, effect );

        // update the tooltip the first time this view is updated
        if ( m_bInit == false ) {
            m_bInit = true;
            UpdateTooltip( effect );
        }
    }

    //////////////////////////////////////////
    /// UpdateTooltip()
    /// Sets the tooltip appropriately based
    /// on the state of the blessing.
    //////////////////////////////////////////
    private void UpdateTooltip( EffectData i_data ) {        
        TooltipTrigger tooltip = gameObject.GetComponent<TooltipTrigger>();

        // set the title to the effect's name
        tooltip.SetText( "TitleText", i_data.Name );

        // the body of the tooltip
        string strTooltip = StringTableManager.Get( "BLESSING_TOOLTIP" );

        // set the active/inactive state
        string strStateKey = "STATE_" + m_eState.ToString();
        string strState = StringTableManager.Get( strStateKey );
        strTooltip = DrsStringUtils.Replace( strTooltip, "STATE", strState );

        // set the description
        strTooltip = DrsStringUtils.Replace( strTooltip, "BODY", i_data.CombatDesc );

        // everything has been replaced, now set it
        tooltip.SetText( "BodyText", strTooltip );
    }

    //////////////////////////////////////////
    /// SetState()
    /// Sets the graphic for the blessing based
    /// on the incoming state.
    //////////////////////////////////////////
    private void SetState( BlessingStates i_eState, EffectData i_effect ) {
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

        // update the tooltip
        UpdateTooltip( i_effect );
    }
}
