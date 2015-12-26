using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

//////////////////////////////////////////
/// AudioManager
/// Singleton manager in charge of playing
/// audio.  Right now there is no pause
/// functionality.
//////////////////////////////////////////

public class AudioManager : Singleton<AudioManager>{
	///////////////////////////////////////////
	/// PlayClip()
	/// Plays a sound with the name strClip
	/// from resources.
	///////////////////////////////////////////	
	public DrsAudioSource PlayClip( string strResource, Hashtable i_hashOptional = null ) {
		if ( i_hashOptional == null )
			i_hashOptional = new Hashtable();

		if ( strResource == "" ) {
			Debug.LogError("Something trying to play a sound with an empty sound id...");
			return null;
		}

		// instantiate and play the actual sound
		GameObject soundObject = new GameObject("Sound: " + strResource); 
		DrsAudioSource soundSource = soundObject.AddComponent<DrsAudioSource>();
		soundSource.Init( strResource, transform, i_hashOptional );
		
		return soundSource;	
	}
	public IEnumerator PlayClipAndWait( string strResource, Hashtable i_hashOptional = null ) {
		DrsAudioSource soundSource = PlayClip( strResource, i_hashOptional );
		yield return new WaitForSeconds( soundSource.GetLifetime() -.1f );
	}
}
