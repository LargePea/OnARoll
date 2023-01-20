using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TutorialNode : MonoBehaviour
{
    [SerializeField] private bool _finalTutorial;
    [SerializeField] private InputActionReference _actionReference;
    [SerializeField] private Sprite _sprite;
    [SerializeField, TextArea] private string _tutorialDescription;
    private TutorialUI tutorial;
    private bool _activated;

    private void Start()
    {
        _activated = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(tutorial == null)
        {
            tutorial = FindObjectOfType<TutorialUI>();
        }
        if (!TutorialUI.PlayTutorial || _activated || tutorial.TutorialActive || tutorial == null) return;

        PlayerController player = collision.GetComponentInParent<PlayerController>();
        BoulderController boulder = collision.GetComponentInParent<BoulderController>();

        if(player != null || boulder != null)
        {
            _activated = true;
            tutorial.SetupPrompt(_sprite, _tutorialDescription);
            tutorial.SubscribeEvent(_actionReference);
            if (_finalTutorial) TutorialUI.SetTutorialActive(false);
        }
    }
}
