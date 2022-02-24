using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using UnityEngine.InputSystem;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Aokoro.UIManagement.Controls
{
    public static class ControlsManager
    {
        public static string CurrentDevice;
        public static event Action<string> OnDeviceChanges;

        private static DeviceControlsList _controlsList;
        private static DeviceControlsList ControlsList
        {
            get
            {
                if (_controlsList == null)
                {
                    _controlsList = Resources.Load<DeviceControlsList>("Aokoro/Controls/DeviceControlsList");
#if UNITY_EDITOR
                    if (_controlsList == null)
                    {
                        _controlsList = ScriptableObject.CreateInstance<DeviceControlsList>();
                        AssetDatabase.CreateAsset(_controlsList, Path.Combine("Resources", "Aokoro", "Controls", "DeviceControllist"));
                    }
#endif
                }

                return _controlsList;
            }
        }

        public static void TriggerControlChanges(string newDevice)
        {
            OnDeviceChanges?.Invoke(newDevice);
            CurrentDevice = newDevice;
        }

        public static DeviceControls GetControlsForDevice(string device)
        {
            DeviceControls[] controls = ControlsList.controls;

            for (int i = 0; i < controls.Length; i++)
            {
                if (controls[i].device == device)
                    return controls[i];
            }
            Debug.Log($"Unkown device {device}, returning default");
            return controls[0];
        }
    }
}
