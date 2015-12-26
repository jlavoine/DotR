using UnityEngine;
using System.Collections;

public class ClickToDismiss : MonoBehaviour {

	public string key;

	// Update is called once per frame
	void Update () {
		if((Input.inputString != null) && (Input.inputString.Length > 0)) {
			Messenger.Broadcast<string>("PopupDismissed", key);
			Destroy (gameObject);
		}
	}

	void OnMouseUpAsButton() {
		Messenger.Broadcast<string>("PopupDismissed", key);
		Destroy (gameObject);
	}

}
