using UnityEngine;
using UnityEngine.UI;

//////////////////////////////////////////
/// PerkView
/// In a player's skill tree, this is a
/// UI element that represents one perk.
//////////////////////////////////////////

public class PerkView : PropertyView {
    // ID for this view
    public string ID;

    public Text Name;
    public Text Cost;

    // the data associated with this perk
    private PerkData m_data;
    protected PerkData Data {
        get {
            if ( m_data == null )
                m_data = IDL_Perks.GetData( ID );

            return m_data;
        }
    }

    //////////////////////////////////////////
    /// UpdateName()
    /// Set the name for the perk button.
    /// This is probably temporary.
    //////////////////////////////////////////
    private void UpdateName() {        
        // get the name of the park
        string strName = StringTableManager.Get( "PerkName_" + Data.ID );

        // get the level of the perk
        PlayerModel model = (PlayerModel) ModelToView;
        int nLevel = model.GetPerkLevel( Data.ID );

        // string to display
        string strDisplay = strName + "(" + nLevel + ")";

        // put the display string on the text component
        Name.text = strDisplay;
    }

    //////////////////////////////////////////
    /// UpdateCost()
    //////////////////////////////////////////
    private void UpdateCost() {
        // get the level of the perk
        PlayerModel model = (PlayerModel) ModelToView;
        int nLevel = model.GetPerkLevel( Data.ID );

        int nCost = Data.GetCostToTrain( nLevel );
        string strLabel = StringTableManager.Get( "Perk_ToTrain" );
        string strCost = nCost > 0 ? nCost.ToString() : "MAX";
        strLabel = DrsStringUtils.Replace( strLabel, "COST", strCost );

        Cost.text = strLabel;
    }

    //////////////////////////////////////////
    /// UpdateView()
    //////////////////////////////////////////
    public override void UpdateView() {
        UpdateName();
        UpdateCost();
    }

    //////////////////////////////////////////
    /// OnClick()
    /// Callback for when the user clicks a
    /// perk.
    //////////////////////////////////////////
    public void OnClick() {
        // train the perk
        PlayerModel model = (PlayerModel) ModelToView;
        model.TrainPerk( Data.ID );
    }
}