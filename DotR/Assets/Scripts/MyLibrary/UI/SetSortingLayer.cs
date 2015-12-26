using UnityEngine;
using System.Collections;

//////////////////////////////////////////
/// SetSortingLayer
/// Simple script that sets the sorting
/// layer of an object.
//////////////////////////////////////////

public class SetSortingLayer : MonoBehaviour {
	// layer this game object will be set to
	public string Layer;

	// order within the layer
	public int Order;

	//////////////////////////////////////////
	/// Start()
	//////////////////////////////////////////
	void Start () {
		// set the sorting layer
		if (GetComponent<Renderer>() && Layer != null) {
			GetComponent<Renderer>().sortingLayerName = Layer;
			GetComponent<Renderer>().sortingOrder = Order;
		}
	}
}
