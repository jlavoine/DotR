using UnityEngine;
using System.Collections;

//////////////////////////////////////////
/// SD_Message
/// Special class for sending Unity
/// messages to the server logs.
//////////////////////////////////////////

public class SD_Message {

	public string Message;

	public SD_Message( string i_strMsg ) {
		Message = 	"------- Begin Unity Log Message -------" + "\n" +
					"Error: " + i_strMsg + "\n" +
					"------- End Unity Log Message ---------";
	}
}
