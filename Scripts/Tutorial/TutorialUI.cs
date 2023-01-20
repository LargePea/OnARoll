using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;
using UnityEngine.Audio;
using TMPro;
using UnityEngine.UI;

public class TutorialUI : MonoBehaviour
{
    private static bool _playTutorial = true;
    public static bool PlayTutorial => _playTutorial;

    [SerializeField, Range(0,1)] private float _timeSlowDown;
    [SerializeField] private AudioMixer _audioMixer;
    [SerializeField] private string _mixerParamName;
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private Image _image;

    private bool _tutorialActive;
    private InputActionReference _actionReference;
    private Animator _animator;
    private Coroutine _animationCoroutine;

    public bool TutorialActive => _tutorialActive;
    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }


    public void SubscribeEvent(InputActionReference actionReference)
    {
        _actionReference = actionReference;
        _actionReference.action.performed += ActionPerformed;
        if(_animationCoroutine != null)
        {
            StopCoroutine(_animationCoroutine);
        }
        _animationCoroutine = StartCoroutine(SetAnimator(true));
    }

    private void ActionPerformed(InputAction.CallbackContext obj)
    {
        if (!_tutorialActive) return;
        _actionReference.action.performed -= ActionPerformed;
        if (_animationCoroutine != null)
        {
            StopCoroutine(_animationCoroutine);
        }
        _animationCoroutine = StartCoroutine(SetAnimator(false));
    }


    private IEnumerator SetAnimator(bool _isActive)
    {
        _animator.SetBool("isActive", _isActive);
        if (_isActive)
        {
            yield return new WaitForSeconds(0.25f);
            _tutorialActive = true;
            Time.timeScale = _timeSlowDown;
            Time.fixedDeltaTime = Time.timeScale * 0.02f;

        }
        else
        {
            _tutorialActive = false;
            Time.timeScale = 1;
            Time.fixedDeltaTime = Time.timeScale * 0.02f;
        }

        _audioMixer.SetFloat(_mixerParamName, Time.timeScale);
    }

    public void SetupPrompt(Sprite sprite, string tutorialDescription)
    {
        _image.sprite = sprite;
        _text.text = tutorialDescription;
    }

    public static void SetTutorialActive(bool isActive)
    {
        _playTutorial = isActive;
    }
}
