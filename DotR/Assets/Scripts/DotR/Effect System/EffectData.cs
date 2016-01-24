using System.Collections.Generic;

//////////////////////////////////////////
/// EffectData
/// Immutable data for an effect.
//////////////////////////////////////////

public class EffectData : GenericData  {
    // name of the effect
    public string Name;

    // duration of the effect: -1 is infinite, 0 is instant, positive integer for # of turns
    public int Duration;

    // list of categories this effect belongs to
    public List<EffectCategories> Categories;

    // list of modifications this effect bestows
    public List<ModificationData> Modifications;

    public EffectData() {
        // just in case the modificaitons list is empty, init it
        Modifications = new List<ModificationData>();
    }

    //////////////////////////////////////////
    /// GetExample()
    /// Returns an example of this class, used
    /// for validating json serialization.
    //////////////////////////////////////////
    public static EffectData GetExample() {
        EffectData exampleEffect = new EffectData();
        exampleEffect.ID = "BLESSING_REGEN";
        exampleEffect.Name = "Blessing of Arrun";
        exampleEffect.Duration = -1;

        List<EffectCategories> exampleCategories = new List<EffectCategories>();
        exampleCategories.Add( EffectCategories.Blessing );
        exampleCategories.Add( EffectCategories.Positive );
        exampleEffect.Categories = exampleCategories;

        List<ModificationData> exampleMods = new List<ModificationData>();
        for ( int i = 0; i < 3; ++i ) {
            ModificationData exampleMod = ModificationData.GetExample();
            exampleMods.Add( exampleMod );
        }
        exampleEffect.Modifications = exampleMods;

        return exampleEffect;
    }

    //////////////////////////////////////////
    /// Modifies()
    /// Returns whether or not this effect
    /// modifies the incoming key.
    //////////////////////////////////////////
    public bool Modifies( string i_strKey ) {
        // get the actual modification effect
        ModificationData data = GetModificationData( i_strKey );

        // if the modification data is null or not determines if the effect modifies it
        return data != null;
    }

    //////////////////////////////////////////
    /// GetModificationData()
    /// Returns the modification data for this
    /// effect and the incoming key, or null
    /// if the effect doesn't modify it.
    //////////////////////////////////////////
    private ModificationData GetModificationData( string i_strKey ) {
        // kinda sucky atm, but just search through the list
        foreach ( ModificationData mod in Modifications ) {
            // if we find it, just return true straight away
            if ( mod.Target == i_strKey )
                return mod;
        }

        // if we get here, no data was found...
        return null;
    }

    //////////////////////////////////////////
    /// GetModification()
    /// Returns the modification for the
    /// incoming key of this effect (as an
    /// integer).
    //////////////////////////////////////////
    public int GetModification( string i_strKey ) {
        // find the modification
        ModificationData data = GetModificationData( i_strKey );

        // if the data is not null, return it's modification
        if ( data != null )
            return (int) data.Amount;
        else
            return 0;
    }
}
