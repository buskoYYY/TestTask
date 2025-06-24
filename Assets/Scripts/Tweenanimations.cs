using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class TweenAniamations : MonoBehaviour
{
    private readonly float _alphaWhenOff = 0f;
    private readonly float _alphaWhenOn = 1f;

    [SerializeField] private float _duration = 1f;
    [SerializeField] private Image _target;
    [SerializeField] private Vector3 _punch = new Vector3(0.5f, 0.5f, 0.5f);
    [SerializeField] private float _punchDuration = 0.8f;
    [SerializeField] private int _vibrato;
    [SerializeField] private float _elacity;
    [SerializeField] private float _totalDuration;

    public float PunchDuration { get { return _punchDuration; } private set { } }

    public void Show(CanvasGroup canvasGroup)
    {
        canvasGroup.DOFade(_alphaWhenOn, _duration).SetUpdate(true);
    }

    public void Hide(CanvasGroup canvasGroup)
    {
        canvasGroup.DOFade(_alphaWhenOff, _duration);
    }

    public void PunchScale(Image image)
    {
        image.transform.DOPunchScale(_punch, _punchDuration, _vibrato, _elacity);
    }
}
