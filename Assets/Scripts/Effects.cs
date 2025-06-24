using UnityEngine;

public class Effects : MonoBehaviour
{
    [SerializeField] private ParticleSystem _flameEffect;
    [SerializeField] private Transform _point;

    public void StartFlameEffect()
    {
        Instantiate( _flameEffect.gameObject, _point.transform);
    }
}
