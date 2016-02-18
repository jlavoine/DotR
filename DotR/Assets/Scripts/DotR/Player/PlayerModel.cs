using UnityEngine;
using Newtonsoft.Json;
using System.Collections.Generic;

//////////////////////////////////////////
/// PlayerModel
/// Temporary(?) class that will represent
/// a player.
//////////////////////////////////////////

public class PlayerModel : DefaultModel {

    //////////////////////////////////////////
    /// Awake()
    //////////////////////////////////////////
    void Awake () {
        // get the temp save data
        string strData = DataUtils.LoadFile( "Player.json" );

        // turn it into player data
        PlayerData data = JsonConvert.DeserializeObject<PlayerData>( strData );

        // set various properties based on our data
        SetProperty( "XP", data.XP );
        SetProperty( "Perks", data.Perks );

        // send out a message about all the perks the player has
        foreach ( KeyValuePair<string,int> pair in data.Perks ) {
            //Messenger.Broadcast<string, int>( "SetPerk", pair.Key, pair.Value );
        }
    }

    //////////////////////////////////////////
    /// Save()
    //////////////////////////////////////////
    private void Save() {
        // get the temp save data
        string strData = DataUtils.LoadFile( "Player.json" );

        // turn it into player data
        PlayerData data = JsonConvert.DeserializeObject<PlayerData>( strData );

        // save out the values we changed...this is a hack...what's a more elegant way?
        data.XP = GetPropertyValue<int>( "XP" );
        data.Perks = GetPropertyValue<Dictionary<string, int>>( "Perks" );

        // save the file
        DataUtils.SaveFile( "Player.json", data );
    }

    //////////////////////////////////////////
    /// Debug_Reset()
    /// For testing, resets the player to the
    /// default value.
    //////////////////////////////////////////
    public void Debug_Reset() {
        SetProperty( "XP", 1000 );
        SetProperty( "Perks", new Dictionary<string, int>() );

        Save();
    }

    //////////////////////////////////////////
    /// GetCurrentXP()
    //////////////////////////////////////////
    public int GetCurrentXP() {
        int nXP = GetPropertyValue<int>( "XP" );
        return nXP;
    }

    //////////////////////////////////////////
    /// GetPerkLevel()
    /// Returns this player's level for the
    /// incoming perk.
    //////////////////////////////////////////
    public int GetPerkLevel( string i_strKey ) {
        Dictionary<string, int> dictPerks = GetPropertyValue<Dictionary<string, int>>( "Perks" );
        int nLevel = 0;
        if ( dictPerks.ContainsKey( i_strKey ) ) {
            nLevel = dictPerks[i_strKey];
        }

        return nLevel;
    }

    //////////////////////////////////////////
    /// CanTrainPerk()
    /// Returns whether or not the player
    /// can train the incoming perk.
    //////////////////////////////////////////
    public bool CanTrainPerk( string i_strKey ) {
        // get the player's perks
        Dictionary<string, int> dictPerks = GetPropertyValue<Dictionary<string, int>>( "Perks" );

        // get the perk data
        PerkData dataPerk = IDL_Perks.GetData( i_strKey );
        if ( dataPerk != null ) {
            // go through all the requirements and make sure the player has the perks at that level
            foreach ( KeyValuePair<string, int> req in dataPerk.PerkRequirements ) {
                // the player doesn't even have required perk? fail
                if ( dictPerks.ContainsKey( req.Key ) == false )
                    return false;

                // the player has the required perk but not at the required level? fail
                if ( dictPerks[req.Key] < req.Value )
                    return false;
            }
        }

        return true;
    }

    //////////////////////////////////////////
    /// TrainPerk()
    //////////////////////////////////////////
    public void TrainPerk( string i_strKey ) {
        // get the perk's data
        PerkData dataPerk = IDL_Perks.GetData( i_strKey );
        if ( dataPerk != null ) {
            // make sure the player can train the perk and the perk has another level to train
            int nLevel = GetPerkLevel( i_strKey );
            int nCost = dataPerk.GetCostToTrain( nLevel );
            int nXP = GetCurrentXP();

            if ( nCost > 0 && nXP >= nCost ) {
                // update player's xp
                int nNewXP = nXP - nCost;
                SetProperty( "XP", nNewXP );

                // update the perk's level
                Dictionary<string, int> dictPerks = GetPropertyValue<Dictionary<string, int>>( "Perks" );
                dictPerks[i_strKey] = nLevel + 1;
                SetProperty( "Perks", dictPerks );

                Save();
            }
        }
    }
}
