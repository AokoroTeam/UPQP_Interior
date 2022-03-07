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
    public class SliceView_Manager : FeatureManager<SliceView_Manager, SliceView_Player>
    {
        [SerializeField]
        Transform levelRoot;
        public CinemachineVirtualCamera virtualCamera;
        [SerializeField]
        WindowManager windowManager;

        private LODGroup[] lods;

        protected override void Awake()
        {
            Build();
            base.Awake();
        }


        [Button]
        public void Build()
        {
            lods = levelRoot.GetComponentsInChildren<LODGroup>();
        }
        [Button]
        public override void OnFeatureStarts()
        {
            windowManager.OpenWindow("SliceView");
            for (int i = 0; i < lods.Length; i++)
            {
                LODGroup lODGroup = lods[i];
                lODGroup.ForceLOD(lODGroup.lodCount - 1);
            }

            virtualCamera.enabled = true;
        }

        [Button]
        public override void OnFeatureEnds()
        {
            windowManager.OpenWindow("Default");
            for (int i = 0; i < lods.Length; i++)
            {
                LODGroup lODGroup = lods[i];
                lODGroup.ForceLOD(-1);
            }

            virtualCamera.enabled = false;
        }

        protected override void OnExistingInstanceFound(SliceView_Manager existingInstance)
        {
            Destroy(gameObject);
        }
    }
}
