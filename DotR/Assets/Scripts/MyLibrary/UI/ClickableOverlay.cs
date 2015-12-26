using UnityEngine;
using System.Collections;

public class ClickableOverlay : MonoBehaviour {

	// the object to show when the mouse is over this object
	public GameObject MouseOverObject = null;

	// the object to show when the mouse is not over this object
	public GameObject NormalStateObject = null;

	// the object to show when the mouse is pressed down over this object
	public GameObject MousePressObject = null;

	private bool m_bIsMouseOver = false;
	public bool IsMouseOver() {
		return m_bIsMouseOver;
	}

	private bool m_bIsMousePressed = false;
	public bool IsMousePressed() {
		return m_bIsMousePressed;
	}

	private bool m_bEnabled = true;
	public void SetEnabled(bool i_bEnabled) {
		m_bEnabled = i_bEnabled;
	}

	private GameObject m_goLastShown = null;

	// Use this for initialization
	protected virtual void Start () {
		UpdateGraphics();
	}
	
	// Update is called once per frame
	protected virtual void Update () {
	
	}

	void UpdateGraphics ()
	{
		if(m_bEnabled) {
			if(m_bIsMouseOver) {
				if(m_bIsMousePressed) {
					// hide last object shown
					if(m_goLastShown != null) m_goLastShown.HideObject();

					MousePressObject.ShowObject();

					// save last object shown
					m_goLastShown = MousePressObject;
				} else {
					// hide last object shown
					if(m_goLastShown != null) m_goLastShown.HideObject();
					
					MouseOverObject.ShowObject();

					// save last object shown
					m_goLastShown = MouseOverObject;
				}
			} else {
				// hide last object shown
				if(m_goLastShown != null) m_goLastShown.HideObject();
				
				NormalStateObject.ShowObject();

				// save last object shown
				m_goLastShown = NormalStateObject;
			}
		} else {
			// hide last object shown
			if(m_goLastShown != null) m_goLastShown.HideObject();
			
			NormalStateObject.ShowObject();
			
			// save last object shown
			m_goLastShown = NormalStateObject;
		}
	}

	void OnMouseEnter() {
		m_bIsMouseOver = true;
		UpdateGraphics();
	}

	void OnMouseExit() {
		m_bIsMouseOver = false;
		m_bIsMousePressed = false;
		UpdateGraphics();
	}

	void OnMouseDown() {
		if(m_bIsMouseOver) {
			m_bIsMousePressed = true;
			UpdateGraphics();
		}
	}

	void OnMouseUp() {
		m_bIsMousePressed = false;
		UpdateGraphics();
	}

	void OnMouseUpAsButton() {
		if(m_bEnabled) DoMouseUpAsButton();
	}

	////////////////////////////////
	/// DoMouseUpAsButton()
	/// Method to override which
	/// performs an action when
	/// the overlay is clicked
	////////////////////////////////
	protected virtual void DoMouseUpAsButton() {
	}
}
