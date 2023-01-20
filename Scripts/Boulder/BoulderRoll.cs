using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class BoulderRoll : MonoBehaviour
{
    //Boulder physics Collider
    private CircleCollider2D _physicsCollider;

    //Boulder physics
    [SerializeField] private GameObject _player;
    private Vector3 _groundNormal;
    private Rigidbody2D _rigidbody;
    private BoulderController _boulderController;

    //boulder landing variables
    [SerializeField] private float _slowDownPercentage;

    //Boulder ground state
    [SerializeField] private LayerMask _groundLayer;
    private bool _isGrounded;
    private bool _wasGrounded;
    private float _groundCollisionDistance => _physicsCollider.radius + 0.1f;
    private float _boulderFudgeTime = 0.05f;
    private float _lastGroundedTime;

    //audio variables
    private BoulderLandingAudio _boulderLandingAudio;
    private BoulderRollingAudio _boulderRollingAudio;
    private bool _isRollingAudioPlaying;

    //Player components
    private PlayerMovement _playerMovement;
    private PlayerState _playerState;

    //Boulder VFX
    [SerializeField] private VisualEffect _landingParticle;
    [SerializeField] private ParticleSystem _rollingParticle;

    public bool IsGrounded => _isGrounded;

    private void Awake()
    {
        _physicsCollider = GetComponent<CircleCollider2D>();
        _rigidbody = GetComponent<Rigidbody2D>();
        _boulderController = GetComponent<BoulderController>();
        _boulderLandingAudio = GetComponentInChildren<BoulderLandingAudio>();
        _boulderRollingAudio = GetComponentInChildren<BoulderRollingAudio>();

        //Get player components
        _playerMovement = _player.GetComponent<PlayerMovement>();
        _playerState = _player.GetComponent<PlayerState>();
    }

    private void Update()
    {
        GroundCheck();
        _wasGrounded = _isGrounded;

        if (!_isRollingAudioPlaying && _isGrounded)
        {
            _isRollingAudioPlaying = _boulderRollingAudio.PlaySound();
            _rollingParticle.Play();
        }
        else if (!IsGrounded && _isRollingAudioPlaying)
        {
            _isRollingAudioPlaying = _boulderRollingAudio.StopSound();
            _rollingParticle.Stop();
            _rollingParticle.Clear();
        }

        if(_playerState.State == EPlayerState.Dead && _boulderController.State != EBoulderState.Freefall)
        {
            _boulderController.ChangeStateToFreefall();
        }
    }

    private void FixedUpdate()
    {
        if(_playerState.State == EPlayerState.Victory && _rigidbody.velocity.magnitude <= 0.01)
        {
            _rigidbody.velocity = Vector2.zero;
        }

        if (IsGrounded)
        {

            //Calculate push acceleration
            Vector2 acceleration = Vector2.zero;
            acceleration += RollBoulder();
            //Calculate gravity into acceleration
            Vector2 gravity;
            if(_boulderController.State == EBoulderState.Freefall)
            {
                gravity = Vector2.down * _boulderController.Gravity;
            }
            else
            {
                gravity = -_groundNormal * _boulderController.Gravity;
            }

            //Apply calculated force
            _rigidbody.AddForce(gravity);
            _rigidbody.AddForce(acceleration);
        }
        else
        {
            _rigidbody.AddForce(Vector2.down * _boulderController.Gravity);
        }
        
        //Apply rotation to boulder to make it rotate
        if(_rigidbody.velocity.normalized.x < 0)
        {
            transform.Rotate(0, 0, _rigidbody.velocity.magnitude * (Time.fixedDeltaTime / 0.02f));        
        }
        else
        {
            transform.Rotate(0, 0, -_rigidbody.velocity.magnitude * (Time.fixedDeltaTime / 0.02f));
        }
        _rollingParticle.transform.rotation = Quaternion.Euler(0, 0, 0);
        ParticleSystem.MainModule rollingParticleMain = _rollingParticle.main;
        rollingParticleMain.startSpeed = _rigidbody.velocity.magnitude;
    }

    private void GroundCheck()
    {
        //Set variables to check ground angle
        ContactFilter2D layerFilter = new ContactFilter2D();
        layerFilter.SetLayerMask(_groundLayer);
        RaycastHit2D[] groundAngleHits = new RaycastHit2D[1];

        // fire ray to check for ground angle
        Physics2D.Raycast(transform.position, -Vector2.up, layerFilter, groundAngleHits, Mathf.Infinity);
        RaycastHit2D firstHit = groundAngleHits[0];

        //reset ground variables
        _groundNormal = Vector2.up;
        _isGrounded = false;

        //if the check passes that means there is ground underneath the boulder
        if (!firstHit) return;

        //set ground angle
        _groundNormal = firstHit.normal;

        //fire ray in the angle of the ground to check if boulder is grounded
        RaycastHit2D[] groundCollisionHits = new RaycastHit2D[1];
        Physics2D.Raycast(transform.position, -_groundNormal, layerFilter, groundCollisionHits, _groundCollisionDistance);
        firstHit = groundCollisionHits[0];

        if (!firstHit)
        {
            if (Time.timeSinceLevelLoad > _boulderFudgeTime + _lastGroundedTime)
            {
                return;
            }
            else
            {
                _isGrounded = true;
                _rollingParticle.transform.position = transform.position + Vector3.down * _physicsCollider.radius;
                return;
            }
        }
        _isGrounded = true;
        _lastGroundedTime = Time.timeSinceLevelLoad;
        _rollingParticle.transform.position = firstHit.point;
        if (_wasGrounded) return;

        //if it is boulder landing play landing vfx and slow down
        Vector2 velocity = _rigidbody.velocity;
        Vector2 targetVelocity = velocity * _slowDownPercentage;

        _rigidbody.AddForce((targetVelocity - velocity) / Time.fixedDeltaTime);
        _boulderLandingAudio.PlaySound();
        _landingParticle.transform.position = firstHit.point;
        _landingParticle.Play();
        _boulderController.IsCatchable = true;
    }

    private Vector2 RollBoulder()
    {
        //If the boulder is in catch or freefall state do not roll boulder forward
        if (_boulderController.State != EBoulderState.Roll) return Vector2.zero;
        
        Vector2 velocity = _rigidbody.velocity;
        Vector2 targetVelocity = new Vector2(_groundNormal.y * _playerMovement._currentRunSpeed, _groundNormal.x * -_playerMovement._currentRunSpeed);
        //return the acceleration needed to go to target vector
        return targetVelocity - velocity;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, -_groundNormal);
    }
}
