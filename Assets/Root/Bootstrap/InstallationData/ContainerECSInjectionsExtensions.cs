using Leopotam.EcsLite;
using Zenject;

namespace Root
{
    public static class ContainerEcsInjectionsExtensions
    {
        public static void Add<TSystem>(this DiContainer container, InstallSettings installSettings, params object[] optionalArguments) where TSystem : IEcsSystem
        {
            container.Bind<TSystem>()
                .AsSingle()
                .WithConcreteId("system")
                .WithArguments(optionalArguments)
                .NonLazy();
            
            installSettings.Add<TSystem>();
        }
    }
}