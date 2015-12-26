using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using System.Text.RegularExpressions;
using UnityEngineInternal;

//////////////////////////////////////////
/// Debug
/// Overriding the debug class so we can
/// do things like output to web console
/// or turn on/off certain types of messages.
/// Got this from the interwebs:
/// http://forum.unity3d.com/threads/38720-Debug-Log-and-needless-spam
//////////////////////////////////////////

public static class Debug {
	public static void Log (object message, string i_strType = null) { 
		// if the incoming type is not null, check to see if we want to display this message
		if (ShouldNotDisplay(i_strType))
			return;

		if (Application.isWebPlayer)
			Application.ExternalCall("log_from_unity", message.ToString());

		UnityEngine.Debug.Log (message);
	}
	
	public static void Log (object message, UnityEngine.Object context, string i_strType = null) {  
		// if the incoming type is not null, check to see if we want to display this message
		if (ShouldNotDisplay(i_strType))
			return;

		if (Application.isWebPlayer)
			Application.ExternalCall("log_from_unity", message.ToString());

		UnityEngine.Debug.Log (message, context);
	}

	private static bool ShouldNotDisplay(string i_strType = null) {
		bool bShouldNot = false;

		// if the type is non-null...
		if (i_strType != null) {
			// get the list of types we are allowed to display
			List<string> listTypes = Constants.GetConstant<List<string>>("DebugTypes");

			// if the type is not in the list, sorry!
			if (listTypes.Contains(i_strType) == false)
				bShouldNot = true;
		}

		return bShouldNot;
	}

	public static void LogError (object message) {   
		UnityEngine.Debug.LogError (message);

		SendMessageToServer( message );
	}
	
	public static void LogError (object message, UnityEngine.Object context) {  
		UnityEngine.Debug.LogError (message, context);

		SendMessageToServer( message );
	}

	//////////////////////////////////////////
	/// SendMessageToServer()
	/// Sends a message to be output in the
	/// server's logs.
	//////////////////////////////////////////
	private static void SendMessageToServer( object message ) {
		// create the special message object to be sending
		//SD_Message messageToPost = new SD_Message( (string) message );

		// only bother doing this if we aren't on a debug build
		//if ( DatabaseManager.Instance && UnityEngine.Debug.isDebugBuild == false ) {
			//string strMessageURL = Constants.GetConstant<string>( "URL_LogMessage" );
			//DatabaseManager.Instance.StartCoroutine( DatabaseUtils.SendPost<SD_Message>(messageToPost, strMessageURL, (retPost) => {}, 1, false));
		//}
	}
	
	public static void LogWarning (object message) {  
		UnityEngine.Debug.LogWarning (message.ToString ());
	}
	
	public static void LogWarning (object message, UnityEngine.Object context) {   
		UnityEngine.Debug.LogWarning (message.ToString (), context);
	}
}