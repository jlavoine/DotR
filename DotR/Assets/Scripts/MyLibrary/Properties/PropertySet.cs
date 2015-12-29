using UnityEngine;
using System.Collections.Generic;

//////////////////////////////////////////
/// PropertySet
/// Generic data structure that has a
/// dictionary of properties that are set
/// and retrieved.
//////////////////////////////////////////

public class PropertySet : MonoBehaviour {
    // dictionary of properties
    public Dictionary<string, Property> Properties = new Dictionary<string, Property>();

    //////////////////////////////////////////
    /// GetPropertyValue()
    /// Returns the value stored with the
    /// incoming key.
    //////////////////////////////////////////    
    public T GetPropertyValue<T>( string i_strKey ) {
        // start with default value of T
        T value = default( T );

        if ( Properties.ContainsKey( i_strKey ) ) {
            Property property = Properties[i_strKey];
            value = property.GetValue<T>();
        }

        return value;
    }

    //////////////////////////////////////////
    /// CreateProperty()
    /// Creates an empty property for the
    /// incoming key.
    //////////////////////////////////////////
    public void CreateProperty( string i_strKey ) {
        SetProperty( i_strKey, null );
    }

    //////////////////////////////////////////
    /// GetProperty()
    /// Returns the actual property value for
    /// the incoming key.
    //////////////////////////////////////////
    public Property GetProperty( string i_strKey ) {
        if ( Properties == null )
            Properties = new Dictionary<string, Property>();

        if ( Properties.ContainsKey( i_strKey ) ) {
            return Properties[i_strKey];
        }
        else {
            Debug.LogError( "Trying to get a property that doesn't exist: " + i_strKey );
            return null;
        }
    }

    //////////////////////////////////////////
    /// SetProperty()
    /// Sets the property with the incoming 
    /// key and value.
    //////////////////////////////////////////
    public void SetProperty( string i_strKey, object i_value ) {
        if ( Properties == null )
            Properties = new Dictionary<string, Property>();

        if ( Properties.ContainsKey( i_strKey ) == false ) {
            Properties.Add( i_strKey, new Property() );
        }

        Property property = Properties[i_strKey];
        property.SetValue( i_value );
    }

    //////////////////////////////////////////
    /// HasProperty()
    /// Returns whether or not a property with
    /// the incoming key exists.
    //////////////////////////////////////////
    public bool HasProperty( string i_strKey ) {
        if ( Properties == null )
            Properties = new Dictionary<string, Property>();

        bool bHas = Properties.ContainsKey( i_strKey );
        return bHas;
    }
}
