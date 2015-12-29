using UnityEngine;
using System.Collections;

//////////////////////////////////////////
/// PropertyView
/// UI element that represents some property.
//////////////////////////////////////////

public abstract class PropertyView : MonoBehaviour {
    // begin pure abstracts ----------------------
    public abstract void UpdateView();      // do whatever updating because a property has changed.
    // end pure abstracts ------------------------

    // name of property for this view
    public string PropertyName;

    // can be set from the inspector
    public DefaultModel ModelToView;

    // the guid of the property for this view
    protected System.Guid m_guid;

    //////////////////////////////////////////
    /// Start()
    //////////////////////////////////////////
    void Start () {
        // model may be non-null from inspector
        if ( ModelToView != null )
            SetModel( ModelToView );
	}

    //////////////////////////////////////////
    /// SetModel()
    /// Sets the model that this view is for.
    //////////////////////////////////////////
    public void SetModel( DefaultModel i_model ) {
        // null check for safety
        ModelToView = i_model;
        if ( i_model == null ) {
            Debug.LogError( "PropertyView has null model: " + PropertyName );
        }

        // if the model doesn't have the property, we want to create it
        bool bHas = ModelToView.HasProperty( PropertyName );
        if ( bHas == false )
            ModelToView.CreateProperty( PropertyName );

        // get and save the property's guid for message listening
        Property property = ModelToView.GetProperty( PropertyName );
        System.Guid guid = property.GetID();
        SetPropertyID( guid, !bHas );
    }

    //////////////////////////////////////////
    /// OnDestroy()
    //////////////////////////////////////////
    void OnDestroy() {
        Messenger.RemoveListener( "SetDirty_" + m_guid, UpdateView );
    }

    //////////////////////////////////////////
    /// SetPropertyID()
    /// Sets the property id for this view.
    //////////////////////////////////////////
    public void SetPropertyID( System.Guid i_guid, bool i_bNew ) {
        m_guid = i_guid;

        // listen for changes
        Messenger.AddListener( "SetDirty_" + m_guid, UpdateView );

        // update the view if the property isn't new
        if ( i_bNew == false )
            UpdateView();
    }
}
