using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Scripts
{
    public class UnityEventHandler : MonoBehaviour
    {
        [Inject] List<IPause> PauseObjects;
        [Inject] List<IQuit> QuitObjects;
        
        void OnApplicationQuit()
        {
            foreach (var t in QuitObjects)
            {
                t.Quit();
            }
        }

        void OnApplicationPause(bool pause)
        {
            foreach (var t in PauseObjects)
            {
                t.Pause(pause);
            }
        }
        
        public interface IPause
        {
            void Pause(bool pause); 
        }

        public interface IQuit
        {
            void Quit();
        }
    }
}