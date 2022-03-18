using NaughtyAttributes;
using UnityEngine;
using Cinemachine;

namespace UPQP.Features.SliceView
{
    [AddComponentMenu("UPQP/Features/SliceView/Manager")]
    public class SliceView_Manager : FeatureComponent<SliceView>
    {
        public CinemachineVirtualCamera virtualCamera;
        [SerializeField] private Transform levelRoot;
        [SerializeField] private GameObject[] hide;


        private LODGroup[] lods;


        [Button]
        protected override void Initiate()
        {
            if (levelRoot == null)
            {
                Debug.LogError("Please add SliceView_LevelRoot component to the root of the level");
                lods = new LODGroup[0];
            }
            else
            {
                levelRoot = levelRoot.transform;
                lods = levelRoot.GetComponentsInChildren<LODGroup>();
            }
        }
        public void OnFeatureEnables()
        {
            _Feature.UI.ShowCommands();
            for (int i = 0; i < lods.Length; i++)
            {
                LODGroup lODGroup = lods[i];
                lODGroup.ForceLOD(lODGroup.lodCount - 1);
            }

            for (int i = 0; i < hide.Length; i++)
                hide[i]?.SetActive(false);

            virtualCamera.enabled = true;

        }

        public void OnFeatureDisables()
        {
            _Feature.UI.HideCommands();
            for (int i = 0; i < lods.Length; i++)
            {
                LODGroup lODGroup = lods[i];
                lODGroup.ForceLOD(-1);
            }

            for (int i = 0; i < hide.Length; i++)
                hide[i]?.SetActive(true);

            virtualCamera.enabled = false;
        }

    }
}
