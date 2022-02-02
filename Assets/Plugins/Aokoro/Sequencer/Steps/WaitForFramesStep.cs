using System;
using System.Collections;
using System.Collections.Generic;
using Aokoro.Tools;
using UnityEngine;

namespace Aokoro.Sequencing.Steps
{
    [Serializable]
    public class WaitForFramesStep : IStep
    {
        private int frameCount;
        private int targetFrameCount;
        
        public WaitForFramesStep(int targetFrameCount)
        {
            this.targetFrameCount = targetFrameCount;
        }

        void IStep.OnBegin() => frameCount = 0;

        bool IStep.Tick(ISequencer parent)
        {
            frameCount++;
            return frameCount > targetFrameCount;
        }

        void IStep.OnEnd() { }
    }
}
