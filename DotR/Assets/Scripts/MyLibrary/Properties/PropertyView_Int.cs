using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

//////////////////////////////////////////
/// PropertyView_Int
/// Simple property view for showing an
/// integer.
//////////////////////////////////////////

public class PropertyView_Int : PropertyView {
    // text for the view
    private Text m_text;
    private Text TextField {
        get {
            if ( m_text == null )
                m_text = GetComponent<Text>();

            return m_text;
        }
    }

    //////////////////////////////////////////
    /// UpdateView()
    //////////////////////////////////////////
    public override void UpdateView() {
        // get the value of the property
        int nValue = ModelToView.GetPropertyValue<int>( PropertyName );

        // set the value
        TextField.text = nValue.ToString();
    }
}
