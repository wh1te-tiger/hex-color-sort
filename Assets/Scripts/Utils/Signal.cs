using System;

namespace Scripts
{
    public class Signal : IDisposable
    {
        private Action _action;

        public Signal(Action action)
        {
            this._action = action;
        }

        public Signal()
        {
            _action = delegate { };
        }

        public void Invoke()
        {
            _action?.Invoke();
        }

        public void Subscribe(Action value)
        {
            _action += value;
        }

        public void Dispose()
        {
            _action = null;
        }
    }
}