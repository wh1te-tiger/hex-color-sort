using System.Collections.Generic;
using Zenject;

namespace Root
{
    public class InstallSettings
    {
        public System.Func<DiContainer> Container;
        public List<SystemInstallInfo> SystemInstallInfos = new();
        
        private int _currentOrderIndex;
        
        public void Add<T>()
        {
            SystemInstallInfos.Add(new SystemInstallInfo()
            {
                OrderIndex = _currentOrderIndex++,
                SystemType = typeof(T)
            });
        }
    }
}