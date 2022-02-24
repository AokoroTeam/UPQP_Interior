using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

namespace UPQP.SliceView
{

    public class SliceViewManager : Singleton<SliceViewManager>
    {
        [SerializeField]
        Transform levelRoot;
        [SerializeField]
        CinemachineVirtualCamera virtualCamera;

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
        public void EnterSliceView()
        {
            for (int i = 0; i < lods.Length; i++)
            {
                LODGroup lODGroup = lods[i];
                lODGroup.ForceLOD(lODGroup.lodCount - 1);
            }

            virtualCamera.enabled = true;
        }

        [Button]
        public void ExitSliceView()
        {
            for (int i = 0; i < lods.Length; i++)
            {
                LODGroup lODGroup = lods[i];
                lODGroup.ForceLOD(-1);
            }

            virtualCamera.enabled = false;
        }

        protected override void OnExistingInstanceFound(SliceViewManager existingInstance)
        {
            Destroy(gameObject);
        }
    }
}
