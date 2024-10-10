using System;
using System.Collections.Generic;
using System.Linq;
using Leopotam.EcsLite;
using Zenject;

namespace Scripts
{
    public class MessageTransporter : IInitializable, ITickable, IFixedTickable, IDisposable
    {
        private readonly InstallSettings _installSettings;

        private EcsSystems _systems;
        private EcsSystems _runSystems;

        public MessageTransporter(InstallSettings installSettings)
        {
            _installSettings = installSettings;
        }
        
        public void Initialize()
        {
            var systems = _installSettings.Container().ResolveAll<IEcsSystem>();
            var world = _installSettings.Container().TryResolve<EcsWorld>();
            _runSystems = new EcsSystems(world);
            systems.OrderBy(x => GetInfo(x, _installSettings).OrderIndex)
                .ToList()
                .ForEach(x =>
                {
                    _runSystems.Add(x);
                });
            
            _runSystems.Init();
            
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
        }

        public void Dispose()
        {
            _runSystems?.Destroy();
        }
    }
}