using UnityEngine;
using Zenject;

public class TimeInstaller : MonoInstaller
{
    [SerializeField] private TimeManager _timeManager;
    public override void InstallBindings()
    {
        Container.Bind<TimeManager>().FromComponentInHierarchy().AsSingle();
        Container.Bind<DigitalClock>().FromComponentInHierarchy().AsSingle();
        Container.Bind<AnalogClock>().FromComponentInHierarchy().AsSingle();
    }
}
