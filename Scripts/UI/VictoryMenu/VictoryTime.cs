using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VictoryTime : MonoBehaviour
{
    [SerializeField] private Timer timer;
    [SerializeField] private TMPro.TextMeshProUGUI _text;

    public void SetFinalTime()
    {
        _text.text = string.Format("Total Time:\n {0:00}:{1:00.00}s", timer.Minutes, timer.Seconds);
    }
}
