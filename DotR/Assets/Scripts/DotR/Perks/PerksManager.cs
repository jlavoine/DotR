﻿using UnityEngine;
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
        //PerkData ex = PerkData.GetExample();
        //List<PlayerData> listEx = new List<PlayerData>();
        //listEx.Add( ex );
        //string json = SerializationUtils.Serialize( ex );
        //Debug.Log( json );
    }

}