using UnityEngine;
using System.Collections;

//////////////////////////////////////////
/// CraLabel
/// Simple (potentially temporary?) label
/// script for setting a text mesh's text
/// to a string key.
//////////////////////////////////////////

public class CraLabel : MonoBehaviour {
	// the text mesh object for this label
	private TextMesh m_textmesh;

	// the string key for this label
	public string StringKey;

	//////////////////////////////////////////
	/// Start()
	//////////////////////////////////////////
	void Start() {
		// set the text mesh object
		m_textmesh = GetComponent<TextMesh>();

		// we got to have a text mesh!
		if (m_textmesh == null) {
			Debug.LogError("No text mesh for CraLabel " + gameObject.name, gameObject);
			return;
		}

		// set the label
		string strText = StringTableManager.Get(StringKey);
		m_textmesh.text = strText;
	}
}
