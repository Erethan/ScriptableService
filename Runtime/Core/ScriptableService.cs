using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Erethan.ScriptableServices
{
    public abstract class ScriptableService<TBehaviour> : ScriptableObject, IService where TBehaviour : ScriptableServiceBehaviour
    {
        public virtual bool DestroyOnLoad { get; } = true;

        private TBehaviour InstantiateNewBehaviour() 
        {
            TBehaviour instance = new GameObject()
                .AddComponent<TBehaviour>();

            if(DestroyOnLoad)
            {
                DontDestroyOnLoad(instance.gameObject);
            }

            instance.gameObject.name = $"{typeof(TBehaviour)}";
            //instance.Initialize();
            return instance;
        }

        private TBehaviour _controllerBehaviour;
        protected TBehaviour ControllerBehaviour
        {
            get
            {
                if (_controllerBehaviour == null)
                {
                    Startup();
                }
                return _controllerBehaviour;
            }
            set
            {
                _controllerBehaviour = value;
            }
        }

        public event Action InitializationComplete;
        public event Action FreeComplete;

        public virtual void Free()
        {
            if (ControllerBehaviour == null)
                return;
            Destroy(ControllerBehaviour.gameObject);
            FreeComplete?.Invoke();
        }

        public void Startup()
        {
            if (_controllerBehaviour != null)
            {
                Debug.Log("Service is already up");
            }
            _controllerBehaviour = InstantiateNewBehaviour();

            ConfigureBehaviour();
            ControllerBehaviour.Initialize();

            InitializationComplete?.Invoke();
        }

        protected virtual void ConfigureBehaviour() { }

    }
}