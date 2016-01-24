//////////////////////////////////////////
/// AppliedEffectData
/// Data for an effect to be applied.
//////////////////////////////////////////

public class AppliedEffectData {
    // ID of the effect to be applied
    public string EffectID;

    // the target of the application
    public CombatTargets Target;

    //////////////////////////////////////////
    /// GetExample()
    /// Returns an example of this class.
    /// Used for testing json serialization.
    //////////////////////////////////////////
    public static AppliedEffectData GetExample() {
        AppliedEffectData ex = new AppliedEffectData();
        ex.EffectID = "BLESSING_A";
        ex.Target = CombatTargets.Self;

        return ex;
    }
}
