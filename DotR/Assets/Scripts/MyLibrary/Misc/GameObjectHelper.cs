using System.Linq;
using UnityEngine;
using System.Collections.Generic;

public static class GameObjectHelper
{
	public static GameObject FindInChildren(this GameObject go, string name)
	{
		return (from x in go.GetComponentsInChildren<Transform>()
		        where x.gameObject.name == name
		        select x.gameObject).FirstOrDefault();
	}

    public static GameObject InstantiateUI( this GameObject go, GameObject i_goPrefab, GameObject i_goParent = null ) {
        GameObject goNew = GameObject.Instantiate<GameObject>( i_goPrefab );
        if ( i_goParent == null )
            i_goParent = go;

        goNew.transform.SetParent( i_goParent.transform, false );

        return goNew;
    }
	
	public static GameObject GetParent( this GameObject go ) {
		GameObject goParent = null;
		
		if ( go.transform.parent )
			goParent = go.transform.parent.gameObject;
		
		if ( goParent == null )
			Debug.LogError("Something trying to get a game object's parent that doesn't have one...");
		
		return goParent;
	}

	public static void DestroyChildren(this GameObject go) {
		
		List<GameObject> children = new List<GameObject>();
		foreach (Transform tran in go.transform)
		{      
			children.Add(tran.gameObject); 
		}
		children.ForEach(child => GameObject.Destroy(child));  
	}
}