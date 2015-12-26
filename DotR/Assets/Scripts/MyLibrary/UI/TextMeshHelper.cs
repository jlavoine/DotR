using UnityEngine;
using System.Collections;

//////////////////////////////////////////
/// TextMesherHelper
/// Functions that extend the TextMesh
/// class.
//////////////////////////////////////////

public static class TextMeshHelper {
	//////////////////////////////////////////
	/// SetText()
	/// Sets some text on the incoming mesh if
	/// it is non-null; throws an error otehrwise.
	//////////////////////////////////////////
	public static void SetText(this TextMesh i_mesh, string i_strText) {
		// null check
		if (i_mesh == null) {
			Debug.LogError("Null text mesh trying to get set.");
			return;
		}
		
		i_mesh.text = i_strText;
	}
}
