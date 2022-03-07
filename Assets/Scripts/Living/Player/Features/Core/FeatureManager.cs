using Aokoro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UPQP.Features
{

    public abstract class FeatureManager<T, U> : Singleton<T>
        where T : MonoBehaviour
        where U : PlayerFeature
    {

        public abstract void OnFeatureStarts();

        public abstract void OnFeatureEnds();
    }
}