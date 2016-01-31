//////////////////////////////////////////
/// BonusData
/// Primary way skills use stats/modifications
/// to alter their damage/healing.
//////////////////////////////////////////

public class BonusData {
    // what type of bonus this is
    public BonusTypes BonusType;

    // the combat target to do the check on
    public CombatTargets Target;

    // the stat to check to see if the bonus passes
    public string Stat;

    // the amount of the stat to check to see if the bonus passes
    public int CheckAmount;

    // the amount the bonus is worth, assuming it passes
    public int BonusAmount;

    //////////////////////////////////////////
    /// GetBonusAmount()
    /// Returns the bonus amount, depending
    /// on all its criteria. May return 0.
    //////////////////////////////////////////
    public int GetBonusAmount( CharacterModel i_modelSelf, CharacterModel i_modelTarget ) {
        // start out pessimistic
        int nBonus = 0;

        // decide which character we will be looking at for the bonus based on the bonus' target
        CharacterModel model = Target == CombatTargets.Self ? i_modelSelf : i_modelTarget;

        // do a different kind of check depending on the bonus type
        if ( BonusType == BonusTypes.ForEvery ) {
            // this type of bonus awards its Bonus Amount for every CheckAmount for the given Stat
            int nCheckValue = model.GetTotalModification( Stat );
            if ( nCheckValue > 0 ) {
                int nApplications = nCheckValue / nCheckValue;
                nBonus = BonusAmount * nApplications;
            }
        }
        else
            Debug.LogError( "Unhandled bonus type: " + BonusType.ToString() );

        return nBonus;
    }
}
