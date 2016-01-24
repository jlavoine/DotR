using System.Collections;

//////////////////////////////////////////
/// ModificationData
/// Immutable data for a single modification.
/// Used by effects to specify what the
/// effect modifies.
//////////////////////////////////////////

public class ModificationData {
    // the name of the thing to modify
    public string Target;

    // what type of modification is this (a percentage, flat amount, etc)
    public ModificationTypes ModType;

    // the actual amount to modify by
    public float Amount;

    //////////////////////////////////////////
    /// GetExample()
    /// Returns an example of this class, used
    /// for validating json serialization.
    //////////////////////////////////////////
    public static ModificationData GetExample() {
        ModificationData example = new ModificationData();
        example.Target = "Strength";
        example.ModType = ModificationTypes.Flat;
        example.Amount = 3;

        return example;
    }
}
