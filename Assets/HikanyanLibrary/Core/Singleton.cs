using System;

namespace HikanyanLibrary.Core
{
    public abstract class Singleton<T> : IDisposable where T : class
    {
        private static T _instance;
        private static readonly object _lockObject = new object();
        private static bool _isDisposed = false;

        public static T Instance
        {
            get
            {
                if (_instance != null)
                    return _instance;

                lock (_lockObject)
                {
                    if (_instance != null || _isDisposed) return _instance;
                    _instance = Activator.CreateInstance(typeof(T), true) as T;
                        
                    if (_instance is Singleton<T> singletonInstance)
                    {
                        singletonInstance.OnInstanceCreated();
                    }
                }

                return _instance;
            }
        }

        protected virtual void OnInstanceCreated() { }

        public static void ResetInstance()
        {
            lock (_lockObject)
            {
                if (_instance is IDisposable disposable)
                {
                    disposable.Dispose();
                }
                _instance = null;
                _isDisposed = false;
            }
        }

        public virtual void Dispose()
        {
            lock (_lockObject)
            {
                _isDisposed = true;
                _instance = null;
            }
        }
    }
}