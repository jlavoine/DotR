using UnityEngine;
using System.Collections.Generic;
using Newtonsoft.Json;

//////////////////////////////////////////
/// PerksManager
/// Manager in charge of the level up
/// perks screen.
//////////////////////////////////////////

public class PerksManager : Singleton<PerksManager> {
    // for now, the player data to load
    public string PlayerID;

    void Start() {
        //PlayerData ex = PlayerData.GetExample();
        //List<PlayerData> listEx = new List<PlayerData>();
        //listEx.Add( ex );
        //string json = SerializationUtils.Serialize( ex );
        //Debug.Log( json );

        AbilityData test = IDL_Abilities.GetData( "Blessing_Regen" );
        Debug.Log( test.Name );
    }

}
