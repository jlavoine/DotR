using UnityEngine;
using System.Collections;
using System.Collections.Generic;

///////////////////////////////////////////
// StringTable
// Parent class that localized tables
// derive from.
///////////////////////////////////////////

public class StringTable {
	// name of string table
	private string m_strName;
	public string GetName() {
		return m_strName;
	}

	private static Dictionary<string,string> m_dictStrings = new Dictionary<string,string>();

	public StringTable( string i_strName ) {
		m_strName = i_strName;
		SetUpData();
	}
	
	///////////////////////////////////////////
	// Get()
	// Returns the localized string of i_key
	// for this string table.  Or ??? if the
	// key cannot be found.
	///////////////////////////////////////////	
	public string Get( string i_strKey ) {
		string strResult = "???";
		
		if (m_dictStrings.ContainsKey(i_strKey) == false)
			Debug.Log("StringTable " + GetName() + " does not have key " + i_strKey);
		else
			strResult = m_dictStrings[i_strKey];

		return strResult;		
	}

	private void SetUpData() {
		string strName = GetName();
		UnityEngine.Object[] files = Resources.LoadAll("StringTables/" + strName, typeof(TextAsset));
		foreach(TextAsset file in files) {
			string xmlString = file.text;
			
			// error message
			string strErrorFile = "Error in file " + file.name;			
			
			//Create XMLParser instance
			XMLParser xmlParser = new XMLParser(xmlString);
			
			//Call the parser to build the IXMLNode objects
			XMLElement xmlElement = xmlParser.Parse();
			
			// this will load files that have replaced the file stored in data locally
			#if UNITY_STANDALONE_WIN
			string strFile = Application.dataPath + "/Resources/" + file.name + ".xml";
			if ( System.IO.File.Exists( strFile ) )
			{
				Debug.Log("An override string table file was found: " + strFile);
				xmlString = System.IO.File.ReadAllText( strFile );
			}
			#endif
			
			AddData( strErrorFile, xmlElement );				
		}		
	}	
	
	private void AddData( string i_strError, IXMLNode i_element ) {
		// i_element is the root element, English
		// each child of this element is an individual string key
		for ( int i = 0; i < i_element.Children.Count; ++i ) {
			IXMLNode entry = i_element.Children[i];
			
			Hashtable hashAttributes = XMLUtils.GetAttributes( entry );
			string strID = (string)hashAttributes["ID"];
			string strValue = XMLUtils.GetString( entry );
			
			if ( m_dictStrings.ContainsKey(strID) )
				Debug.Log(i_strError + "Duplicate string ID: " + strID);
			else
				m_dictStrings[strID] = strValue;
		}
	}		
}
