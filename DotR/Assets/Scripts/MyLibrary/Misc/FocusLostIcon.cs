using UnityEngine;
using System.Collections;

//////////////////////////////////////////
/// FocusLost
/// Script that enables/disabled a
/// gameobject based on the focus state of
/// Unity.
//////////////////////////////////////////

public class FocusLostIcon : Singleton<FocusLostIcon> {
	// the object that will be shown/hidden based on focus
	public GameObject FocusLostObject;

	// if this is marked true, this object will destory itself on dev builds
	public bool DestroyOnDev;

	// number of times the app has lost focus this session
	private int m_nLostFocusCount = 0;
	public int GetLostFocusCount() {
		return m_nLostFocusCount;
	}

	// total time (in seconds) the app's focus has been lost
	private float m_fLostFocusTime;
	public string GetLostFocusTime() {
		string strTime = TimerDisplay.OutReadableTime(m_fLostFocusTime);
		return strTime;
	}

	//////////////////////////////////////////
	/// Start()
	//////////////////////////////////////////
	void Start () {
		if ( DestroyOnDev && UnityEngine.Debug.isDebugBuild )
			Destroy( gameObject );

		// this object will persist forever
		DontDestroyOnLoad(this);

		// set off by default
		FocusLostObject.SetActive(false);
	}

	//////////////////////////////////////////
	/// Update()
	//////////////////////////////////////////
	void Update () {
		// if the game is not focused, add to the time
		if ( IsGameFocused() == false ) 
			m_fLostFocusTime += Time.deltaTime;
	}

	//////////////////////////////////////////
	/// OnApplicationFocus()
	/// Unity callback when the game loses
	/// or regains focus.
	//////////////////////////////////////////
	private void OnApplicationFocus(bool focusState) {
		// send a message about focus gained/lost
		Messenger.Broadcast<bool>("FocusState", focusState);

		// set the object's active state to true or false based on focus of the game
		if(!focusState) {
			// app has lost focus, so turn our object on
			FocusLostObject.SetActive(true);

			// increment the total number of times the app has lost focus...big brother is watching
			m_nLostFocusCount++;
		}
		else {
			// app has regained focus
			FocusLostObject.SetActive(false);

			// little bit of a hack because Unity is doing something weird at startup...it is losing/regaining focus instantly
			if ( m_fLostFocusTime == 0 )
				m_nLostFocusCount--;
		}
	}

	//////////////////////////////////////////
	/// IsGameFocused()
	/// Returns whether or not the game has
	/// focus.
	//////////////////////////////////////////
	public bool IsGameFocused() {
		// the app is focused if the lost object is NOT active
		bool bFocused = !FocusLostObject.activeInHierarchy;
		return bFocused;
	}
}
