using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoulderThrow : MonoBehaviour
{
    //Boulder throw metrics
    [SerializeField] private float _throwDistanceMax;
    [SerializeField] private float _throwHeightMax;
    [SerializeField] private float _throwDistanceMin = 1f;
    [SerializeField] private float _throwHeightMin = 1f;

    private float _deltaThrowHeight => _throwHeightMax - _throwHeightMin;
    private float _deltaThrowDistance => _throwDistanceMax - _throwDistanceMin;

    private BoulderController _boulderController;
    private Rigidbody2D _rigidbody;
    private Vector2 throwVelocity;

    private void Awake()
    {
        _boulderController = GetComponent<BoulderController>();
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    public bool ThrowBoulder(float throwStrength)
    {
        if (_boulderController.State != EBoulderState.Roll || throwStrength < 0.001) return false;

        _boulderController.ChangeStateToFreefall();
        //Set horizontal and vertical inital velocities
        throwVelocity.y = Mathf.Sqrt(2 * _boulderController.Gravity * ((_deltaThrowHeight * throwStrength) + _throwHeightMin));
        throwVelocity.x = ((_deltaThrowDistance * throwStrength) + _throwDistanceMin) / Mathf.Sqrt((2 * ((_deltaThrowHeight * throwStrength) + _throwHeightMin)) / _boulderController.Gravity);


        //Apply the velocity
        _rigidbody.velocity += throwVelocity;
        return true;
    }
}
