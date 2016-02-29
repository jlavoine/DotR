using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

//////////////////////////////////////////
/// IDL_Abilities
/// Loader for abilities.
//////////////////////////////////////////

public abstract class IDL_Abilities {
    // dictionary of data
    private static Dictionary<string, AbilityData> m_dictData;

    //////////////////////////////////////////
    /// GetData()
    /// Returns an ability's data by ID.
    //////////////////////////////////////////
    public static AbilityData GetData( string i_strKey ) {
        // if our dictionary of data is null, we must load it
        if ( m_dictData == null ) {
            m_dictData = new Dictionary<string, AbilityData>();
            DataUtils.LoadData<AbilityData>( m_dictData, "Abilities" );
        }

        // get the data and send an error if it's null
        AbilityData data = null;
        if ( m_dictData.ContainsKey( i_strKey ) )
            data = m_dictData[i_strKey];
        else
            Debug.LogError( "Looking for ability data with key " + i_strKey + " but not found!" );

        return data;
    }
}
