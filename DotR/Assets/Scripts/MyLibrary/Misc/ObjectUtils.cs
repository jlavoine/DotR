using UnityEngine;
using System.Collections;

////////////////////////////////
/// ObjectUtils
/// Helper methods for 
/// manipulating GameObjects
////////////////////////////////
public static class ObjectUtils {

	////////////////////////////////////////////////////////////////
	/// HideObject()
	/// Hides the object and its children, and disables any
	/// collider2d on the object
	/////////////////////////////////////////////////////////////////
	public static void HideObject(this GameObject i_goObjectToHide) {
		if(i_goObjectToHide != null) {
			// hide all children
			Renderer[] rlistChildren = i_goObjectToHide.GetComponentsInChildren<Renderer>();
			for(int i = 0; i < rlistChildren.Length; i++) {
				rlistChildren[i].enabled = false;
			}
			
			// if the object has a renderer, turn it off
			if(i_goObjectToHide.GetComponent<Renderer>() != null) 
				i_goObjectToHide.GetComponent<Renderer>().enabled = false;
			
			// if there's a collider, deactivate it
			if(i_goObjectToHide.GetComponent<Collider2D>() != null) 
				i_goObjectToHide.GetComponent<Collider2D>().enabled = false;
		}
	}

	////////////////////////////////////////////////////////////////
	/// ShowObject()
	/// Show the object and its children, and enables any
	/// collider2d on the object
	/////////////////////////////////////////////////////////////////
	public static void ShowObject(this GameObject i_goObjectToShow) {
		if(i_goObjectToShow != null) {
			// show all children
			Renderer[] rlistChildren = i_goObjectToShow.GetComponentsInChildren<Renderer>();
			for(int i = 0; i < rlistChildren.Length; i++) {
				rlistChildren[i].enabled = true;
			}
			
			// if the object has a renderer, turn it on
			if(i_goObjectToShow.GetComponent<Renderer>() != null) 
				i_goObjectToShow.GetComponent<Renderer>().enabled = true;
			
			// if there's a collider, activate it
			if(i_goObjectToShow.GetComponent<Collider2D>() != null) 
				i_goObjectToShow.GetComponent<Collider2D>().enabled = true;
		}
	}
}
