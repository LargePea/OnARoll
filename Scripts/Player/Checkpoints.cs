using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoints : MonoBehaviour
{
    [SerializeField] private AudioSource _bgMusic;

    private static bool _usingCheckpoints = false;
    private static float _currentCheckpoint = Mathf.Infinity;
    private static float _musicTime;
    private static float _timer;

    public static bool UsingCheckpoints => _usingCheckpoints;
    public static float CurrentCheckpoint => _currentCheckpoint;
    public static float Timer => _timer;

    public void SetCheckpoint(float checkPointID)
    {
        _currentCheckpoint = checkPointID;
        _musicTime = _bgMusic.time;
    }

    public void RespawnAtCheckpoint(float checkPointID = Mathf.Infinity)
    {
        if(checkPointID != Mathf.Infinity)
        {
            _bgMusic.time = _musicTime;
        }
    }

    public void SavePlayerState(float timer)
    {
        _timer = timer;
    }

    public static void ResetCheckpointState()
    {
        _currentCheckpoint = Mathf.Infinity;
        _musicTime = 0;
        _timer = 0;
    }

    public static void ActivateCheckpoint(bool usingCheckpoints)
    {
        _usingCheckpoints = usingCheckpoints;
    }
}
