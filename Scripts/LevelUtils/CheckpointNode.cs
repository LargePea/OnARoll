using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointNode : MonoBehaviour
{
    [SerializeField] private float _iD;

    public float ID => _iD;
    [SerializeField] CapsuleCollider2D _playerCollider;
    [SerializeField] CircleCollider2D _boulderCollider;
    [SerializeField] float _tpBuffer = 0.1f;

    [SerializeField] private LayerMask _groundMask;

    private Vector3 _groundPerpendicular;
    private Vector3 _playerTpPoint;
    private Vector3 _boulderTpPoint;
    private Vector3 _groundHitPoint;

    private void Awake()
    {
        //find ground for player to spawn on
        GetGround();

        //set tp points
        SetTpPoints();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out BoulderController controller))
        {
            Checkpoints checkpoints = FindObjectOfType<Checkpoints>();
            checkpoints.SetCheckpoint(_iD);
        }
    }

    public void SetSpawnPoint(GameObject player, GameObject boulder)
    {
        player.transform.position = _playerTpPoint;
        boulder.transform.position = _boulderTpPoint;
    }

    private void GetGround()
    {
        //Set variables to check ground angle
        ContactFilter2D layerFilter = new ContactFilter2D();
        layerFilter.SetLayerMask(_groundMask);
        RaycastHit2D[] groundAngleHits = new RaycastHit2D[1];

        // fire ray to check for ground angle
        Physics2D.Raycast(transform.position, -Vector2.up, layerFilter, groundAngleHits, Mathf.Infinity);
        RaycastHit2D firstHit = groundAngleHits[0];

        //reset ground variables
        Vector3 _groundNormal = Vector2.up;
        _groundPerpendicular = Vector2.right;
        _playerTpPoint = transform.position;
        _boulderTpPoint = transform.position + Vector3.right * _boulderCollider.radius;

        if (!firstHit) return;

        //set ground hit point
        _groundHitPoint = firstHit.point;
        //set ground angle
        _groundNormal = firstHit.normal;
        //find perpendicular of ground angle
        _groundPerpendicular = new Vector2(_groundNormal.y, -_groundNormal.x);
    }

    private void SetTpPoints()
    {
        //set player tp point
        _playerTpPoint = _groundHitPoint;
        _playerTpPoint.y += _tpBuffer;

        //set boulder tp point
        _boulderTpPoint = _groundHitPoint + (_groundPerpendicular * ((_playerCollider.size.x / 2) + _boulderCollider.radius + _tpBuffer));
        _boulderTpPoint.y += _boulderCollider.radius + _tpBuffer;
    }
}
