using UnityEngine;
using System.Collections;

public class TheaterCharacter : MonoBehaviour {
    private Animator m_animator;
    private Rigidbody m_rbody;

    private TheaterCombatStates m_eState;

    void Awake() {
        ListenForMessages( true );

        m_animator = GetComponent<Animator>();
        m_rbody = GetComponent<Rigidbody>();
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
            Messenger.AddListener( "ReturnToStart", ReturnToStart );
        }
        else {
            Messenger.RemoveListener( "InitCombat", OnInitCombat );
            Messenger.RemoveListener( "ReturnToStart", ReturnToStart );
        }
    }

    private void OnInitCombat() {
        m_eState = TheaterCombatStates.ToCombat;
        MoveCharacter();
    }

    private void MoveCharacter() {
        m_animator.SetBool( "Run", true );
        float fSpeed = Constants.GetConstant<float>( "Theater_MoveSpeed" );
        m_rbody.velocity = transform.forward * fSpeed;
    }

    private void StopCharacter() {
        m_rbody.velocity = new Vector3( 0, 0, 0 );
        m_animator.SetBool( "Run", false );
    }

    private void FlipCharacter() {
        transform.RotateAround( transform.position, transform.up, 180f );
    }

    public void OnTriggerEnter( Collider i_collider ) {
        StopCharacter();

        if ( m_eState == TheaterCombatStates.FromCombat ) {
            FlipCharacter();
            m_eState = TheaterCombatStates.Idle;
        }
    }

    private void ReturnToStart() {
        m_eState = TheaterCombatStates.FromCombat;
        FlipCharacter();
        MoveCharacter();
    }
}
