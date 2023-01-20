using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleTutorial : MonoBehaviour
{

    private void OnEnable()
    {
        if (TryGetComponent(out Toggle toggle))
        {
            
            toggle.isOn = TutorialUI.PlayTutorial;
        }
    }

    public void SetValue(bool value)
    {
        TutorialUI.SetTutorialActive(value);
    }
}
