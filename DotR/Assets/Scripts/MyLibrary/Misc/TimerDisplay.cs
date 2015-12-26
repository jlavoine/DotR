using UnityEngine;
using System.Collections;

//////////////////////////////////////////
/// TimerDisplay
/// Class that shows a "blah blah in X
/// seconds" message as a timer.
//////////////////////////////////////////

public class TimerDisplay : Timer {
	// text mesh to do the displaying
	private TextMesh m_textDisplay;

	// string that will display the time
	private string m_strText;

	//////////////////////////////////////////
	/// Init()
	//////////////////////////////////////////
	public virtual void Init( float i_fTime, string i_strText ) {
		base.Init(i_fTime);

		// set some variables
		m_textDisplay = GetComponent<TextMesh>();
		m_strText = i_strText;
	}

	//////////////////////////////////////////
	/// Update()
	//////////////////////////////////////////
	protected override void Update() {
		if (!ShouldUpdate())
			return;

		// update our text based on the time
		float fTime = GetTime();
		string strTime = OutReadableTime(fTime);
		// create the label based on the label this timer should be using, with the time substituted in
		string strLabel = DrsStringUtils.Replace(m_strText, DrsStringUtils.NUM, strTime);

		m_textDisplay.SetText(strLabel);

		// call parent
		base.Update();
	}

	//////////////////////////////////////////
	/// OutReadableTime()
	/// Basically just takes the time remaining
	/// and puts it in a string.  This should
	/// get more complex if we want to output
	/// things in minutes or hours.
	//////////////////////////////////////////
	public static string OutReadableTime( float i_fTime ) {
		//format the floating point seconds to minutes:seconds
		int nTime = Mathf.RoundToInt(i_fTime);
		int nMinutes = nTime / 60;
		int nSeconds = nTime % 60;
		string strTime = nMinutes.ToString() + ":";
		
		if (nSeconds > 9)
			strTime = strTime + nSeconds.ToString();
		else 
			strTime = strTime + "0" + nSeconds.ToString();

		return strTime;
	}

	//////////////////////////////////////////
	/// CreateTimerDisplay()
	/// Static function that will init a
	/// timer display off of an incoming
	/// game object.  For convenience.
	//////////////////////////////////////////
	public static void CreateTimerDisplay( GameObject i_go, float i_fTime, string i_strText ) {
		TimerDisplay timer = i_go.GetComponent<TimerDisplay>();
		if (timer)
			timer.Init(i_fTime, i_strText);
		else
			Debug.Log("No such timer on " + i_go.name, i_go);
	}
}
