using UnityEngine;
using System.Collections.Generic;

//////////////////////////////////////////
/// CharacterModel
/// Represents a character's data.
//////////////////////////////////////////

public class CharacterModel : DefaultModel {
    // what key should this model uses?
    public string Name;

    // data this model uses
    private ProtoCharacterData m_data;
    public ProtoCharacterData GetData() {
        return m_data;
    }

    //////////////////////////////////////////
    /// Awake()
    //////////////////////////////////////////
    void Awake() {
        // init data asap
        m_data = IDL_ProtoCharacters.GetCharacter( Name );

        // set various things
        SetProperty( "HP", m_data.HP );
        SetProperty( "Effects", new Dictionary<string, Effect>() );

        // listen for messages
        ListenForMessages( true );
    }

    //////////////////////////////////////////
    /// OnDestroy()
    //////////////////////////////////////////
    void OnDestroy() {
        // stop listening for messages
        ListenForMessages( false );
    }

    //////////////////////////////////////////
    /// ListenForMessages()
    //////////////////////////////////////////
    private void ListenForMessages( bool i_bListen ) {
        if ( i_bListen ) {
            Messenger.AddListener( "TurnOver_" + m_data.Name, OnTurnOver );
        }
        else {
            Messenger.AddListener( "TurnOver_" + m_data.Name, OnTurnOver );
        }
    }

    //////////////////////////////////////////
    /// OnTurnOver()
    /// Callback for when this character's
    /// turn is over.
    //////////////////////////////////////////
    private void OnTurnOver() {
        // process any hp ticking by getting the total amount hp should change...
        int nHpTick = GetTotalModification( "HpTick" );

        // then just alter hp by it
        AlterHP( nHpTick );

        // tick all effects so their duration goes down
        TickEffects();
    }

    //////////////////////////////////////////
    /// TickEffects()
    /// Ticks the duration of each effect,
    /// and removes any that have expired.
    /// This code is kind of similar to the
    /// RemoveEffect() code...
    //////////////////////////////////////////
    private void TickEffects() {
        // first get all effects on this model
        List<Effect> listEffects = GetAllEffects();

        // this new dictionary will contain the non-removed effects
        Dictionary<string, Effect> dictNewEffects = new Dictionary<string, Effect>();

        // loop through each effect and tick it down a turn
        foreach ( Effect effect in listEffects ) {
            if ( effect.RemainingTurns > 0 )
                effect.RemainingTurns -= 1;

            // remove effects that have no remaining turns
            bool bShouldRemove = effect.RemainingTurns == 0;
            if ( bShouldRemove == false )
                dictNewEffects.Add( effect.GetID(), effect );
            else {
                //Debug.Log( "Effect Removed: " + effect.GetID() );
            }
        }

        // set our current effects to the new list, which will have the removed effects eliminated from it
        SetProperty( "Effects", dictNewEffects );
    }

    //////////////////////////////////////////
    /// GetTotalModification()
    /// Returns the total modification for the
    /// incoming key for this character.
    //////////////////////////////////////////
    private int GetTotalModification( string i_strKey ) {
        int nTotal = 0;

        // go through all effects on this character and get any relevant modifications
        List<Effect> listEffects = GetAllEffects();
        foreach ( Effect effect in listEffects ) {
            if ( effect.Modifies( i_strKey ) )
                nTotal += effect.GetModification( i_strKey );
        }

        return nTotal;
    }

    //////////////////////////////////////////
    /// GetAllEffects()
    /// Returns all effects currently on
    /// this character.
    //////////////////////////////////////////
    private List<Effect> GetAllEffects() {
        List<Effect> listEffects = new List<Effect>();

        // crummy, but get the dictionary of effects and turn it into a list
        Dictionary<string, Effect> dictEffects = GetPropertyValue<Dictionary<string, Effect>>( "Effects" );
        foreach ( KeyValuePair<string, Effect> pair in dictEffects )
            listEffects.Add( pair.Value );

        return listEffects;
    }

    //////////////////////////////////////////
    /// AlterHP()
    /// Changes this model's HP by the incoming
    /// amount.
    //////////////////////////////////////////
    public void AlterHP( int i_nAmount ) {
        int nHP = GetPropertyValue<int>( "HP" );
        nHP += i_nAmount;
        SetProperty( "HP", nHP );
    }

    //////////////////////////////////////////
    /// VerifyChain()
    /// Returns true if this character has
    /// an ability that meets the incoming
    /// list's criteria.
    //////////////////////////////////////////
    public bool VerifyChain( List<GamePiece_Chain> i_listChain ) {
        List<AbilityColors> listColors = new List<AbilityColors>();
        foreach ( GamePiece piece in i_listChain )
            listColors.Add( piece.GetColor() );

        foreach ( ProtoAbilityData data in m_data.Abilities ) {
            List<AbilityColors> listRequired = data.RequiredColors;

            // if the chain so far is longer than the ability's required colors, then they are not a match
            if ( listColors.Count > listRequired.Count )
                continue;

            // otherwise, let's check to see if there's a match
            bool bMatch = true;
            for ( int i = 0; i < listColors.Count; ++i ) {
                if ( listColors[i] != listRequired[i] ) {
                    bMatch = false;
                    break;
                }
            }

            // if there was a match throughout every color in the chain, verified!
            if ( bMatch )
                return true;
        }

        // no abilities that matched the chain were found
        return false;
    }

    //////////////////////////////////////////
    /// GetAbilityFromChain()
    /// Returns the ability from the incoming
    /// chain of pieces. CAN RETURN NULL.
    //////////////////////////////////////////
    public ProtoAbilityData GetAbilityFromChain( List<GamePiece_Chain> i_listChain ) {
        List<AbilityColors> listColors = new List<AbilityColors>();
        foreach ( GamePiece piece in i_listChain )
            listColors.Add( piece.GetColor() );

        foreach ( ProtoAbilityData data in m_data.Abilities ) {
            List<AbilityColors> listRequired = data.RequiredColors;

            // if the chain's length doesn't match the ability's length, don't bother checking
            if ( listColors.Count != listRequired.Count )
                continue;

            // getting here mean's we need to check if the chain's colors match the required colors
            bool bMatch = true;
            for ( int i = 0; i < listColors.Count; ++i ) {
                if ( listColors[i] != listRequired[i] ) {
                    bMatch = false;
                    break;
                }
            }

            // if there was a match throughout every color in the chain, verified!
            if ( bMatch )
                return data;
        }

        // there was no such ability
        return null;
    }

    //////////////////////////////////////////
    /// ApplyEffect()
    /// Applies incoming effect on this model.
    //////////////////////////////////////////
    public void ApplyEffect( AppliedEffectData i_effectApplied ) {
        // create a new effect from the data
        Effect effect = new Effect( i_effectApplied );

        // get the currently applied effects on this model
        Dictionary<string, Effect> dictEffects = GetPropertyValue<Dictionary<string, Effect>>( "Effects" );

        // only apply the effect if it's not already on the character (subject to change? ugh...it's a dictionary!)
        if ( dictEffects.ContainsKey( i_effectApplied.EffectID ) == false ) {
            dictEffects.Add( i_effectApplied.EffectID, effect );
            SetProperty( "Effects", dictEffects );
        }

        //Debug.Log( "Applying effect " + i_effectApplied.EffectID );
    }

    //////////////////////////////////////////
    /// RemoveEffect()
    /// Remove incoming effect on this model,
    /// if it exists.
    //////////////////////////////////////////
    public void RemoveEffect( RemovedEffectData i_removalData ) {
        // first get all effects on this model
        List<Effect> listEffects = GetAllEffects();

        // this new dictionary will contain the non-removed effects
        Dictionary<string, Effect> dictNewEffects = new Dictionary<string, Effect>();

        // loop through each effect and see if it should be removed...
        // if it shouldn't, add it to the new list
        foreach ( Effect effect in listEffects ) {
            bool bShouldRemove = effect.ShouldRemove( i_removalData );
            if ( bShouldRemove == false )
                dictNewEffects.Add( effect.GetID(), effect );
            else {
                //Debug.Log( "Effect Removed: " + effect.GetID() );
            }
        }

        // set our current effects to the new list, which will have the removed effects eliminated from it
        SetProperty( "Effects", dictNewEffects );
    }

    //////////////////////////////////////////
    /// HasEffect()
    /// Returns whether or not the incoming
    /// effect is present on this model.
    //////////////////////////////////////////
    public bool HasEffect( string i_strKey ) {
        // get dictionary of effects
        Dictionary<string, Effect> dictEffects = GetPropertyValue<Dictionary<string, Effect>>( "Effects" );

        // is the key in the dictionary? simple
        bool bHas = dictEffects.ContainsKey( i_strKey );
        return bHas;
    }

    //////////////////////////////////////////
    /// SetEffects()
    /// This is essentially a helper method
    /// for setting a list of effects on the
    /// model, because it normally takes a
    /// dictionary.
    //////////////////////////////////////////
    /*private void SetEffects( List<Effect> i_listEffects ) {
        Dictionary<string, Effect> dictEffects = new Dictionary<string, Effect>();

        // loop through the effects and add them to the dictionary
        foreach ( Effect effect in i_listEffects )

        // set the effects
        SetProperty( "Effects", dictEffects );
    }*/
}
