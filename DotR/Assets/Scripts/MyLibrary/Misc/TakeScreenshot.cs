using UnityEngine;
using System.Collections;

//////////////////////////////////////////
/// TakeScreenshot()
/// Simple script designed to let us take
/// screenshots of the games from a PC
/// build.  Simply attach this to the
/// DataManager and hit "s" to take a 
/// screenshot.
//////////////////////////////////////////

public class TakeScreenshot : MonoBehaviour {
	// screenshot count (this reset each time the game is started, so old screenshots will get overriden...)
	private int m_nCount = 0;
	
	//////////////////////////////////////////
	/// Update()
	//////////////////////////////////////////
	void Update () {
		if ( Input.GetKeyDown(KeyCode.S) ) {
			Debug.Log("Taking screenshot #" + m_nCount);
			Application.CaptureScreenshot( "Screenshot_" + m_nCount + ".png");

			m_nCount++;
		}
	}
}
