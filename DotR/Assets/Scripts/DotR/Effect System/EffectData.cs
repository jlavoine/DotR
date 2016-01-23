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
}
