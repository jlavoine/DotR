using UnityEngine;
using System.Collections;

public class SceneLoader : MonoBehaviour {

    public void LoadCombat() {
        Application.LoadLevel( "Proto1" );
    }

    public void LoadPerks() {
        Application.LoadLevel( "Perks" );
    }

    public void LoadMain() {
        Application.LoadLevel( "Main" );
    }
}
