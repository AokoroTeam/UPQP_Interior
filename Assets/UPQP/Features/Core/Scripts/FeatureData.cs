using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UPQP.Managers;

namespace UPQP.Features
{
    public abstract class FeatureData<T> : FeatureDataAsset where T : Feature
    {
        internal override Feature GenerateFeature(LevelManager controller) => GenerateFeatureFromData(controller);
        public abstract T GenerateFeatureFromData(LevelManager controller);
    }

    public abstract class FeatureDataAsset : ScriptableObject
    {
        internal abstract Feature GenerateFeature(LevelManager controller);
    }
}
