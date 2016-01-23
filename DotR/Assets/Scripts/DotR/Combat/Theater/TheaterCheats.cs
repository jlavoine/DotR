using UnityEngine;
using System.Collections;

public class TheaterCheats : MonoBehaviour {

	// Update is called once per frame
	void Update () {
        if ( Input.GetKeyDown( KeyCode.Alpha1 ) ) {
            Debug.Log( "InitCombat" );
            Messenger.Broadcast( "InitCombat" );
        }
        else if ( Input.GetKeyDown( KeyCode.Alpha2 ) ) {
            Debug.Log( "ReturnToStart" );
            Messenger.Broadcast( "ReturnToStart" );
        }
	}
}
