using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//////////////////////////////////////////
/// ModelManager
/// Keeps track of all the models in
/// combat.
//////////////////////////////////////////

public class ModelManager : Singleton<ModelManager> {
    // list of models
    public List<CharacterModel> Models;

    public CharacterModel GetModel( string i_strName ) {
        // loop through until we find our model
        foreach ( CharacterModel model in Models ) {
            if ( model.Name == i_strName )
                return model;
        }

        Debug.LogError( "Modle null for " + i_strName + " -- crash inc" );

        return null;
    }
}
