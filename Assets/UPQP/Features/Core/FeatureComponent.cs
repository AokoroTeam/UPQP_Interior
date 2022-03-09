using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace UPQP.Features
{
    public abstract class FeatureComponent<T> : MonoBehaviour where T : Feature
    {
        public T _Feature { get; internal set; }
        private void Awake()
        {
            Feature.InitiateFeatureComponents += Feature_InitiateFeatureComponents;
        }

        private void Feature_InitiateFeatureComponents(Feature feature)
        {
            if(feature == _Feature)
            {
                Feature.InitiateFeatureComponents -= Feature_InitiateFeatureComponents;
                Initiate();
            }
        }

        protected abstract void Initiate();

        protected virtual void Start()
        {
            if (_Feature == null)
                Destroy(gameObject);
        }
    }
}