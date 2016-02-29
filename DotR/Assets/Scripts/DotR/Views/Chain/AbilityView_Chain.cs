using UnityEngine;
using UnityEngine.UI;
using ModelShark;
using System.Collections.Generic;

//////////////////////////////////////////
/// AbilityView_Chain
/// View for an ability in combat (chain
/// prototype).
//////////////////////////////////////////

public class AbilityView_Chain : AbilityView {
    // gameobject that holds the ui elements of the view
    private GameObject m_goElements;

    // the default preferred height of this element -- kinda hacky, but content size fitters are a pita
    private float m_fDefaultPreferredHeight;

    //////////////////////////////////////////
    /// Init()
    /// Inits this UI with the incoming
    /// ability data.
    //////////////////////////////////////////
    public override void Init( AbilityData i_data ) {
        base.Init( i_data );

        // cache some parts of the view
        m_goElements = gameObject.FindInChildren( "Elements" );
        m_fDefaultPreferredHeight = gameObject.GetComponent<LayoutElement>().preferredHeight;

        // listen for messages
        ListenForMessages( true );

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
            Messenger.AddListener<List<GamePiece_Chain>>( "ChainChanged", OnChainChanged );
            Messenger.AddListener( "ResetChain", OnChainReset );
        }
        else {
            Messenger.RemoveListener<List<GamePiece_Chain>>( "ChainChanged", OnChainChanged );
            Messenger.RemoveListener( "ResetChain", OnChainReset );
        }
    }

    //////////////////////////////////////////
    /// OnChainReset()
    /// Callback for when the player's current
    /// chain is reset.
    //////////////////////////////////////////
    private void OnChainReset() {
        // make the contents visible, since there's no chain
        SetVisibility( true );
    }

    //////////////////////////////////////////
    /// OnChainChanged()
    /// Callback for when the player adds a 
    /// tile to the chain. Note that this does
    /// not mean the chain is valid.
    //////////////////////////////////////////
    private void OnChainChanged( List<GamePiece_Chain> i_listChain ) {
        // we want to see if this ability meets the criteria of the chain...
        List<AbilityColors> listColors = new List<AbilityColors>();
        foreach ( GamePiece piece in i_listChain )
            listColors.Add( piece.GetColor() );

        bool bMeets = m_dataAbility.VerifyChain( listColors );

        // set the visibility of the contents of this view based on whether or not this is true
        SetVisibility( bMeets );
    }

    //////////////////////////////////////////
    /// SetVisibility()
    /// Sets the visibility of this view.
    //////////////////////////////////////////
    private void SetVisibility( bool i_bVis ) {
        // change the preferred height of the layout element on this view
        LayoutElement layout = GetComponent<LayoutElement>();
        if ( i_bVis ) {
            // restore the default preferred height of the layout element
            layout.preferredHeight = m_fDefaultPreferredHeight;            
        }
        else {
            // set the layout elements preferred height to 0, effectively hiding this element
            layout.preferredHeight = 0;
        }

        // turn the elements on/off (this is the parent object of all ui pieces inside the view)
        m_goElements.SetActive( i_bVis );

        // whenever we change the visibility of one of these views, we need to reset the parent scroll rect...
        ScrollRect rect = GetComponentInParent<ScrollRect>();
        rect.verticalNormalizedPosition = 1;
    }

    //////////////////////////////////////////
    /// AddTooltip()
    /// Adds a tooltip to this ability view
    /// using the ability's description.
    //////////////////////////////////////////
    private void AddTooltip( AbilityData i_data ) {
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
