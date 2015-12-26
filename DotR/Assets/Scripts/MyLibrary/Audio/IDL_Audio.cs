using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;

//////////////////////////////////////////
/// IDL_Audio
/// Immutable data loader for all audio.
/// Audio can play without immutable data
/// for it, but the immutable data allows
/// standard properties, like volume, to
/// be overriden.
//////////////////////////////////////////

public class IDL_Audio {
	// dictionary of all audio data
	private static Dictionary<string, ID_Audio> m_dictData;
	
	//////////////////////////////////////////
	/// GetData()
	//////////////////////////////////////////
	public static ID_Audio GetData( string i_strKey ) {
		// if the hash isn't set up yet, we need to do that
		if (m_dictData == null)
			SetUpData();
		
		// data for the variable to be returned
		ID_Audio data = null;
		
		// search for the proper rollout data...
		if(m_dictData.ContainsKey(i_strKey))
			data = m_dictData[i_strKey];

		// there is no else becase it's fine if there is no audio data
		
		return data;
	}
	
	//////////////////////////////////////////
	/// SetUpData()
	//////////////////////////////////////////
	public static void SetUpData(){
		m_dictData = new Dictionary<string, ID_Audio>();
		
		//Load all data xml files
		UnityEngine.Object[] files = Resources.LoadAll("Audio", typeof(TextAsset));
		foreach(TextAsset file in files){
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
				Debug.Log("An override audio file was found: " + strFile);
				xmlString = System.IO.File.ReadAllText( strFile );
			}
			#endif
			
			//Go through all child node of xmlElement (the parent of the file)
			for(int i=0; i<xmlElement.Children.Count; i++){
				IXMLNode childNode = xmlElement.Children[i];
				
				// Get list of children elements
				List<IXMLNode> listChildren = XMLUtils.GetChildrenList(childNode);				
				
				//Get id
				Hashtable hashAttr = XMLUtils.GetAttributes(childNode);
				string strID = (string)hashAttr["ID"];
				string strError = strErrorFile + "(" + strID + "): ";
				
				ID_Audio data = new ID_Audio( strID, hashAttr, listChildren, strError );
				
				// store the data
				if ( m_dictData.ContainsKey( strID ) )
					Debug.LogError(strError + "Duplicate audio data!");
				else {
					// add data to dictionary
					m_dictData.Add(strID, data);	
				}
			}
		}
	}
}

