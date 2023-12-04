using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace GRP.Unity
{
    public abstract class MonoSingle<T> : MonoBehaviour
    where T : MonoBehaviour
    {
        public static MonoSingle<T> Instance { get; private set; }

        void Awake()
        {
            if (Instance is not null && Instance != this)
            {
                Destroy(this.gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);
            OnAwake();
        }

        protected virtual void OnAwake() { }
    }
}