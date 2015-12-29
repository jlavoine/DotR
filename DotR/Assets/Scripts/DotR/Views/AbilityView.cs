using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

//////////////////////////////////////////
/// AbilityView
/// View for an ability in combat.
//////////////////////////////////////////

public class AbilityView : MonoBehaviour {
    // labels and images on the view
    private Text m_textName;
    private Text m_textDesc;
    public List<Image> CostTiles;

    // the ability this view represents
    private ProtoAbilityData m_dataAbility;
    public AbilityColors GetResourceUsed() {
        return m_dataAbility.RequiredColor;
    }

    // current # of cost met for this ability
    private int m_nCurrentValue;

    //////////////////////////////////////////
    /// Awake()
    //////////////////////////////////////////
    void Awake() {
        // cache some important objects
        m_textName = gameObject.FindInChildren("Name").GetComponent<Text>();
        m_textDesc = gameObject.FindInChildren("Desc").GetComponent<Text>();
    }

    //////////////////////////////////////////
    /// Init()
    /// Inits this UI with the incoming
    /// ability data.
    //////////////////////////////////////////
    public void Init(ProtoAbilityData i_data) {
        m_dataAbility = i_data;

        // set labels
        m_textName.text = i_data.Name;
        m_textDesc.text = i_data.Desc;

        // set the color of the cost tiles and destroy unnecessary cost tiles
        float fStartingAlpha = Constants.GetConstant<float>( "AbilityCost_No" );
        Color color = GameBoard.Instance.GetAbilityColor( i_data.RequiredColor );
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
        Messenger.Broadcast<ProtoAbilityData>( "QueueAction", m_dataAbility );

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
