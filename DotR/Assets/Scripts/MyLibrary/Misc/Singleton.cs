using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

//////////////////////////////////////////
/// Singleton
/// Simple singleton class we can use
/// all projects at CRA.  Note that the way
/// in which we use singletons in Unity is
/// probably different in the way we would
/// use them elsewhere.  Singletons in Unity
/// are probably more scene-unique than
/// application unique.  We could change this
/// by not destroying singletons on load,
/// but then it would make it more difficult
/// to edit individual scenes.
//////////////////////////////////////////

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour {
	// the instance of this singleton
	protected static T m_instance;

	//////////////////////////////////////////
	/// Instance
	/// Returns the instance of this singleton
	/// if it exists, and instantiates and
	/// returns it if it does not.
	//////////////////////////////////////////
	public static T Instance {
		get {
			if(m_instance == null)
				m_instance = (T) FindObjectOfType(typeof(T));
			
			return m_instance;
		}
	}

	//////////////////////////////////////////
	/// OnDestroy()
	//////////////////////////////////////////
	void OnDestroy(){
		m_instance = null;
	}
}