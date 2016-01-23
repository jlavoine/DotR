using UnityEngine;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.IO;

////////////////////////////////
/// DataUtils
/// Functions used to load data
/// from various json files.
////////////////////////////////
 
public static class DataUtils {

	public static string LoadFile( string i_strFile ) {
		string strData = "";

		string strPath = Application.streamingAssetsPath + "/" + i_strFile;
		if ( File.Exists( strPath ) ) {
			FileStream file = new FileStream( strPath, FileMode.Open, FileAccess.Read );
			StreamReader sr = new StreamReader( file );
			
			strData = sr.ReadToEnd();
			sr.Close();
			file.Close();
		}

		return strData;
	}

    //////////////////////////////////////////
    /// LoadFiles()
    /// Returns a list of strings from the
    /// streaming assets folder that are the
    /// files in the folder.
    //////////////////////////////////////////
    public static List<string> LoadFiles( string i_strFolder ) {
        // list of strings (representing the contents of each file in the folder)
        List<string> listContents = new List<string>();

        // get all the files in the incoming directory
        string strPath = Application.streamingAssetsPath + "/" + i_strFolder + "/";
        DirectoryInfo infoDirectory = new DirectoryInfo( strPath );
        FileInfo[] infoFiles = infoDirectory.GetFiles();

        // loop through and add the contents of the file to our list
        foreach ( FileInfo file in infoFiles ) {
            string strFilename = file.Name;

            // we only want non meta files!
            if ( strFilename.Contains( ".meta" ) )
                continue;

            // get the file's contents and add it to our list
            string strFile = LoadFile( i_strFolder + "/" + strFilename );
            listContents.Add( strFile );
        }

        return listContents;
    }

    //////////////////////////////////////////
    /// LoadData()
    /// Loads a generic dictionary of data
    /// from the streaming assets folder.
    //////////////////////////////////////////
    public static void LoadData<T>( Dictionary<string, T> i_dictData, string i_strFolder ) where T : GenericData {
        // get all files in the folder (their contents)
        List<string> listFiles = LoadFiles( i_strFolder );

        // loop through each files contents..
        foreach ( string strFile in listFiles ) {
            // and get the list of data from that file's contents
            List<T> listData = JsonConvert.DeserializeObject<List<T>>( strFile );

            // loop through each piece of data and add it to our dictionary
            foreach ( T data in listData ) {
                string strID = data.ID;
                i_dictData.Add( strID, data );
            }
        }
    }
}
