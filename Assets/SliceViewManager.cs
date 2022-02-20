using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

namespace UPQP.SliceView
{

    public class SliceViewManager : MonoBehaviour
    {
        [SerializeField]
        Transform levelRoot;
        [SerializeField]
        CinemachineVirtualCamera virtualCamera;

        private LODGroup[] lods;

        private void Awake()
        {
            Build();
        }
        
        [Button]
        public void Build()
        {
            lods = levelRoot.GetComponentsInChildren<LODGroup>();
        }

        [Button]
        public void EnterInSliceView()
        {
            for (int i = 0; i < lods.Length; i++)
            {
                LODGroup lODGroup = lods[i];
                lODGroup.ForceLOD(lODGroup.lodCount - 1);
            }

            virtualCamera.gameObject.SetActive(true);
        }

        [Button]
        public void ExitInSliceView()
        {
            for (int i = 0; i < lods.Length; i++)
            {
                LODGroup lODGroup = lods[i];
                lODGroup.ForceLOD(lODGroup.lodCount - 1);
            }

            //virtualCamera.gameObject.SetActive(false);
        }

    }
}
