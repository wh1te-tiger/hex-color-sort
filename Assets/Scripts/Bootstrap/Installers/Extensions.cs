using Leopotam.EcsLite;
using Zenject;

namespace Scripts
{
    public static class Extensions
    {
        public static void Add<TSystem>(this DiContainer container, InstallSettings installSettings, params object[] optionalArguments) where TSystem : IEcsSystem
        {
            container.BindInterfacesTo<TSystem>()
                .AsSingle()
                .WithConcreteId("system")
                .WithArguments(optionalArguments)
                .NonLazy();
            
            installSettings.Add<TSystem>();
        }
    }
}