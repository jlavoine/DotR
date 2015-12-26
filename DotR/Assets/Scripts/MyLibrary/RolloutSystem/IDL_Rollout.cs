using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;

//////////////////////////////////////////
/// IDL_Rollout
/// Immutable data loader for all variables
/// that may roll out.
//////////////////////////////////////////

public class IDL_Rollout {
	// hashtable of all rollout data -- it is a hash of hashes
	private static Hashtable m_hashData;
	
	//////////////////////////////////////////
	/// GetDataForSession()
	//////////////////////////////////////////
	public static T GetDataForSession<T>( string i_strKey, int i_nSession ) {
		// if the hash isn't set up yet, we need to do that
		if (m_hashData == null)
			SetUpData();

		// data for the variable to be returned
		T data = default(T);

		// search for the proper rollout data...
		if(m_hashData.ContainsKey(i_strKey)) {
			// if the uppermost hash contains the variable key, we now need to get that hash and look for the session key
			Hashtable hashVariable = (Hashtable) m_hashData[i_strKey];
			if ( hashVariable.ContainsKey(i_nSession) ) {
				// now, if the variable hash contains the session key, let's use that data
				data = (T) hashVariable[i_nSession];
			}
			else {
				// otherwise the variable hash didn't have they session key -- which may be fine, just use the base
				if ( hashVariable.ContainsKey("Base") ) 
					data = (T) hashVariable["Base"];
				else
					Debug.Log("No base value for rollout data " + i_strKey + " and session " + i_nSession + " had no data.");
			}
		}
		else
			Debug.LogError("No such rollout data for key " + i_strKey);
		
		return data;
	}
	
	//////////////////////////////////////////
	/// SetUpData()
	//////////////////////////////////////////
	public static void SetUpData(){
		m_hashData = new Hashtable();
		
		//Load all data xml files
		UnityEngine.Object[] files = Resources.LoadAll("RolloutData", typeof(TextAsset));
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
				Debug.Log("An override constants file was found: " + strFile);
				xmlString = System.IO.File.ReadAllText( strFile );
			}
			#endif
			
			//Go through all child node of xmlElement (the parent of the file)
			for(int i=0; i<xmlElement.Children.Count; i++){
				IXMLNode childNode = xmlElement.Children[i];
				
				// Get list of children elements
				//List<IXMLNode> listChildren = XMLUtils.GetChildrenList(childNode);				

				// each child is one set of rollout data
				CreateRolloutData( childNode, strErrorFile);
			}
		}
	}

	//////////////////////////////////////////
	/// CreateRolloutData()
	/// Creates the hashtable for a given
	/// rollout variable as defined in i_node.
	//////////////////////////////////////////
	private static void CreateRolloutData( IXMLNode i_node, string i_strError ) {
		// get attributes of the data
		Hashtable hashAttr = XMLUtils.GetAttributes(i_node);
		
		string strID = (string)hashAttr["ID"];		// the id of the data; i.e. the variable name to be accessed
		string strType = (string)hashAttr["Type"];	// type of the value of this variable; int, string, etc
		string strBase = (string)hashAttr["Base"];	// the base value of the variable
		
		i_strError += "(" + strID + "): ";

		// now that we have our base data, create the hash for this variable
		Hashtable hashRollout = new Hashtable();
		m_hashData[strID] = hashRollout;

		// now get the individual pieces of rollout data
		List<IXMLNode> listData = XMLUtils.GetChildrenList(i_node);

		//Debug.Log("Loading rollout data for " + strID);

		// now we want to load all these individual pieces of data into our newly created hash based on type
		switch ( strType ) {
			case "Int":
				LoadRolloutData<int>( hashRollout, strBase, listData, i_strError );
				break;
			case "String":
				LoadRolloutData<string>( hashRollout, strBase, listData, i_strError );
				break;
			case "IntList":
				LoadRolloutData<string>( hashRollout, strBase, listData, i_strError );
				ConvertData<int>( strID, hashRollout );
				break;
			case "Bool":
				LoadRolloutData<bool>( hashRollout, strBase, listData, i_strError );
				break;
			default:
				Debug.Log(i_strError + "Yikes...no way to handle loading rollout data for the following type: " + strType);
				break;
		}
	}

	//////////////////////////////////////////
	/// LoadRolloutData()
	/// This function actually loads the
	/// various rollout data into the hash
	/// responsible for storing the data.
	//////////////////////////////////////////
	private static void LoadRolloutData<T>( Hashtable i_hash, string i_strBase, List<IXMLNode> i_listData, string i_strError ) {
		// get the converter for this type
		TypeConverter converter = TypeDescriptor.GetConverter(typeof(T));

		// set the base val
		T baseVal = (T)converter.ConvertFromString(i_strBase);
		i_hash["Base"] = baseVal;
		//Debug.Log("Bae value is " + baseVal);

		// now loop through all the individual pieces of rollout data
		for ( int i = 0; i < i_listData.Count; ++i ) {
			IXMLNode node = i_listData[i];
			Hashtable hashAttr = XMLUtils.GetAttributes(node);

			// get the info we need to build our rollout data
			int nMin = int.Parse((string)hashAttr["Min"]);	// min level for this value
			int nMax = int.Parse((string)hashAttr["Max"]);	// max level for this value
			string strVal = (string)hashAttr["Value"];		// the actual value between the min/max
			T val = (T)converter.ConvertFromString(strVal);

			// do some error checking
			if ( nMax < nMin ) {
				Debug.Log(i_strError + "Max(" + nMax + ") < min(" + nMin + ")");
				continue;
			}

			// if no error there, now go through and add the value to the hash at the appropriate levels
			for ( int j = nMin; j <= nMax; ++j ) {
				if ( i_hash.ContainsKey(j) )
					Debug.Log(i_strError + "Already contains value for " + j + " -- it will be overwritten");

				i_hash[j] = val;

				//Debug.Log(j + ": " + val);
			}
		}
	}

	//////////////////////////////////////////
	/// ConvertData()
	/// For rollout data that is a list,
	/// because I couldn't figure out how to
	/// write a converter for it, this function
	/// will convert the string value to a list
	/// of T.
	//////////////////////////////////////////
	private static void ConvertData<T>( string i_strID, Hashtable i_hash ) {
		// this new hash will replace i_hash for i_strID
		Hashtable hash = new Hashtable();

		// loop through every entry in the old hash
		foreach (DictionaryEntry pair in i_hash) {
			// get the pair and value
			object key = pair.Key;				// this needs to be obj because keys could be ints or strings
			string strVal = (string)pair.Value;

			// then create a list of T from the string value we loaded from xml
			List<T> list = Constants.ParseList<T>( strVal );
			hash[key] = list;
		}

		// set the key to our new hash
		m_hashData[i_strID] = hash;
	}
}

