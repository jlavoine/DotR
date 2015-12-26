using UnityEngine;
using System.Collections;

///////////////////////////////////////////
/// DestroyThis
/// Attach this script on any kind of
/// game object and give it a time, and it
/// will destroy itself.
///////////////////////////////////////////

public class DestroyThis : MonoBehaviour {
	// has the script had its life set yet
	private bool m_bSet = false;
	private bool IsSet() {
		return m_bSet;
	}
	
	// the time, in seconds, that the object should persist
	private float m_fLife = 0;
	public float m_fStartLife = 0;
	public void SetLife( float i_float ) {
		if ( IsSet() ) {
			Debug.LogError("DestroyThis script getting its life set more than once; please check this out.");
			return;
		}
		
		m_fLife = i_float;
		m_bSet = true;
	}

	// is the script paused?
	private bool m_bPaused = false;
	private bool IsPaused() {
		return m_bPaused;
	}
	public void SetPause(bool b_pause) {
		m_bPaused = b_pause;
	}

	///////////////////////////////////////////
	/// Start()
	///////////////////////////////////////////	
	void Start() {
		// shortcut -- if the start life is negative, it means just destroy this object right away.
		// used as a way to keep things from getting into the build that aren't totally ready
		if ( m_fStartLife < 0 ) {
			Destroy( gameObject );
			return;
		}
		
		// life may be set on the script itself
		if ( !IsSet() && m_fStartLife > 0 )
			SetLife( m_fStartLife );
	}

	///////////////////////////////////////////
	/// Update()
	///////////////////////////////////////////	
	void Update() {
		// if life has been set, let the countdown begin
		if ( IsSet() && !IsPaused() ) {
			float fDelta = Time.deltaTime;
			
			m_fLife -= fDelta;
			
			if ( m_fLife <= 0 )
				Destroy(gameObject);
		}
	}
}
