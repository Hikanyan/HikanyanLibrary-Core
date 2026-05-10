using UnityEngine;

namespace HikanyanLibrary.Core
{
    [DisallowMultipleComponent]
    public abstract class SingletonMonoBehaviour<T> : MonoBehaviour where T : SingletonMonoBehaviour<T>
    {
        private static T _instance;
        private static bool _isInitialized = false;

        public static T Instance
        {
            get
            {
                if (_instance != null)
                    return _instance;

                _instance = FindFirstObjectByType<T>();

                if (_instance == null)
                {
                    GameObject singletonObject = new GameObject(typeof(T).Name);
                    _instance = singletonObject.AddComponent<T>();
                }

                if (!_isInitialized)
                {
                    _isInitialized = true;
                    _instance.OnInstanceCreated();
                }

                return _instance;
            }
        }

        protected virtual void OnInstanceCreated() { }

        /// <summary>
        /// DontDestroyOnLoadを適用するかどうかを決定します
        /// デフォルトはtrue。継承先でオーバーライドして制御してください
        /// </summary>
        protected virtual bool IsPersistent => true;

        protected virtual void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
                return;
            }

            _instance = this as T;

            if (!_isInitialized)
            {
                _isInitialized = true;
                OnInstanceCreated();
            }

            if (IsPersistent)
            {
                DontDestroyOnLoad(gameObject);
            }
        }

        protected virtual void OnDestroy()
        {
            if (_instance == this)
            {
                _instance = null;
                _isInitialized = false;
            }
        }

        public static void ResetInstance()
        {
            if (_instance != null)
            {
                Destroy(_instance.gameObject);
            }
            _instance = null;
            _isInitialized = false;
        }
    }
}