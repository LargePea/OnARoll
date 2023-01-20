using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
#pragma warning disable 649

public enum EPlayerState
{
    Awake,
    Run,
    Roll,
    Catch,
    Throw,
    Dead,
    Victory
}

// sends input from PlayerInput to attached CharacterMovement components
public class PlayerController : MonoBehaviour
{
    // initial cursor state
    [SerializeField] private CursorLockMode _cursorMode = CursorLockMode.Locked;
    // make character look in Camera direction instead of MoveDirection
    [SerializeField] private bool _lookInCameraDirection;
    //Get Boulder
    [SerializeField] private GameObject _boulder;
    // Set boulder throw charge time
    [SerializeField] private float _boulderChargeTime;
    public float BoulderChargeTime => _boulderChargeTime;
    //Get strength ui to trigger decrease
    [SerializeField] private StrengthBar _strengthBar;
    [SerializeField] private float _clickCooldown = 0.5f;

    //Player scripts
    private PlayerMovement _playerMovement;
    private PlayerStrength _playerStrength;
    private PlayerState _playerStateController;
    private Animator _animator;

    //Camera scripts
    [SerializeField] private CameraSpeedUpSlowDown _cameraSpeedUpSlowDown;

    //Boulder scripts
    private CatchBoulderAlpha _catchBoulder;
    private BoulderThrow _boulderThrow;
    private BoulderController _boulderController;

    //Things
    private Vector2 _moveInput;
    private bool _jumpButtonReleased = true;
    private float _fireHoldStartTime;
    private float _clickCooldownTime;
    public float ClickCooldownTime => _clickCooldownTime;

    //Player physics Variable
    private Collider2D _playerCollider;

    //Player audio variables
    private PlayerThrowAudio _playerThrowAudio;

    private void Awake()
    {
        //Get player components
        _playerMovement = GetComponent<PlayerMovement>();
        _playerCollider = GetComponentInChildren<Collider2D>();
        _playerStrength = GetComponent<PlayerStrength>();
        _playerStateController = GetComponent<PlayerState>();

        //get boulder components
        _catchBoulder = _boulder.GetComponentInChildren<CatchBoulderAlpha>();
        _boulderThrow = _boulder.GetComponent<BoulderThrow>();
        _boulderController = _boulder.GetComponent<BoulderController>();
        Cursor.lockState = _cursorMode;

        //Get audio components
        _playerThrowAudio = GetComponentInChildren<PlayerThrowAudio>();

        //get animator
        _animator = GetComponentInChildren<Animator>();
    }

    public void OnMove(InputValue value)
    {
        _moveInput = value.Get<Vector2>();
        _playerMovement?.ChangeSpeed(_moveInput.x);
        _cameraSpeedUpSlowDown?.OffsetCamera(_moveInput.x);
    }

    public void OnJump(InputValue value)
    {
        _jumpButtonReleased = !_jumpButtonReleased;
        if (_playerStateController.State != EPlayerState.Run) return;

        if (_jumpButtonReleased)
        {
            _playerMovement?.ShortJump();
        }
        else
        {
            _playerMovement?.Jump();
        }
    }

    public void OnCatch(InputValue value)
    {
        if (_playerStateController.State == EPlayerState.Dead || Time.timeSinceLevelLoad <= _clickCooldownTime) return;
        _clickCooldownTime = Time.timeSinceLevelLoad + _clickCooldown;
        //catch the boulder only if the boulder is not caught and being used by the player and if the player is grounded
        if (_playerStateController.State == EPlayerState.Run && _playerMovement.IsGrounded)
        {
            if (_catchBoulder.CatchManager(_playerStrength, _playerCollider))
            {
                _playerStateController.ChangeStateToCatch();
            }
        }
    }

    public void OnFireStart(InputValue value)
    {
        //dont do anything when the player is dead
        if (_playerStateController.State == EPlayerState.Dead || Time.timeSinceLevelLoad <= _clickCooldownTime) return;
        _clickCooldownTime = Time.timeSinceLevelLoad + _clickCooldown;
        //throw the boulder only when the player is rolling the boulder
        if (_playerStateController.State == EPlayerState.Roll && _playerMovement.IsGrounded)
        {
            _fireHoldStartTime = Time.timeSinceLevelLoad;
            _playerStateController.ChangeStateToThrow();
            _strengthBar.StartCharge(_fireHoldStartTime);
        }
    }

    public void OnFireStop(InputValue value)
    {
        
        if (_playerStateController.State == EPlayerState.Dead || _playerStateController.State != EPlayerState.Throw) return;

        //calculate how much strength is used
        float strength = Mathf.Clamp01((Time.timeSinceLevelLoad - _fireHoldStartTime) / _boulderChargeTime);
        _strengthBar.EndCharge();

        //throw boulder and use strength with recalculated strength
        if (_boulderThrow.ThrowBoulder(_playerStrength.UseStrength(strength)))
        {
            _playerStateController.TimeSinceLastThrown = Time.timeSinceLevelLoad;
            _playerThrowAudio.PlaySound();
            _playerStateController.ChangeStateToRun();
        }
        else
        {
            _playerStateController.ChangeStateToRoll();
        }
    }

    public void OnFireCharged(InputValue value)
    {
        
        if (_playerStateController.State == EPlayerState.Dead || _playerStateController.State != EPlayerState.Throw) return;

        //throw boulder and use strength with recalculated strength
        _strengthBar.EndCharge();
        if (_boulderThrow.ThrowBoulder(_playerStrength.UseStrength()))
        {
            _playerStateController.TimeSinceLastThrown = Time.timeSinceLevelLoad;
            _playerThrowAudio.PlaySound();
            _playerStateController.ChangeStateToRun();
        }
        else
        {
            _playerStateController.ChangeStateToRoll();
        }
    }
}
