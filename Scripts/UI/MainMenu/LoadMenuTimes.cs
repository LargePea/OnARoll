using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LoadMenuTimes : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _text;
    private string[] _times = new string[3];

    private void Awake()
    {
        LoadTimes();
    }

    private void OnEnable()
    {
        LoadTimes();   
    }

    public void ResetHighScores()
    {
        SaveTime.ResetPlayerDataFile();
        LoadTimes();
    }

    private void LoadTimes()
    {
        PlayerTime playerTime = SaveTime.LoadTimes();
        if (playerTime == null)
        {
            _text.text = "Play to get times";
            return;
        }

        for (int time = 0; time < _times.Length; time++)
        {
            if (playerTime.times[time] != Mathf.Infinity)
            {
                float minutes = playerTime.times[time] / 60f;
                minutes = Mathf.FloorToInt(minutes);
                float seconds = playerTime.times[time] % 60f;
                _times[time] = string.Format("{0:00}:{1:00.00}", minutes, seconds);
            }
            else
            {
                _times[time] = "";
            }
        }
        _text.text = string.Format("1.{0}\n2.{1}\n3.{2}", _times[0], _times[1], _times[2]);
    }
}
