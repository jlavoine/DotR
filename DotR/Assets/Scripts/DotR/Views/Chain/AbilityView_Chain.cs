using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

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
    /// ActivateAbility()
    /// Fires the ability off.
    //////////////////////////////////////////
    private void ActivateAbility() {
        /*
        // safety check
        if ( m_nCurrentValue != m_dataAbility.Cost ) {
            Debug.LogError( "Ability illegally trying to fire: " + m_dataAbility.Name );
            return;
        }

        // reset the cost to 0
        m_nCurrentValue = 0;

        // queue the action
        Messenger.Broadcast<ProtoAbilityData>( "QueueAction", m_dataAbility );

        // update the UI
        UpdateResourceImages();
        */
    }
}
