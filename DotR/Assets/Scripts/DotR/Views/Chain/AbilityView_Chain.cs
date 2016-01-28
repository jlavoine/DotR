using UnityEngine;
using UnityEngine.UI;
using ModelShark;

//////////////////////////////////////////
/// AbilityView_Chain
/// View for an ability in combat (chain
/// prototype).
//////////////////////////////////////////

public class AbilityView_Chain : AbilityView {
    //////////////////////////////////////////
    /// Init()
    /// Inits this UI with the incoming
    /// ability data.
    //////////////////////////////////////////
    public override void Init( ProtoAbilityData i_data ) {
        base.Init( i_data );

        // add this ability's description as a tooltip
        AddTooltip( i_data );        

        // loop through all the required colors of an ability and set the tile to that color
        // this isn't exactly safe, but I'll make sure # of colors < # of tiles for now
        for ( int i = 0; i < CostTiles.Count; ++i ) {
            // the image on the UI
            Image image = CostTiles[i];

            if ( i < m_dataAbility.RequiredColors.Count ) {
                // get the color for this part of the ability
                AbilityColors eColor = m_dataAbility.RequiredColors[i];
                Color color = GameBoard_Chain.Instance.GetAbilityColor( eColor );

                // set the image to whatever color the ability uses
                image.color = color;
            }
            else {               
                // destroy the image; it's not necessary
                Destroy( image.gameObject );
            }
        }
    }

    //////////////////////////////////////////
    /// AddTooltip()
    /// Adds a tooltip to this ability view
    /// using the ability's description.
    //////////////////////////////////////////
    private void AddTooltip( ProtoAbilityData i_data ) {
        TooltipTrigger tooltipTrigger = gameObject.AddComponent<TooltipTrigger>();
        TooltipStyle tooltipStyle = Resources.Load<TooltipStyle>( "CleanSimple" );
        tooltipTrigger.tooltipStyle = tooltipStyle;

        // Set the tooltip text.
        tooltipTrigger.SetText( "BodyText", i_data.Desc );

        // Set some extra style properties on the tooltip
        tooltipTrigger.maxTextWidth = 250;
        tooltipTrigger.backgroundTint = Color.white;
        tooltipTrigger.tipPosition = TipPosition.TopRightCorner;
    }
}
