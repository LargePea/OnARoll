using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CatchTelegraph : MonoBehaviour
{
    [SerializeField] private UnityEvent _catchGlow;
    [SerializeField] private UnityEvent _catchUnglow;

    private BoulderController _boulderController;

    private void Awake()
    {
        _boulderController = GetComponentInParent<BoulderController>();
    }

    //telegraph when player can catch the boulder
    private void OnTriggerStay2D(Collider2D collision)
    {
        PlayerMovement playerMovement = collision.GetComponentInParent<PlayerMovement>();
        PlayerController playerController = collision.GetComponentInParent<PlayerController>();
        PlayerState playerState = collision.GetComponentInParent<PlayerState>();
        if (playerMovement != null && playerMovement.IsGrounded && Time.timeSinceLevelLoad >= playerController.ClickCooldownTime && playerState.State == EPlayerState.Run && _boulderController.IsCatchable)
        {
            _catchGlow.Invoke();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        PlayerMovement playerMovement = collision.GetComponentInParent<PlayerMovement>();
        if (playerMovement != null)
        {
            _catchUnglow.Invoke();
        }
    }
}
