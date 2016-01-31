using UnityEngine;
using UnityEngine.UI;
using System.Collections;

//////////////////////////////////////////
/// GameOverScreen
/// Simple game over screen. Will probably
/// break this into victory and defeat at
/// some point.
//////////////////////////////////////////

public class GameOverScreen : MonoBehaviour {
    // text showing the player the result
    public Text Title;

    //////////////////////////////////////////
    /// Init()
    //////////////////////////////////////////
    public void Init( bool i_bVictory ) {
        string strTitleKey = i_bVictory ? "GameOver_Victory" : "GameOver_Defeat";
        string strTitle = StringTableManager.Get( strTitleKey );
        Title.text = strTitle;
    }
}
