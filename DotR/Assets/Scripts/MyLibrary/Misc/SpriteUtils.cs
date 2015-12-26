using UnityEngine;
using System.Collections;

//////////////////////////////////////////
/// SpriteUtils
/// Misc utilities for sprites.
//////////////////////////////////////////

public class SpriteUtils {
	//////////////////////////////////////////
	/// ToggleSprite()
	/// Toggles the visibility for the sprite
	/// on the incoming game object.
	//////////////////////////////////////////
	public static void ToggleSprite( GameObject i_go ) {
		SpriteRenderer spr = GetSpriteRenderer( i_go );
		if ( spr != null )
			spr.enabled = !spr.enabled;
	}
	public static void ToggleSprite( GameObject i_go, bool i_bOn ) {
		SpriteRenderer spr = GetSpriteRenderer( i_go );
		if ( spr != null )
			spr.enabled = i_bOn;
	}

	//////////////////////////////////////////
	/// SetSprite()
	/// Sets the sprite of i_go to i_sprite.
	//////////////////////////////////////////
	public static void SetSprite( GameObject i_go, Sprite i_sprite ) {
		SpriteRenderer spr = GetSpriteRenderer( i_go );
		if (spr != null)
			spr.sprite = i_sprite;
	}

	//////////////////////////////////////////
	/// GetSpriteRenderer()
	/// Returns sprite renderer for incoming
	/// game object.
	//////////////////////////////////////////
	public static SpriteRenderer GetSpriteRenderer( GameObject i_go ) {
		SpriteRenderer spr = i_go.GetComponent<SpriteRenderer>();
		if ( spr == null )
			Debug.LogError("SpriteUtils: No sprite renderer on " + i_go, i_go);

		return spr;
	}

	//////////////////////////////////////////
	/// GetSpriteName()
	/// Returns sprite name for incoming
	/// game object.
	//////////////////////////////////////////
	public static string GetSpriteName( GameObject i_go ) {
		SpriteRenderer spr = GetSpriteRenderer( i_go );
		string strName = "NoName";

		if ( spr != null ) {
			if ( spr.sprite != null )
				strName = spr.sprite.name;
			else
				Debug.LogError("Sprite Utils: No sprite name for " + i_go, i_go);
		}

		return strName;
	}
}
