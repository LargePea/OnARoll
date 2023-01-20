using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#pragma warning disable 649

public class PlayerMovement : MonoBehaviour
{
    //Instance editable movement variable
    [Header("Movement")]
    [SerializeField] private float _runSpeed;
    [SerializeField] private float _jumpDistance;
    [SerializeField] private float _gravity;
    [SerializeField] private float _shortJumpSlowDownSpeed;
    [SerializeField] private float _minjumpDistance;
    [SerializeField] private float _speedModifier;

    //Editable grounding variables
    [Header("Ground Check")]
    [SerializeField] private Transform _groundCheckStart;
    [SerializeField] private float _groundCheckDistance;
    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private float jumpVelocity => Mathf.Sqrt(-2 * _gravity * _jumpDistance);

    //Jump/Physics variables
    public bool IsGrounded;
    private bool _wasGrounded;
    private Rigidbody2D _rigidbody;

    //Run speed variables
    private Vector3 NormalizeGround;
    private PlayerState _state;
    private float _angle;
    public float _currentRunSpeed { get; private set; }
    public Vector3 Velocity => _rigidbody.velocity;

    //Particle Variables
    [SerializeField] private ParticleSystem _walkParticle;
    [SerializeField] private ParticleSystem _landingParticle;

    //Audio Variables
    [SerializeField] private AudioSource _landingAudio;
    [SerializeField] private AudioSource _jumpingAudio;

    private void Awake()
    {
        _wasGrounded = true;
        _rigidbody = GetComponent<Rigidbody2D>();
        _state = GetComponent<PlayerState>();
        _currentRunSpeed = _runSpeed;
    }

    private void Update()
    {
        IsGrounded = CheckGround();
        _wasGrounded = IsGrounded;
        if (!IsGrounded)
        {
            _walkParticle.Clear();
            _landingParticle.Clear();
        }
    }

    private void FixedUpdate()
    {
        if(_state.State == EPlayerState.Dead)
        {
            if (!IsGrounded)
            {
                _rigidbody.AddForce(Vector2.up * _gravity);
                return;
            }
            _rigidbody.velocity = Vector2.zero;
            return;
        }
        Vector3 acceleration = Vector3.zero; 
        //if the player is not in the air update it's horizontal acceleration
        if (IsGrounded)
        {
            //Update player velocity to match angle of floor to have consistent 
            Vector2 velocity = _rigidbody.velocity;
            Vector2 targetVelocity = new Vector2(Mathf.Cos(_angle) * _currentRunSpeed, Mathf.Sin(_angle) * _currentRunSpeed);
            Vector2 velocityDiff = (targetVelocity - velocity);
            acceleration = velocityDiff;
        }
        //Add gravity to acceleration
        acceleration += NormalizeGround * _gravity;
        _rigidbody.AddForce(acceleration);
    }

    public void Jump()
    {
        if (IsGrounded)
        {
            _jumpingAudio.Play();
            //Change y velocity
            _rigidbody.velocity += new Vector2(0f, jumpVelocity);

        }
    }
    public void ShortJump()
    {
        if (!IsGrounded && _rigidbody.velocity.y > 0)
        {
            
            Vector2 targetVelocity;
            Vector2 acceleration;

            //Check if player has jumped the minimum distance
            float deltaDistanceToMin = _minjumpDistance - ((_rigidbody.velocity.y * _rigidbody.velocity.y - jumpVelocity * jumpVelocity) / (2 * _gravity));

            if(deltaDistanceToMin > 0)
            {
                //Add additional velocity needed to reach min jump distance
                targetVelocity = new Vector2(_rigidbody.velocity.x, Mathf.Sqrt(-2 * _gravity * deltaDistanceToMin));
                acceleration = (targetVelocity - _rigidbody.velocity);
            }
            else
            {
                //slow player down 
                targetVelocity = new Vector2(_rigidbody.velocity.x, _shortJumpSlowDownSpeed);
                acceleration = targetVelocity.y < _rigidbody.velocity.y ? (targetVelocity - _rigidbody.velocity) : Vector2.zero;
            }

            _rigidbody.AddForce(acceleration, ForceMode2D.Impulse);
        }
    }

    private bool CheckGround()
    {
        //set variables needed for raycast
        ContactFilter2D layerFilter = new ContactFilter2D();
        layerFilter.SetLayerMask(_groundLayer);
        RaycastHit2D[] hits = new RaycastHit2D[1];

        //send out ray and return first hit
        Physics2D.Raycast(_groundCheckStart.position, -transform.up, layerFilter, hits, _groundCheckDistance);
        RaycastHit2D firsthit = hits[0];

        //reset angle of player
        NormalizeGround = Vector2.up;
        _angle = Vector2.Angle(transform.up, NormalizeGround) * Mathf.Deg2Rad;

        if (!firsthit) return false;

        //set new angle of the floor that the player is walking on
        NormalizeGround = firsthit.normal;
        _angle = Vector2.Angle(transform.up, NormalizeGround) * Mathf.Deg2Rad;

        //Prevent player from slowing down from landing
        if (!IsGrounded) _rigidbody.velocity = new Vector2(Mathf.Cos(_angle) * _currentRunSpeed, Mathf.Sin(_angle) * _currentRunSpeed);

        //Check if player was grounded from the previous check, if not the play landing sound and particle
        if (!_wasGrounded)
        {
            _landingParticle.Play();
            _landingAudio.Play();
        }

        return true;
    }

    public void ChangeSpeed(float speedModifier = 0, bool startingSpeedChange = false)
    {
        //Increase or decrease speed depending on what player inputs
        if(speedModifier != 0)
        {
            
            _currentRunSpeed += speedModifier * _speedModifier;
            return;
        }
        if (startingSpeedChange)
        {
            _currentRunSpeed = 0;
            return;
        }
        _currentRunSpeed = _runSpeed;
    }
}
