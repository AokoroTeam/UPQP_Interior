using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UPQP.Managers;

namespace UPQP.Features
{
    public abstract class Feature
    {
        public bool IsActive;
        public abstract string FeatureName { get; }
        public static event Action<Feature> InitiateFeatureComponents;

        internal void Setup(LevelManager controller)
        {
            GenerateNeededContentOnSetup(controller);
            InitiateFeatureComponents?.Invoke(this);
        }

        protected abstract void GenerateNeededContentOnSetup(LevelManager controller);
        public abstract void CleanContentOnDestroy(LevelManager controller);

        public abstract void EnableFeature();
        public abstract void DisableFeature();
    }
}