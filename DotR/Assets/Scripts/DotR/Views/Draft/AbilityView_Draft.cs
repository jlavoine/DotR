using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

//////////////////////////////////////////
/// AbilityView_Draft
/// View for an ability in combat.
//////////////////////////////////////////

public class AbilityView_Draft : AbilityView {
    // current # of cost met for this ability
    private int m_nCurrentValue;

    //////////////////////////////////////////
    /// Init()
    /// Inits this UI with the incoming
    /// ability data.
    //////////////////////////////////////////
    public override void Init(AbilityData i_data) {
        base.Init( i_data );

        // set the color of the cost tiles and destroy unnecessary cost tiles
        float fStartingAlpha = Constants.GetConstant<float>( "AbilityCost_No" );
        Color color = Gameboard_Draft.Instance.GetAbilityColor( i_data.RequiredColor );
        color.a = fStartingAlpha;

        for ( int i = 0; i < CostTiles.Count; ++i ) { 
            // set the image to whatever color the ability uses
            Image image = CostTiles[i];
            image.color = color;

            // destroy the image if we don't need it (i.e. cost of ability is lower than index)
            if ( i >= i_data.Cost )
                Destroy( image.gameObject );
        }
    }

    //////////////////////////////////////////
    /// GainResource()
    /// Method to call when this ability
    /// should gain a resource.
    //////////////////////////////////////////
    public void GainResource() {
        // increment current value of the ability
        m_nCurrentValue++;

        // update the resource images on the ability to match how much the player has earned
        UpdateResourceImages();

        // if the ability's current value == it means the ability should be fired off
        if ( m_nCurrentValue == m_dataAbility.Cost )
            ActivateAbility();
    }

    //////////////////////////////////////////
    /// ActivateAbility()
    /// Fires the ability off.
    //////////////////////////////////////////
    private void ActivateAbility() {
        // safety check
        if ( m_nCurrentValue != m_dataAbility.Cost ) {
            Debug.LogError( "Ability illegally trying to fire: " + m_dataAbility.Name );
            return;
        }

        // reset the cost to 0
        m_nCurrentValue = 0;

        // queue the action
        Messenger.Broadcast<AbilityData>( "QueueAction", m_dataAbility );

        // update the UI
        UpdateResourceImages();
    }

    //////////////////////////////////////////
    /// UpdateResourceImages()
    /// Updates the UI for this ability by
    /// highlighting how much of the cost the
    /// player has earned.
    //////////////////////////////////////////
    private void UpdateResourceImages() {
        float fAlphaCostYes = Constants.GetConstant<float>( "AbilityCost_Yes" );
        float fAlphaCostNo = Constants.GetConstant<float>( "AbilityCost_No" );

        for ( int i = 0; i < CostTiles.Count; ++i ) {
            Image image = CostTiles[i];
            Color color = image.color;
           
            if ( i < m_nCurrentValue ) {
                // this means the resource has already been earned
                color.a = fAlphaCostYes;
            }
            else {
                // resource is yet to be earned
                color.a = fAlphaCostNo;
            }

            // set the appropriate color/alpha
            image.color = color;
        }
    }
}
