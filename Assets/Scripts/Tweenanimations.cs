/*using DG.Tweening;
using TMPro;
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

    private void OnEnable()
    {
        _enemy.EnemyDied += PunchScale;
    }
    private void OnDisable()
    {
        _enemy.EnemyDied -= PunchScale;
    }
    public void Show(CanvasGroup _canvasGroup)
    {
        _canvasGroup.DOFade(_alphaWhenOn, _duration).SetUpdate(true);
    }

    public void Hide(CanvasGroup _canvasGroup)
    {
        _canvasGroup.DOFade(_alphaWhenOff, _duration);
    }

    private void PunchScale()
    {
        _target.gameObject.transform.DOPunchScale(_punch, _punchDuration, _vibrato, _elacity);
    }
}*/
