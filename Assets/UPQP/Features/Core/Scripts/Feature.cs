using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UPQP.Managers;

namespace UPQP.Features
{
    public abstract class Feature
    {
        public abstract string FeatureName { get; }
        public static event Action<Feature> InitiateFeatureComponents;

        internal void Setup(LevelManager controller)
        {
            GenerateNeededContent(controller);
            InitiateFeatureComponents?.Invoke(this);
        }

        protected abstract void GenerateNeededContent(LevelManager controller);
        public abstract void Clean(LevelManager controller);

        public abstract void EnableFeature();
        public abstract void DisableFeature();
    }
}