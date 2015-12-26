using UnityEngine;
using System.Collections;

///////////////////////////////////////////
/// DrsAudioSource
/// Custom audio source.
/// It is a layer on top of Unity's
/// standard audio source object.
///////////////////////////////////////////

public class DrsAudioSource : MonoBehaviour {
	// Unity's base AudioSource object
	private AudioSource audioSource;

	// lifetime of this sound
	private float m_fLifetime;
	public float GetLifetime() {
		return m_fLifetime;
	}

	///////////////////////////////////////////
	/// Init()
	/// Configures the actual audio source and
	/// plays the sound.
	///////////////////////////////////////////	
	public void Init( string strResource, Transform tf, Hashtable i_hashOptional ) {		
		// load the clip
		AudioClip clip = Resources.Load(strResource) as AudioClip;
		
		if ( clip == null ) {
			Debug.LogError("No such sound clip for resource " + strResource);
			Destroy( gameObject );
			return;
		}
	
		// create the audio source	
		audioSource = gameObject.AddComponent<AudioSource>(); 
		audioSource.clip = clip; 

		// set the default volume (may be overriden)
		float fDefaultVolume = Constants.GetConstant<float>( "DefaultVolume" );
		audioSource.volume = fDefaultVolume;

		ID_Audio dataAudio = IDL_Audio.GetData( strResource );
		if ( dataAudio != null ) {
			audioSource.volume = dataAudio.GetVolume();
			audioSource.loop = dataAudio.ShouldLoop();
		}

		gameObject.transform.parent = tf;
		gameObject.transform.position = tf.position;
		audioSource.Play();

		// add destroy script -- if the audio doesn't loop
		if ( audioSource.loop == false ) {
			m_fLifetime = clip.length + 0.1f;
			if ( i_hashOptional.ContainsKey("Time") )
				m_fLifetime = (float)i_hashOptional["Time"];

			DestroyThis scriptDestroy = gameObject.AddComponent<DestroyThis>();
			scriptDestroy.SetLife( m_fLifetime );		
		}
	}
	
	///////////////////////////////////////////
	/// FadeOut()
	/// Fades out this sound over the incoming
	/// fTime, in seconds.
	///////////////////////////////////////////	
	public IEnumerator FadeOut( float fTime ) {
		for (int i = 9; i >= 0; i--) {
			audioSource.volume = i * .1f;
			yield return new WaitForSeconds(fTime / 10);
		}			
	}
}
