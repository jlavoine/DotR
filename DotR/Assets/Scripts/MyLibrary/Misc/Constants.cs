using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;

///////////////////////////////////////////
/// Constants
/// Reads constants from an XML file.  If
/// there exists a constants file with the
/// same name in the build folder (stand
/// alone builds only) that file will be
/// used instead of the one in data.
///////////////////////////////////////////

public class Constants {
	// hash of all constants
	private static Hashtable hashConstants;
	
	///////////////////////////////////////////
	/// GetConstant()
	/// Returns the constant for the incoming
	/// key.  See AddData() for types.
	///////////////////////////////////////////	
	public static T GetConstant<T>( string strKey, T defaultVal ) {
		// if the data is not set it up, set it up
		if ( hashConstants == null )
			SetUpData();
		
		T data = defaultVal;
		if ( hashConstants.ContainsKey( strKey ) )
			data = (T) hashConstants[strKey];
		else
			Debug.LogError("No such constant for key " + strKey);
		
		return data;
	}
	public static T GetConstant<T>( string strKey ) {
		return GetConstant<T>(strKey, default(T));
	}
	
	///////////////////////////////////////////
	/// ParseVector3()
	/// Parses a vector3 from a string.
	///////////////////////////////////////////	
	public static Vector3 ParseVector3( string i_str ) {
		Vector3 vector = new Vector3(0,0,0);
		String[] arrayVector3 = i_str.Split(","[0]);
		if ( arrayVector3.Length == 3 ) {
			vector = new Vector3( 
			                     float.Parse( arrayVector3[0] ), 
			                     float.Parse( arrayVector3[1] ), 
			                     float.Parse( arrayVector3[2] ) );
		}	
		else
			Debug.LogError("Illegal vector3 parsing, reverting to 0,0,0");
		
		return vector;
	}

	///////////////////////////////////////////
	/// ParseStringList()
	/// Parses a string list from a string.
	///////////////////////////////////////////	
	public static List<string> ParseStringList( string i_str ) {
		List<string> list = new List<string>();
		String[] array = i_str.Split(","[0]);

		for (int i = 0; i < array.Length; ++i)
			list.Add(array[i]);
		
		return list;
	}
	public static List<T> ParseList<T>( string i_str ) {
		List<T> list = new List<T>();
		String[] array = i_str.Split(","[0]);
		TypeConverter converter = TypeDescriptor.GetConverter(typeof(T));
		
		for (int i = 0; i < array.Length; ++i) {
			T val = (T)converter.ConvertFromString(array[i]);
			list.Add(val);
		}
		
		return list;
	}
	
	///////////////////////////////////////////
	/// ParseColor()
	/// Parses a color from a string.
	///////////////////////////////////////////	
	private static Color ParseColor( string i_str ) {
		Color color = Color.white;
		String[] arrayColor = i_str.Split(","[0]);
		if ( arrayColor.Length == 4 ) {
			color = new Color( 
			                  float.Parse( arrayColor[0] ) / 255, 		// why unity's color picker is 0-255 but the code wants floats, I don't know...
			                  float.Parse( arrayColor[1] ) / 255, 
			                  float.Parse( arrayColor[2] ) / 255, 
			                  float.Parse( arrayColor[3] ) / 255 );
		}	
		else
			Debug.LogError("Illegal color parsing...reverting to white");
		
		return color;
	}
	
	///////////////////////////////////////////
	/// SetUpData()
	///////////////////////////////////////////	
	private static void SetUpData() {
		hashConstants = new Hashtable();
		
		//Load all item xml files
		UnityEngine.Object[] files = Resources.LoadAll("Constants", typeof(TextAsset));
		foreach(TextAsset file in files){
			string xmlString = file.text;
			
			// this will load files that have replaced the file stored in data locally
			#if UNITY_STANDALONE_WIN
			string strFile = Application.dataPath + "/Resources/" + file.name + ".xml";
			if ( System.IO.File.Exists( strFile ) )
			{
				Debug.Log("An override constants file was found: " + strFile);
				xmlString = System.IO.File.ReadAllText( strFile );
			}
			#endif
			
			//Create XMLParser instance
			XMLParser xmlParser = new XMLParser(xmlString);
			
			//Call the parser to build the IXMLNode objects
			XMLElement xmlElement = xmlParser.Parse();
			
			string strError = "Error in file " + file.name + ": ";
			
			//Go through all child node of xmlElement (the parent of the file)
			for(int i=0; i<xmlElement.Children.Count; i++) {
				IXMLNode childNode = xmlElement.Children[i];
				Hashtable hashAttr = XMLUtils.GetAttributes(childNode);
				
				AddData( strError, hashAttr );	
			}
		}				
	}
	
	///////////////////////////////////////////
	/// AddData()
	/// hashAttributes is essentially the data
	/// for one constants file.
	///////////////////////////////////////////	
	private static void AddData( string i_strError, Hashtable hashAttributes ) {
		if (!hashAttributes.Contains ("Name") || !hashAttributes.Contains ("Type") || !hashAttributes.Contains ("Value")) {
			// make sure that the attributes contain all the relevant info, or send an error
			Debug.LogError (i_strError + "Illegal constant in Constants.xml");
		}
		else {
			string strKey = (string) hashAttributes["Name"];
			string strType = (string) hashAttributes["Type"];
			string strValue = (string) hashAttributes["Value"];
			
			if ( hashConstants.Contains(strKey) )
				Debug.LogError(i_strError + "Repeat constant: " + strKey);
			
			// get the type and switch on it so we know what kind of value the constant is
			switch ( strType ) {
			case "String":
				hashConstants[ strKey ] = strValue;
				break;
			case "Int":
				hashConstants[ strKey ] = int.Parse( strValue );
				break;
			case "Bool":
				hashConstants[ strKey ] = bool.Parse( strValue );
				break;
			case "Float":
				hashConstants[ strKey ] = float.Parse( strValue );
				break;
			case "Color":
				hashConstants[ strKey ] = ParseColor( strValue );
				break;
			case "Vector3":
				hashConstants[ strKey ] = ParseVector3( strValue );				
				break;
			case "StringList":
				hashConstants[ strKey ] = ParseStringList( strValue );
				break;
			default:
				Debug.LogError("Illegal constant type for " + strKey);
				break;
			}
		}	
	}
}

