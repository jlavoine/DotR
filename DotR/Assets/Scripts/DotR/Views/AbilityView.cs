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
    protected Text m_textName;
    protected Text m_textDesc;
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
    /// Awake()
    //////////////////////////////////////////
    void Awake() {
        // cache some important objects
        m_textName = gameObject.FindInChildren( "Name" ).GetComponent<Text>();
        m_textDesc = gameObject.FindInChildren( "Desc" ).GetComponent<Text>();
    }

    //////////////////////////////////////////
    /// Init()
    /// Inits this UI with the incoming
    /// ability data.
    //////////////////////////////////////////
    public virtual void Init( ProtoAbilityData i_data ) {
        m_dataAbility = i_data;

        // set labels
        m_textName.text = i_data.Name;
        m_textDesc.text = i_data.Desc;
    }
}
