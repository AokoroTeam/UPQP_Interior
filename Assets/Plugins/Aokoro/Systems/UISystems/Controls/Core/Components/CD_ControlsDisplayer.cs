using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using UnityEngine.InputSystem;

namespace Aokoro.UIManagement.ControlDisplay
{
    public class CD_ControlsDisplayer : MonoBehaviour
    {
        [SerializeField]
        private CD_ControlMapProvider mapProvider;
        [SerializeField]
        string mapName;
        [SerializeField]
        GameObject displayPrefab;
        [SerializeField]
        Transform root;
        private List<CD_CommandDisplayer> displays;

        private void OnEnable()
        {
            ControlsDiplayManager.OnDeviceChanges += OnDeviceChanges;
        }
        private void OnDisable()
        {
            ControlsDiplayManager.OnDeviceChanges -= OnDeviceChanges;
        }

        private void OnDeviceChanges(string device)
        {
            CD_DeviceControls controls = ControlsDiplayManager.GetControlsForDevice(device);
            List<CD_Command> commands = new List<CD_Command>();
            
            var actionMap = mapProvider.GetInputActionMap(mapName);
            var actions = actionMap.actions;

            actionMap.Enable();
        }
    }
}
