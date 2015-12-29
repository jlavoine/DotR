using UnityEngine;
using System.Collections;

public class TheaterCharacter : MonoBehaviour {
    private Animator m_animator;
    private Rigidbody m_rbody;

    void Awake() {
        ListenForMessages( true );

        m_animator = GetComponent<Animator>();
        m_rbody = GetComponent<Rigidbody>();
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnDestroy() {
        ListenForMessages( false );
    }

    //////////////////////////////////////////
    /// ListenForMessages()
    /// Subscribes or unsubscribes from messages.
    //////////////////////////////////////////
    private void ListenForMessages( bool i_bSubscribe ) {
        if ( i_bSubscribe ) {
            Messenger.AddListener( "InitCombat", OnInitCombat );
        }
        else {
            Messenger.RemoveListener( "InitCombat", OnInitCombat );
        }
    }

    private void OnInitCombat() {
        m_animator.SetBool( "Run", true );
        float fSpeed = Constants.GetConstant<float>( "Theater_MoveSpeed" );
        m_rbody.velocity = transform.forward * fSpeed;
    }
}
