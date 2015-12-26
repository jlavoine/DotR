using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

//////////////////////////////////////////
/// Tutorial
/// The parent class for all tutorials.
/// Originally created for MIRACLE, but I'm
/// hoping to make it generic enough for
/// all other projects.
//////////////////////////////////////////

public abstract class Tutorial : MonoBehaviour {
	// pure abstract functions
	protected abstract void SetKey();						// the tutorial key is used to mark a lot of lists
	protected abstract void SetMaxSteps();					// set the max steps of the tutorial
	protected abstract void ProcessStep( int nStep );		// most of a tutorial is processing its steps and doing things
	//protected abstract void End( bool bFinished );			// when the tutorial is finishd

	// list of objects that can be processed as input
	private List<GameObject> listCanProcess = new List<GameObject>();
	protected void AddToProcessList( GameObject go ) {
		listCanProcess.Add( go );
	}
	protected void RemoveFromProcessList( GameObject go ) {
		listCanProcess.Remove( go );	
	}
	public bool CanProcess( GameObject go ) {
		bool bCan = listCanProcess.Contains( go );
		return bCan;
	}

	// step the tutorial is currently on
	private int m_nCurrentStep;
	protected void SetStep( int num ) {
		m_nCurrentStep = num;
		
		// if we have exceeded max steps in this tutorial, end it
		if (m_nCurrentStep >= m_nMaxSteps)
			End(true);
		else {
			// if a step already existed, destroy it
			CleanupPopup();

			// now process the next step
			ProcessStep(m_nCurrentStep);
		}
	}
	protected int GetStep() {
		return m_nCurrentStep;	
	}

	// this object represents the actual thing that is running a step of a tutorial
	protected GameObject m_goTutStep;
	
	// max steps in the tutorial
	protected int m_nMaxSteps;
	
	// key for this tutorial
	protected string m_strKey;
	protected string GetKey() {
		if ( string.IsNullOrEmpty(m_strKey) )
			SetKey();
		
		return m_strKey;	
	}

	protected virtual void Start() {
		Debug.Log("Starting tutorial " + GetKey());

		// add listener for when user hits an ok button
		Messenger.AddListener( "OkPressed", OnOK );

		SetMaxSteps();
		SetStep( 0 );
	}

	//////////////////////////////////////////
	/// Advance()
	/// Go to the next part of this tutorial.
	//////////////////////////////////////////
	public void Advance() {
		// increment the current step of the tutorial
		int nStep = GetStep();
		nStep++;
		SetStep( nStep );
	}	
	
	///////////////////////////////////////////
	/// End()
	/// When this tutorial is finished.
	///////////////////////////////////////////		
	protected virtual void End( bool bFinished ) {
		// debug message
		Debug.Log("Tutorial Ending: " + GetKey());

		// destroy the popup hanging around
		CleanupPopup();
			
		// remove the listener for ok button
		Messenger.RemoveListener( "OkPressed", OnOK );

		// destroy game object housing this tutorial
		Destroy(gameObject);
	
		// save the fact that the user completed this tutorial
		if (bFinished)
			Messenger.Broadcast<string>("OnTutorialCompleted", GetKey());
	}
	
	///////////////////////////////////////////
	/// Abort()
	/// Ends the tutorial early.
	///////////////////////////////////////////		
	public void Abort() {
		End( false );	
	}

	//////////////////////////////////////////	
	/// CleanupPopup()
	//////////////////////////////////////////
	protected virtual void CleanupPopup() {
		if (m_goTutStep != null)
			Destroy (m_goTutStep);
	}
	
	//////////////////////////////////////////	
	/// ShowPopup()
	/// Will show whatever popup is in the
	/// resources folder for this step.
	//////////////////////////////////////////
	protected void ShowPopup( int i_nStep ) {
		// create the appropriate messaging/image object
		GameObject prefab = Resources.Load (GetKey() + "_" + i_nStep) as GameObject;
		m_goTutStep = GameObject.Instantiate(prefab) as GameObject;

		// set the bg opacity to the correct value (if there's a bg)
		GameObject goBG = m_goTutStep.FindInChildren("BG");
		if(goBG != null) {
			SpriteRenderer sprBG = goBG.GetComponent<SpriteRenderer>();
			if (sprBG != null) {
				Color color = sprBG.color;
				color.a = (float)195/255;
				sprBG.color = color;
			}
		}
	}

	//////////////////////////////////////////	
	/// OnOK()
	/// Using OnGUI as a quick hack to advance
	/// the step of a tutorial.
	//////////////////////////////////////////
	private void OnOK() {
		Advance();
	}
}

