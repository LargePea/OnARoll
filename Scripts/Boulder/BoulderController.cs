using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoulderController : MonoBehaviour
{
    //Boulder states
    private EBoulderState _state;
    public EBoulderState State => _state;
    public bool IsCatchable;

    //Boulder physics
    [SerializeField] private float _gravity;
    public float Gravity => _gravity;
    private Rigidbody2D _rigidbody;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _state = EBoulderState.Freefall;
    }

    public void ChangeStateToFreefall()
    {
        _state = EBoulderState.Freefall;
    }

    public void ChangeStateToCatch()
    {
        _state = EBoulderState.Catch;
    }


    public void ChangeStateToRoll()
    {
        _state = EBoulderState.Roll;
    }

    public void StopBoulder()
    {
        _rigidbody.velocity = Vector3.zero;
    }
}
