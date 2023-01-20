using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lightning : MonoBehaviour
{
    [SerializeField] private LayerMask _groundMask;
    [SerializeField] private float _lightningBoltDuration;
    
    private Vector3 _relativeGroundLocation;
    private Transform[] _lightning;
    private ParticleSystem[] _lightningParticleSystem;
    private Collider2D lightningHitBox;
    private LightningAudio _lightningAudio;
    private float _previousStrikeTime = Mathf.Infinity;
    private float _delayTime;


    private void Awake()
    {
        _lightning = GetComponentsInChildren<Transform>();
        _lightningParticleSystem = GetComponentsInChildren<ParticleSystem>();
        lightningHitBox = GetComponentInChildren<Collider2D>();
        _lightningAudio = GetComponentInParent<LightningAudio>();
    }

    private void Update()
    {
        if (Time.timeSinceLevelLoad >= _previousStrikeTime)
        {
            lightningHitBox.enabled = true;
        }

        if(Time.timeSinceLevelLoad >= _previousStrikeTime + _lightningBoltDuration)
        {
            lightningHitBox.enabled = false;
        }
    }

    private void FixedUpdate()
    {
        ContactFilter2D layerFilter = new ContactFilter2D();
        layerFilter.SetLayerMask(_groundMask);
        RaycastHit2D[] hits = new RaycastHit2D[1];

        Physics2D.Raycast(transform.position, -Vector3.up, layerFilter, hits, Mathf.Infinity);
        RaycastHit2D firsthit = hits[0];

        if (!firsthit) return;

        Vector3 groundLocation = firsthit.point;
        _relativeGroundLocation = transform.InverseTransformPoint(groundLocation);

        _lightning[1].transform.localPosition = _relativeGroundLocation;
    }

    public void Strike(float chargeTime)
    {
        //Set spark playtime
        float duration = chargeTime * _lightningParticleSystem[0].main.simulationSpeed;
        ParticleSystem.MainModule mainModuleCharge = _lightningParticleSystem[0].main;
        mainModuleCharge.duration = duration;

        //set delayTime
        _delayTime = _lightningParticleSystem[0].main.startDelay.constant / _lightningParticleSystem[0].main.simulationSpeed;

        //set lighting bolt strike delay
        ParticleSystem.MainModule mainModuleBolt = _lightningParticleSystem[1].main;
        mainModuleBolt.startDelay = chargeTime + _delayTime;

        _previousStrikeTime = Time.timeSinceLevelLoad + chargeTime + _delayTime;
        _lightningParticleSystem[0].Play();
        StartCoroutine(_lightningAudio.PlaySound(chargeTime, _delayTime));
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawRay(transform.position, -transform.up);
    }
}
