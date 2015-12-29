using UnityEngine;
using System.Collections;

//////////////////////////////////////////
/// Property
/// A guid/value object that will send an
/// update whenever it changes.
//////////////////////////////////////////

public class Property {
    // property's id
    public System.Guid ID;
    public System.Guid GetID() {
        return ID;
    }

    // value the property holds
    public object Value;
    public T GetValue<T>() {
        return (T) Value;
    }

    public void SetValue( object i_object ) {
        Value = i_object;

        // send out a message that this property has been changed
        Messenger.Broadcast( "SetDirty_" + ID );
    }

    //////////////////////////////////////////
    /// Property()
    //////////////////////////////////////////
    public Property( object i_value = null ) {
        // when the property is created, create a new guid
        ID = System.Guid.NewGuid();

        // set the value
        Value = i_value;
    }
}
