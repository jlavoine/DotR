using UnityEngine;
using System.Collections;

public class Persist : MonoBehaviour {

	////////////////////////////////
	/// Start()
	////////////////////////////////
	void Start() {
		DontDestroyOnLoad( gameObject );
	}
}
