using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public abstract class CharacterView : MonoBehaviour {
    // various labels set by this view
    protected Text m_textName;

    // list of abilities this char has
    public List<AbilityView> AbilityViews;

    // character this view represents
    protected ProtoCharacterData m_dataCharacter;
    public string GetID() {
        return m_dataCharacter.Name;
    }

    //////////////////////////////////////////
    /// Awake()
    //////////////////////////////////////////
    void Awake() {
        // cache some important objects
        m_textName = gameObject.FindInChildren( "Name" ).GetComponent<Text>();
    }

    //////////////////////////////////////////
    /// OnDestroy()
    //////////////////////////////////////////
    private void OnDestroy() {
        ListenForMessages( false );
    }

    //////////////////////////////////////////
    /// Init()
    /// Inits this UI with the incoming
    /// character data.
    //////////////////////////////////////////
    public virtual void Init( ProtoCharacterData i_data ) {
        m_dataCharacter = i_data;

        // set labels
        m_textName.text = i_data.Name;

        // listen for messages
        ListenForMessages( true );
    }

    //////////////////////////////////////////
    /// ListenForMessages()
    //////////////////////////////////////////
    protected abstract void ListenForMessages( bool i_bListen );
}
