using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//////////////////////////////////////////
/// ID_Audio
/// Immutable data that is an individual
/// audio.
/// Audio can play without immutable data
/// for it, but the immutable data allows
/// standard properties, like volume, to
/// be overriden.
//////////////////////////////////////////

public class ID_Audio {
	// volume override
	private float m_fVolume = 1;
	public float GetVolume() {
		return m_fVolume;
	}

	// should this sound loop?
	private bool m_bLoop = false;
	public bool ShouldLoop() {
		return m_bLoop;
	}

	//////////////////////////////////////////
	/// ID_SimilarityBucket()
	//////////////////////////////////////////
	public ID_Audio( string i_strID, Hashtable hashData, List<IXMLNode> listElements, string strError ) {
		if ( hashData.ContainsKey( "Volume" ) )
			m_fVolume = float.Parse(hashData["Volume"].ToString());

		if ( hashData.ContainsKey( "Loop" ) )
			m_bLoop = bool.Parse(hashData["Loop"].ToString());
	}	
}
