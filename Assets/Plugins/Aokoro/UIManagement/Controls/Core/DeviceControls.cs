using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Editor;

namespace Aokoro.UIManagement.Controls
{
    [CreateAssetMenu(menuName = "Aokoro/UI/Inputs/DeviceControls")]
    public class DeviceControls : ScriptableObject
    {
        public string device;
        public ControlData[] controls;

        public bool GetMatchingControl(string controlPath, out ControlData data)
        {
            data = default;
            for (int i = 0; i < controls.Length; i++)
            {
                ControlData controlData = controls[i];
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
