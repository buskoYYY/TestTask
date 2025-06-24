using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class WarningUI : MonoBehaviour
{
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private ManualTimeSetter _timeSetter;
    [SerializeField] private TweenAniamations _tweenAniamations;
    [SerializeField] private Image _target;
    [SerializeField] private AudioManager _audioManager;
    [SerializeField] private float _totalDuration;

    private float _punchDuration;

    private void OnEnable()
    {
        _timeSetter.SetValidDate += StartWarningPanelAnimation;
    }

    private void Start()
    {
        _punchDuration = _tweenAniamations.PunchDuration;
    }

    private void OnDisable()
    {
        _timeSetter.SetValidDate -= StartWarningPanelAnimation;
    }

    private void StartWarningPanelAnimation()
    {
        _tweenAniamations.Show(_canvasGroup);
        StartCoroutine(PlayPunchAnimation());
    }
    private IEnumerator PlayPunchAnimation()
    {
        float elapsedTime = 0f;

        while (elapsedTime < _totalDuration)
        {
            _tweenAniamations.PunchScale(_target);
            _audioManager.PlayDadngerSound();
            yield return new WaitForSeconds(_punchDuration);
            elapsedTime += _punchDuration;

            if (elapsedTime >= _totalDuration)
            {
                _tweenAniamations.Hide(_canvasGroup);
                _audioManager.StopDangerSound();
            }
        }
    }
}
