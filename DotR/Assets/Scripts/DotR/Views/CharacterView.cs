using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

//////////////////////////////////////////
/// CharacterView
/// View for a character in combat.
//////////////////////////////////////////

public class CharacterView : MonoBehaviour {
    // various labels set by this view
    private Text m_textName;
    private Text m_textHP;

    // list of abilities this char has
    public List<AbilityView> m_listAbilities;

    // character this view represents
    private ProtoCharacterData m_dataCharacter;
    public string GetID() {
        return m_dataCharacter.Name;
    }

    //////////////////////////////////////////
    /// Awake()
    //////////////////////////////////////////
    void Awake () {
        // cache some important objects
        m_textHP = gameObject.FindInChildren("Health").GetComponent<Text>();
        m_textName = gameObject.FindInChildren("Name").GetComponent<Text>();

        // cache ability views
        m_listAbilities = new List<AbilityView>();
        m_listAbilities.Add(gameObject.FindInChildren("AbilityView_1").GetComponent<AbilityView>());
        m_listAbilities.Add(gameObject.FindInChildren("AbilityView_2").GetComponent<AbilityView>());
        m_listAbilities.Add(gameObject.FindInChildren("AbilityView_3").GetComponent<AbilityView>());
        m_listAbilities.Add(gameObject.FindInChildren("AbilityView_4").GetComponent<AbilityView>());        
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
    public void Init(ProtoCharacterData i_data) {
        m_dataCharacter = i_data;

        // set labels
        m_textName.text = i_data.Name;
        m_textHP.text = i_data.HP.ToString();

        // create ability views
        for (int i = 0; i < m_listAbilities.Count; ++i) {
            m_listAbilities[i].Init(i_data.Abilities[i]);
        }

        // listen for messages
        ListenForMessages( true );
    }

    //////////////////////////////////////////
    /// Awake()
    //////////////////////////////////////////
    private void ListenForMessages( bool i_bListen ) {
        // create unique key for messages for this character
        string strKey = "_" + GetID();

        if ( i_bListen ) {
            Messenger.AddListener<AbilityColors>( "GainResource" + strKey, OnResourceGained );
        }
        else {
            Messenger.RemoveListener<AbilityColors>( "GainResource" + strKey, OnResourceGained );
        }
    }
  

    //////////////////////////////////////////
    /// OnResourceGained()
    /// Callback for when this character gains
    /// a resource.
    //////////////////////////////////////////
    private void OnResourceGained( AbilityColors i_eResource ) {
        // loop through all the abilities this char has until we find one that uses the incoming resource
        foreach ( AbilityView ability in m_listAbilities ) {
            AbilityColors eResource = ability.GetResourceUsed();
            if ( eResource == i_eResource )
                ability.GainResource();
        }
    }
}
