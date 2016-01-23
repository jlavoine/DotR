using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

//////////////////////////////////////////
/// AbilityView
/// View for an ability in combat.
//////////////////////////////////////////

public abstract class AbilityView : MonoBehaviour {
    // labels and images on the view
    public Text m_textName;
    protected Text Name {
        get {
            if ( m_textName == null )
                m_textName = gameObject.FindInChildren( "Name" ).GetComponent<Text>();

            return m_textName;
        }
    }

    public Text m_textDesc;
    protected Text Desc {
        get {
            if ( m_textDesc == null )
                m_textDesc = gameObject.FindInChildren( "Desc" ).GetComponent<Text>();

            return m_textDesc;
        }
    }
    public List<Image> CostTiles;

    // the ability this view represents
    protected ProtoAbilityData m_dataAbility;
    public bool IsSet() {
        return m_dataAbility != null;
    }
    public ProtoAbilityData GetAbilityData() {
        return m_dataAbility;
    }
    public AbilityColors GetResourceUsed() {
        return m_dataAbility.RequiredColor;
    }

    //////////////////////////////////////////
    /// Init()
    /// Inits this UI with the incoming
    /// ability data.
    //////////////////////////////////////////
    public virtual void Init( ProtoAbilityData i_data ) {
        m_dataAbility = i_data;
        
        // set labels
        Name.text = i_data.Name;
        Desc.text = i_data.Desc;
    }
}
