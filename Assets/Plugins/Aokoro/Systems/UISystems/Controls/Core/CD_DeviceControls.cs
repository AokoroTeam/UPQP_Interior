using UnityEngine;

namespace Aokoro.UIManagement.ControlDisplay
{
    [CreateAssetMenu(menuName = "Aokoro/UI/Inputs/DeviceControls")]
    public class CD_DeviceControls : ScriptableObject
    {
        public string device;
        public CD_ControlRepresentation[] controls;

        public bool GetMatchingControl(ref string controlPath, out CD_ControlRepresentation data)
        {
            data = default;
            for (int i = 0; i < controls.Length; i++)
            {
                CD_ControlRepresentation controlData = controls[i];
                for (int j = 0; j < controlData.matchPaths.Length; j++)
                {
                    if (controlData.matchPaths[j] == controlPath)
                    {
                        data = controlData;
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
