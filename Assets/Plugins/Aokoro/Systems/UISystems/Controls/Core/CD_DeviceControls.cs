using NaughtyAttributes;
using UnityEngine;

namespace Aokoro.UIManagement.ControlsDiplaySystem
{
    [CreateAssetMenu(menuName = "Aokoro/UI/Inputs/DeviceControls")]
    public class CD_DeviceControls : ScriptableObject
    {
        public string Device => device;

        [SerializeField]
        private string device;

        private CD_Input defaultControl;
        [SerializeField]
        private CD_Input[] SpecialControls;

        internal CD_Input GetMatchingControl(ref string controlPath)
        {
            for (int i = 0; i < SpecialControls.Length; i++)
            {
                CD_Input controlData = SpecialControls[i];
                if (!defaultControl.HasValue && controlData.isDefault)
                    defaultControl = controlData;

                for (int j = 0; j < controlData.matchPaths.Length; j++)
                {
                    if (controlData.matchPaths[j] == controlPath.Trim().ToLower())
                        return controlData;
                }
            }

            return defaultControl;
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            for (int i = 0; i < SpecialControls.Length; i++)
            {
                CD_Input controlData = SpecialControls[i];
                for (int j = 0; j < controlData.matchPaths.Length; j++)
                    controlData.matchPaths[j] = controlData.matchPaths[j].Trim().ToLower();
            }
        }
#endif
    }
}
