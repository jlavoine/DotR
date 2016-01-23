using Newtonsoft.Json;

//////////////////////////////////////////
/// SerializationUtils
/// Misc utilities for serialization.
//////////////////////////////////////////

public static class SerializationUtils {

    //////////////////////////////////////////
    /// Serialize()
    /// Takes an object and turns it into a
    /// json string with some standard settings.
    //////////////////////////////////////////
    public static string Serialize( object o ) {
        JsonSerializerSettings settings = new JsonSerializerSettings();
        settings.TypeNameHandling = TypeNameHandling.All;
        settings.TypeNameAssemblyFormat = System.Runtime.Serialization.Formatters.FormatterAssemblyStyle.Full;
        string json = JsonConvert.SerializeObject( o, Formatting.Indented, settings );

        return json;
    }
}
