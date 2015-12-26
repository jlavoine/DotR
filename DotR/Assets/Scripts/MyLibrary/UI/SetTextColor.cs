using UnityEngine;
using System.Collections;

//////////////////////////////////////////
/// SetTextColor
/// Simple script that sets the color of
/// the TextMesh on this object to a color
/// specified in constants.
//////////////////////////////////////////

public class SetTextColor : MonoBehaviour {
	// constant that contains the color
	public string TextColor;
	
	//////////////////////////////////////////
	/// Start()
	//////////////////////////////////////////
	void Start () {
		TextMesh mesh = gameObject.GetComponent<TextMesh>();
		if ( mesh ) {
			Color colorText = Constants.GetConstant<Color>( TextColor );
			mesh.color = colorText;
		}
		else
			Debug.LogError( "Warning: Object has no TextMesh but trying to access it( " + gameObject.name + ")", gameObject );
	}
}
