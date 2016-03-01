using System.Collections;

//////////////////////////////////////////
/// QueuedAction
/// Represents a queued action for the
/// action manager.
//////////////////////////////////////////

public class QueuedAction {
    // character who used the action
    private string m_strCharacterID;
    public string GetOwnerID() {
        return m_strCharacterID;
    }

    // ability for the queued action
    private AbilityData m_dataAbility;
    public AbilityData GetData() {
        return m_dataAbility;
    }

    public QueuedAction( DefaultModel i_char, AbilityData i_ability ) {
        m_strCharacterID = i_char.GetPropertyValue<string>( "Name" );
        m_dataAbility = i_ability;
    }
}
