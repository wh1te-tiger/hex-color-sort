using System;
using System.Collections.Generic;
using Zenject;

namespace Scripts
{
    public class InstallSettings
    {
        public Func<DiContainer> Container;
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
    
    public class SystemInstallInfo
    {
        public int OrderIndex;
        public Type SystemType;
    }
}