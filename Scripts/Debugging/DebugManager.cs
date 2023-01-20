using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DebugManager : MonoBehaviour
{
    [SerializeField] private GameObject _player;
    [SerializeField] private GameObject _boulder;

    [Header("Teleport Points")]
    [SerializeField] private DebugTpPoint[] debugTpPoints;

    //teleport point logic variables
    private int _tpIndex = 0;

    public static bool Invulnerability { get; private set; } = false;
    public static bool InfiniteStrength { get; private set; } = false;
    private static bool _stopTime;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Invulnerability = !Invulnerability;
            Debug.Log($"Invulnerable: {Invulnerability}");
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            InfiniteStrength = !InfiniteStrength;
            Debug.Log($"Infinite Strength: {InfiniteStrength}");
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            if (_tpIndex - 1 >= 0)
            {
                _tpIndex--;
            }
            Debug.Log($"Teleporting to Tp point: {_tpIndex}");
            debugTpPoints[_tpIndex].Tp(_player, _boulder);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            if(_tpIndex + 1 < debugTpPoints.Length)
            {
                _tpIndex++;
            }
            Debug.Log($"Teleporting to Tp point: {_tpIndex}");
            debugTpPoints[_tpIndex].Tp(_player, _boulder);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            _stopTime = !_stopTime;
            if (!_stopTime)
            {
                Time.timeScale = 0f;
            }
            else
            {
                Time.timeScale = 1f;
            }

        }
        else if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            SceneManager.LoadScene("LDFirstLevelPass");
        }
    }

    public void OverrideCurrentTpPoint(DebugTpPoint debugTpPoint)
    {
        _tpIndex = Array.IndexOf(debugTpPoints, debugTpPoint);
    }
}
