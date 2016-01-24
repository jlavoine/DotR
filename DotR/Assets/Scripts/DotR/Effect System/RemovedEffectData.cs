using System.Collections.Generic;

//////////////////////////////////////////
/// RemovedEffectData
/// Data for an effect to be removed.
//////////////////////////////////////////

public class RemovedEffectData {
    // the target of the removal
    public CombatTargets Target;

    // ID of the effect to be removed (optional)
    public string EffectID;

    // list of categories to look for to remove (optional)
    public List<EffectCategories> Categories;

    public RemovedEffectData() {
        // init optional params
        Categories = new List<EffectCategories>();
    }
}
