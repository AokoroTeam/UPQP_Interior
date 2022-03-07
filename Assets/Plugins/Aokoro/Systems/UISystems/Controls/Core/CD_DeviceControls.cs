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

        internal CD_Input GetMatchingInput(ref string controlPath)
        {
            for (int i = 0; i < SpecialControls.Length; i++)
            {
                CD_Input controlData = SpecialControls[i];
                if (!defaultControl.HasValue && controlData.isDefault)
                    defaultControl = controlData;

                if (controlData.MatchesPath(controlPath))
                    return controlData;

            }

            return defaultControl;
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            for (int i = 0; i < SpecialControls.Length; i++)
                SpecialControls[i].Validate();
        }
#endif
    }
}
