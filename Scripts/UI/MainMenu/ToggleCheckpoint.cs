using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleCheckpoint : MonoBehaviour
{
    private void OnEnable()
    {
        if (TryGetComponent(out Toggle toggle))
        {

            toggle.isOn = Checkpoints.UsingCheckpoints;
        }
    }

    public void SetValue(bool value)
    {
        Checkpoints.ActivateCheckpoint(value);
    }
}
