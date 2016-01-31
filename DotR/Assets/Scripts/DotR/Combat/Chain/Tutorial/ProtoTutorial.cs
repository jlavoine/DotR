using UnityEngine;
using System.Collections.Generic;

public class ProtoTutorial : MonoBehaviour {
    public List<GameObject> Tutorials;

    public GameObject CurrentTutorial;

    private int m_nTutorial = 0;

	// Use this for initialization
	void Start () {
        CurrentTutorial = Tutorials[m_nTutorial];
        CurrentTutorial.SetActive( true );
	}
	
	// Update is called once per frame
	void Update () {
        if ( Input.GetKeyDown( KeyCode.Escape ) )
            Destroy( gameObject );
	}

    public void OnClick() {
        m_nTutorial++;
        if ( m_nTutorial >= Tutorials.Count ) {
            Destroy( gameObject );
            return;
        }
        else {
            Destroy( CurrentTutorial );
            CurrentTutorial = Tutorials[m_nTutorial];
            CurrentTutorial.SetActive( true );
        }
    }
}
