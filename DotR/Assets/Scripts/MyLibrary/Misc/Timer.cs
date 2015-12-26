using UnityEngine;
using System.Collections;

//////////////////////////////////////////
/// Timer
/// Simple class designed to just count
/// down and then send off a message. 
/// Could probably be made to count UP
/// at some point too.
//////////////////////////////////////////

public class Timer : MonoBehaviour {
	// has the timer been set yet?
	private TimerStates m_eState;

	// time to count from
	private float m_fTime;
	protected float GetTime() {
		return m_fTime;
	}

	//////////////////////////////////////////
	/// Init()
	//////////////////////////////////////////
	public virtual void Init( float i_fTime ) {
		m_fTime = i_fTime;
		m_eState = TimerStates.Ticking;
	}

	//////////////////////////////////////////
	/// Update()
	//////////////////////////////////////////
	protected virtual void Update() {
		if (!ShouldUpdate())
			return;

		m_fTime -= Time.deltaTime;

		if (m_fTime <= 0) {
			m_eState = TimerStates.Done;
			Messenger.Broadcast("TimerDone");
		}
	}

	//////////////////////////////////////////
	/// ShouldUpdate()
	/// Timers should only update if they are
	/// in the ticking state.
	//////////////////////////////////////////
	protected bool ShouldUpdate() {
		bool bShouldUpdate = m_eState == TimerStates.Ticking;
		return bShouldUpdate;
	}
}
