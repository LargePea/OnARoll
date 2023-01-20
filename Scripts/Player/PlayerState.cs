using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerState : MonoBehaviour
{
    [SerializeField] private float _boulderStunTimer = 0f; //Set player stun in seconds
    [SerializeField] private float _checkDistance;
    [SerializeField] private Transform _checkOffset;
    [SerializeField] private LayerMask _layerMask;
    [SerializeField] private BoulderController _boulderController;
    [SerializeField] private UnityEvent _stopCatchGlow;
    [SerializeField] private UnityEvent _playerStun;

    private Coroutine _StunCo;
    private Rigidbody2D _rigidbody;
    private PlayerController _playerController;
    private ShaderFloatController _shaderFloatController;
    private float _collisionGraceTime = 0.04f;
    private EPlayerState _state;
    private bool _isStunned = false;

    //public pointers
    public EPlayerState State => _state;
    public bool IsStunned => _isStunned;
    public float TimeSinceLastThrown;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _playerController = GetComponent<PlayerController>();
        _shaderFloatController = GetComponent<ShaderFloatController>();
        _state = EPlayerState.Run;
        _isStunned = false;
    }

    private void Start()
    {
        ChangeStateToAwake();
    }

    private void OnEnable()
    {
        StartCoroutine(SpawnPlayer());
    }

    //check if player collided with the boulder
    //Player will handle if th boulder is freefalling onto player
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent(out BoulderController boulderController) && (_state == EPlayerState.Run || _state == EPlayerState.Catch))
        {
            if (Time.timeSinceLevelLoad <= TimeSinceLastThrown + _collisionGraceTime) return;
            boulderController.IsCatchable = false;
            StopPlayer();
            _stopCatchGlow.Invoke();
            //only stun when player is running
            if(boulderController.State == EBoulderState.Freefall)
            {
                if (_StunCo != null)
                {
                    StopCoroutine(_StunCo);
                }
                _StunCo = StartCoroutine(StunPlayer(_boulderStunTimer));
            }
            ChangeStateToRoll();

            //always change boulder state when colliding with player
            boulderController.ChangeStateToRoll();
            boulderController.StopBoulder();
        }
    }

    public void ChangeStateToRun()
    {
        _state = EPlayerState.Run;
        Debug.Log("Change state to run");
    }

    public void ChangeStateToThrow()
    {
        _state = EPlayerState.Throw;
        Debug.Log("Change state to throw");
    }

    public void ChangeStateToCatch()
    {
        _state = EPlayerState.Catch;
        Debug.Log("Change state to catch");
    }

    public void ChangeStateToRoll()
    {
        _state = EPlayerState.Roll;
        Debug.Log("Change state to roll");
    }

    public void ChangeStateToDead()
    {
        _state = EPlayerState.Dead;
    }
    public void ChangeStateToAwake()
    {
        _state = EPlayerState.Awake;
    }

    public void ChangeStateToVictory()
    {
        _state = EPlayerState.Victory;
    }

    public IEnumerator StunPlayer(float stunTime)
    {
        _isStunned = true;
        _playerStun.Invoke();
        yield return new WaitForSeconds(stunTime);
        _isStunned = false;
    }

    public void StopPlayer()
    {
        _rigidbody.velocity = Vector3.zero;
    }

    private IEnumerator SpawnPlayer()
    {
        yield return StartCoroutine(_shaderFloatController.IncreaseValue());

        ChangeStateToRun();
    }
}
