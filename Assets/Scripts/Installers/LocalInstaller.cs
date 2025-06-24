using Zenject;

public class ManualTimeSetterInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<ManualTimeSetter>().FromComponentInHierarchy().AsTransient();
    }
}
