using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

//////////////////////////////////////////
/// IDL_Effects
/// Loader for effects.
//////////////////////////////////////////

public abstract class IDL_Effects {
    // dictionary of data
    private static Dictionary<string, EffectData> m_dictData;

    //////////////////////////////////////////
    /// GetData()
    /// Returns an effect's data by ID.
    //////////////////////////////////////////
    public static EffectData GetData( string i_strKey ) {
        // if our dictionary of data is null, we must load it
        if ( m_dictData == null ) {
            m_dictData = new Dictionary<string, EffectData>();
            DataUtils.LoadData<EffectData>( m_dictData, "Effects" );
        }

        // get the data and send an error if it's null
        EffectData data = null;
        if ( m_dictData.ContainsKey( i_strKey ) )
            data = m_dictData[i_strKey];
        else
            Debug.LogError( "Looking for effect data with key " + i_strKey + " but not found!" );

        return data;
    }
}
