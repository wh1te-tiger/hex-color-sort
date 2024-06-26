using System;
using System.Linq;
using Leopotam.EcsLite;
using Zenject;

namespace Root
{
    public class ZenjectToEcsMessagesTransporter : IInitializable, ITickable, IFixedTickable, IDisposable
    {
        [Inject] InstallSettings installSettings;

        private EcsSystems _systems;
        private EcsSystems _runSystems;
        private EcsSystems _fixedRunSystems;
        
        public void Initialize()
        {
            var world = installSettings.Container().TryResolve<EcsWorld>();
            _runSystems = new EcsSystems(world);
            _fixedRunSystems = new EcsSystems(world);
            
            var systemTypes = typeof(IEcsSystem).GetChildTypes();
            systemTypes.Select(x => installSettings.Container().TryResolve(x) as IEcsSystem)
                .Where(x => x != null)
                .OrderBy(x => GetInfo(x, installSettings).OrderIndex)
                .ToList()
                .ForEach(x =>
                {
                    switch (x)
                    {
                        case IEcsFixedRunSystem:
                            _fixedRunSystems.Add(x);
                            break;
                        default:
                            _runSystems.Add(x);
                            break;
                    }
                });
            
            _runSystems.Init();
            _fixedRunSystems.Init();
            
            return;

            SystemInstallInfo GetInfo(IEcsSystem x, InstallSettings installInfo)
            {
                return installInfo.SystemInstallInfos.Find(info => info.SystemType == x.GetType());
            }
        }

        public void Tick()
        {
            _runSystems?.Run();
        }

        public void FixedTick()
        {
            _fixedRunSystems?.Run();
        }

        public void Dispose()
        {
            _runSystems?.Destroy();
            _fixedRunSystems?.Destroy();
        }
    }
}