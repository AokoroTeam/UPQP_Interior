using NaughtyAttributes;
using UnityEngine;

namespace Aokoro.UI.ControlsDiplaySystem
{
    [CreateAssetMenu(menuName = "Aokoro/UI/Inputs/DeviceControls")]
    public class CD_DeviceControls : ScriptableObject
    {

        public string[] Devices => devices;
        public string ControlScheme => controlScheme;

        [SerializeField]
        private string[] devices;
        [SerializeField]
        private string controlScheme;

        private CD_Input defaultControl;
        [SerializeField]
        private CD_Input[] SpecialControls;

        internal bool TryGetMatchedInput(string path, out CD_MatchedInput matchedInput)
        {
            CD_Input data = FindInputData(path);
            matchedInput = data.HasValue ? new CD_MatchedInput(data, path) : default;

            return data.HasValue;
        }
        internal CD_Input FindInputData(string controlPath)
        {
            for (int i = 0; i < SpecialControls.Length; i++)
            {
                CD_Input controlData = SpecialControls[i];
                if (!defaultControl.HasValue && controlData.isDefault)
                    defaultControl = controlData;

                if (controlData.MatchesPath(controlPath))
                    return controlData;

            }
            //Debug.LogError($"{controlPath} was not found in {device}", this);
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
