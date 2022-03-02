using System;

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
