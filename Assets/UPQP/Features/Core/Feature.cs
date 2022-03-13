using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UPQP.Managers;

namespace UPQP.Features
{
    public abstract class Feature
    {
        public static event Action<Feature> InitiateFeatureComponents;

        internal void Setup(LevelManager controller)
        {
            Generate(controller);
            InitiateFeatureComponents?.Invoke(this);
        }

        protected abstract void Generate(LevelManager controller);
        public abstract void Clean(LevelManager controller);
    }
}