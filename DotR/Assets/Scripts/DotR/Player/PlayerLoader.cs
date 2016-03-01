using Newtonsoft.Json;

//////////////////////////////////////////
/// PlayerLoader
/// Created this class as a single point
/// for loading player data.
//////////////////////////////////////////

public class PlayerLoader  {
    public static PlayerData LoadPlayer() {
        // get the temp save data
        string strData = DataUtils.LoadFile( "Player.json" );

        // turn it into player data
        PlayerData data = JsonConvert.DeserializeObject<PlayerData>( strData );

        return data;
    }
}
