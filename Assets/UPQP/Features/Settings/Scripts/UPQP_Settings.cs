using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UPQP.Features;
using UPQP.Managers;

namespace UPQP.Features.Settings
{
    public class UPQP_Settings : Feature
    {
        public override string FeatureName => "Settings";

        public override void CleanContentOnDestroy(LevelManager controller)
        {
            
        }
        protected override void GenerateNeededContentOnSetup(LevelManager controller)
        {

        }


        public override void DisableFeature()
        {

        }

        public override void EnableFeature()
        {
        }


    }
}