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
    private ProtoAbilityData m_dataAbility;
    public ProtoAbilityData GetData() {
        return m_dataAbility;
    }

    public QueuedAction( ProtoCharacterData i_char, ProtoAbilityData i_ability ) {
        m_strCharacterID = i_char.Name;
        m_dataAbility = i_ability;
    }
}
