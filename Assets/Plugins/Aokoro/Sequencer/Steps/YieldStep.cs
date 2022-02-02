using System;
using System.Collections;
using System.Collections.Generic;
using Aokoro.Tools;
using UnityEngine;

namespace Aokoro.Sequencing.Steps
{
    [Serializable]
    public class YieldStep : WaitForFramesStep
    {
        public YieldStep() : base(1)
        {

        }
    }
}
