using UnityEngine;
using System.Collections;

public class PlayAgainButton : MonoBehaviour {

	public void OnClick() {
        Application.LoadLevel( "Proto1" );
    }
}
