using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class CharacterAnimations : MonoBehaviour
{
    [SerializeField] private GameObject _playerAnimation;
    [SerializeField] private string _victoryLayer;

    private Animator _animator;
    private PlayerState _playerState;
    private PlayerMovement _playerMovement;
    private Rigidbody2D _rigidbody2D;
    private bool victoryDance = false;

    private void Awake()
    {
        _animator = GetComponentInChildren<Animator>();
        _playerState = GetComponent<PlayerState>();
        _playerMovement = GetComponent<PlayerMovement>();
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        _animator.SetBool("IsGrounded", _playerMovement.IsGrounded);

        _animator.SetBool("IsPushing", _playerState.State == EPlayerState.Roll);

        _animator.SetBool("IsDead", _playerState.State == EPlayerState.Dead);

        _animator.SetBool("IsCatching", _playerState.State == EPlayerState.Catch);

        _animator.SetBool("IsThrowing", _playerState.State == EPlayerState.Throw);

        _animator.SetBool("Victory", _playerState.State == EPlayerState.Victory && (float)Math.Round(_rigidbody2D.velocity.magnitude, 1) <= 0.001);

        if (_animator.GetBool("Victory") && !victoryDance)
        {
            _playerMovement.ChangeSpeed(0, true);
            SetAllLayers(gameObject);
            _playerAnimation.transform.rotation = Quaternion.Euler(0, 180, 0);
            _animator.SetFloat("VictoryDance", UnityEngine.Random.Range(0, 2));
            victoryDance = true;
        }
    }

    private void SetAllLayers(GameObject obj)
    {
        obj.layer = LayerMask.NameToLayer(_victoryLayer);

        foreach (Transform child in obj.transform)
        {
            SetAllLayers(child.gameObject);
        }
    }
}