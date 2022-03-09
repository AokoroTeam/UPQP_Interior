using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UPQP.Player;

namespace UPQP.Features
{

    public interface IPlayerFeature
    {
        public Feature @Feature { get; }
        public string FeatureName { get; }
        public UPQP_Player Player { get; set; }


        public void ExecuteFeature();
        public void EndFeature();
    }
}