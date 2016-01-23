using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;

//////////////////////////////////////////
/// IDL_ProtoCharacters
/// Loader for prototype characters.
//////////////////////////////////////////

public static class IDL_ProtoCharacters {
    // data
    private static List<ProtoCharacterData> m_listCharacters;

    //////////////////////////////////////////
    /// GetCharacter()
    /// Returns character data for the 
    /// incoming name.
    //////////////////////////////////////////
    public static ProtoCharacterData GetCharacter(string i_strName) {
        if (m_listCharacters == null)
            LoadCharacters();

        foreach (ProtoCharacterData charData in m_listCharacters) {
            if ( charData.Name == i_strName ) {
                return charData;
            }
        }

        return null;
    }

    private static void LoadCharacters() {
        string strData = DataUtils.LoadFile("protocharacters.json");

        m_listCharacters = JsonConvert.DeserializeObject<List<ProtoCharacterData>>(strData);

        //JsonSerializerSettings settings = new JsonSerializerSettings();
        //settings.TypeNameHandling = TypeNameHandling.All;
        //settings.TypeNameAssemblyFormat = System.Runtime.Serialization.Formatters.FormatterAssemblyStyle.Full;
        //string test = JsonConvert.SerializeObject( m_listCharacters, Formatting.Indented, settings );
        //Debug.Log( test );
    }
}
