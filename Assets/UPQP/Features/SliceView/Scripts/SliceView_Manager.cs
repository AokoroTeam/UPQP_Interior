using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Aokoro;
using UPQP.Features;
using Michsky.UI.ModernUIPack;

namespace UPQP.Features.SliceView
{
    public class SliceView_Manager : FeatureComponent<SliceView>
    {
        [SerializeField]
        Transform levelRoot;
        public CinemachineVirtualCamera virtualCamera;

        private LODGroup[] lods;


        [Button]
        protected override void Initiate()
        {
            Build();
        }
        public void Build()
        {
            //lods = levelRoot.GetComponentsInChildren<LODGroup>();
        }

        public void OnFeatureStarts()
        {
            _Feature.UI.ShowCommands();
            for (int i = 0; i < lods.Length; i++)
            {
                LODGroup lODGroup = lods[i];
                lODGroup.ForceLOD(lODGroup.lodCount - 1);
            }

            virtualCamera.enabled = true;
        }

        [Button]
        public void OnFeatureEnds()
        {
            _Feature.UI.HideCommands();
            for (int i = 0; i < lods.Length; i++)
            {
                LODGroup lODGroup = lods[i];
                lODGroup.ForceLOD(-1);
            }

            virtualCamera.enabled = false;
        }

    }
}
