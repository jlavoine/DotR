using UnityEngine;
using System.Collections;
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

		string strPath = Application.streamingAssetsPath + "/" + i_strFile + ".json";
		if ( File.Exists( strPath ) ) {
			FileStream file = new FileStream( strPath, FileMode.Open, FileAccess.Read );
			StreamReader sr = new StreamReader( file );
			
			strData = sr.ReadToEnd();
			sr.Close();
			file.Close();
		}

		return strData;
	}
	
}
