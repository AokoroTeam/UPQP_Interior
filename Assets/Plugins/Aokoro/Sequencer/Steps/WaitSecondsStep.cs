using System;
using System.Collections;
using System.Collections.Generic;
using Aokoro.Tools;
using UnityEngine;

namespace Aokoro.Sequencing.Steps
{
    [Serializable]
    public class WaitSecondsStep : IStep
    {
        private float start;
        readonly float duration;

        public WaitSecondsStep(float duration)
        {
            this.duration = duration;
        }

        void IStep.OnBegin() => start = Time.timeSinceLevelLoad;

        bool IStep.Tick(ISequencer parent) => Time.timeSinceLevelLoad - start >= duration;

        void IStep.OnEnd() => start = 0;
    }
}
