using System.Collections;

//////////////////////////////////////////
/// Effect
/// An non-immutable instance of an effect
/// that can be applied to a model.
//////////////////////////////////////////

public class Effect  {
    // data that this effect points to
    private EffectData m_data;

    // # of turns this effect will remain
    private int m_nRemainingTurns = 0;

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
}
