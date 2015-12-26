using UnityEngine;
using System.Collections;

//////////////////////////////////////////
/// MessengerHelper
/// Stick this on a game object in every
/// scene that uses messages -- it will
/// clean up the event table when the level
/// is destroyed.
//////////////////////////////////////////

public class MessengerHelper : MonoBehaviour {

	void OnDestroy() {
		Messenger.Cleanup();
	}
}
