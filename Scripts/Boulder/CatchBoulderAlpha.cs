using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CatchBoulderAlpha : MonoBehaviour
{
    //boulder states
    private BoulderController _boulderController;

    //Catch colliders
    private CircleCollider2D[] _catchColliders;
    private CircleCollider2D _perfectCatch;
    private CircleCollider2D _normalCatch;

    //catch refill variables
    [SerializeField, Range(0,1)] private float _normalCatchRefill = .5f;

    //catch events
    [SerializeField] private UnityEvent _perfectCatchEvent;
    [SerializeField] private UnityEvent _catchEvent;

    private void Awake()
    {
        _boulderController = GetComponentInParent<BoulderController>();
        _catchColliders = GetComponents<CircleCollider2D>();

        if(_catchColliders[0].radius > _catchColliders[1].radius)
        {
            _normalCatch = _catchColliders[0];
            _perfectCatch = _catchColliders[1];
        }
        else
        {
            _normalCatch = _catchColliders[1];
            _perfectCatch = _catchColliders[0];
        }
    }

    //Return values determine if the player caught the boulder or not
    public bool CatchManager(PlayerStrength playerStrength, Collider2D playerCollider)
    {

        if (_perfectCatch.IsTouching(playerCollider))
        {
            PerfectCatch(playerStrength);
            _perfectCatchEvent.Invoke();
            return true;
        }
        else if (_normalCatch.IsTouching(playerCollider))
        {
            _catchEvent.Invoke();
            Catch(playerStrength);
            return true;
        }

        return false;
    }

    //Only partially refill player strength bar
    private void Catch(PlayerStrength playerStrength)
    {
        playerStrength.RefillStrength(_normalCatchRefill);
        _boulderController.ChangeStateToCatch();
        _boulderController.IsCatchable = false;
    }

    //Fully refill player strength bar
    private void PerfectCatch(PlayerStrength playerStrength)
    {
        playerStrength.RefillStrength();
        _boulderController.ChangeStateToCatch();
        _boulderController.IsCatchable = false;
    }
}
