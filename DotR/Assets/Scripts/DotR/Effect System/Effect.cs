using System.Collections;

//////////////////////////////////////////
/// Effect
/// An non-immutable instance of an effect
/// that can be applied to a model.
//////////////////////////////////////////

public class Effect  {
    // data that this effect points to
    private EffectData m_data;
    public string GetID() {
        return m_data.ID;
    }
    public EffectData GetData() {
        return m_data;
    }

    // # of turns this effect will remain
    private int m_nRemainingTurns = 0;
    public int RemainingTurns {
        get {
            return m_nRemainingTurns;
        }
        set {
            m_nRemainingTurns = value;
        }
    }

    //////////////////////////////////////////
    /// Effect()
    //////////////////////////////////////////
    public Effect( AppliedEffectData i_data ) {
        // store a reference to the actual data of this effect
        m_data = IDL_Effects.GetData( i_data.EffectID );

        if ( m_data != null ) {
            // set the # of turns this effect will last
            m_nRemainingTurns = m_data.Duration;
        }
        else
            Debug.LogError( "Warning, no effect data for " + i_data.EffectID );
    }

    //////////////////////////////////////////
    /// Modifies()
    /// Returns whether or not this effect
    /// modifies the incoming key.
    //////////////////////////////////////////
    public bool Modifies( string i_strKey ) {
        bool bModifies = m_data.Modifies( i_strKey );

        return bModifies;
    }

    //////////////////////////////////////////
    /// GetModification()
    /// Returns the modification for the
    /// incoming key of this effect.
    //////////////////////////////////////////
    public int GetModification( string i_strKey ) {
        int nMod = m_data.GetModification( i_strKey );
        return nMod;
    }

    //////////////////////////////////////////
    /// ShouldRemove()
    /// Given the incoming removal data, will
    /// return if this effect should be 
    /// removed.
    //////////////////////////////////////////
    public bool ShouldRemove( RemovedEffectData i_removalData ) {
        // if the removal data has an effect ID that matches this effect's ID, it should be removed
        if ( i_removalData.EffectID == m_data.ID )
            return true;

        // next, if the removal effect removes by category, see if there are any matching categories
        foreach ( EffectCategories category in i_removalData.Categories ) {
            if ( m_data.Categories.Contains( category ) )
                return true;
        }

        // the effect should not be removed if we got to here
        return false;
    }
}
