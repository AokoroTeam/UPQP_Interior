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
    [AddComponentMenu("UPQP/Features/SliceView/Manager")]
    public class SliceView_Manager : FeatureComponent<SliceView>
    {
        public CinemachineVirtualCamera virtualCamera;

        private Transform levelRoot;
        private LODGroup[] lods;


        [Button]
        protected override void Initiate()
        {
            Build();
        }
        public void Build()
        {
            SliceView_LevelRoot sliceView_LevelRoot = GameObject.FindObjectOfType<SliceView_LevelRoot>();
            if (sliceView_LevelRoot == null)
            {
                Debug.LogError("Please add SliceView_LevelRoot component to the root of the level");
                lods = new LODGroup[0];
            }
            else
            {
                levelRoot = sliceView_LevelRoot.transform;
                lods = levelRoot.GetComponentsInChildren<LODGroup>();
            }
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
