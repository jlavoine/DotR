using UnityEngine;
using System.Collections;

///////////////////////////////////////////
/// DrsStringUtils
/// Utility functions for formatting/localizing
/// strings.
///////////////////////////////////////////

public class DrsStringUtils {
	
	public static string NUM = "NUM";
	public static string NUM_2 = "NUM2";
	public static string NUM_3 = "NUM3";
	public static string TIME = "TIME";
	public static string NAME = "NAME";
	
	///////////////////////////////////////////
	// Replace()
	// Replaces a tag in a string with a value.
	///////////////////////////////////////////	
	public static string Replace( string i_str, string i_strKey, string i_strVal ) {
		string str = i_str;
		string key = "$" + i_strKey + "$";	
		string strResult = str.Replace(key, i_strVal);

        // this is kind of a hack, but I want to replace all newlines because Unity doesn't do this...
        strResult = strResult.Replace( "\\n", "\n" );
		
		return strResult;
	}
	
	public static string Replace( string i_str, string i_strKey, int i_nVal ) {
		string strVal = FormatNumber( i_nVal );
		return DrsStringUtils.Replace( i_str, i_strKey, strVal );
	}

	///////////////////////////////////////////
	// Replace()
	// Replaces a tag in a string with a value.
	///////////////////////////////////////////	
	public static string FormatNumber( int i_nVal ) {
		string strDelim = StringTableManager.Get( "NUMBER_DELIMETER" );
		string strVal = i_nVal.ToString("n0");
		
		strVal = strVal.Replace(",", strDelim);
		
		return strVal;
	}
}
