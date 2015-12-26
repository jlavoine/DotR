using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof(Text))]
public class SetText : MonoBehaviour {
	public string Key;

	///////////////////////////////////////////
	/// Awake()
	///////////////////////////////////////////
	void Awake() {
		string strText = StringTableManager.Get( Key );
		Text text = GetComponent<Text>();
		text.text = strText;
	}
}
