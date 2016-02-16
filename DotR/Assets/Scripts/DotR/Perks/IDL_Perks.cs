using System.Collections.Generic;

//////////////////////////////////////////
/// IDL_Perks
/// Loader for perks.
//////////////////////////////////////////

public abstract class IDL_Perks {
    // dictionary of data
    private static Dictionary<string, PerkData> m_dictData;

    //////////////////////////////////////////
    /// GetData()
    /// Returns a perk's data by ID.
    //////////////////////////////////////////
    public static PerkData GetData( string i_strKey ) {
        // if our dictionary of data is null, we must load it
        if ( m_dictData == null ) {
            m_dictData = new Dictionary<string, PerkData>();
            DataUtils.LoadData<PerkData>( m_dictData, "Perks" );
        }

        // get the data and send an error if it's null
        PerkData data = null;
        if ( m_dictData.ContainsKey( i_strKey ) ) {
            data = m_dictData[i_strKey];
        }
        else
            Debug.LogError( "Looking for perk data with key " + i_strKey + " but not found!" );

        return data;
    }
}
