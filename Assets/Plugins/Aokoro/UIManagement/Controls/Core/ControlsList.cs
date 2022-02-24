using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using UnityEngine.InputSystem;

namespace Aokoro.UIManagement.Controls
{
    public class ControlsList : MonoBehaviour
    {
        [SerializeField]
        private ControlMapProvider mapProvider;
        [SerializeField]
        string mapName;
        [SerializeField]
        GameObject displayPrefab;
        private List<ControlsDisplayer> displays;

        private void OnEnable()
        {
            ControlsManager.OnDeviceChanges += OnDeviceChanges;
        }
        private void OnDisable()
        {
            ControlsManager.OnDeviceChanges -= OnDeviceChanges;
        }

        private void OnDeviceChanges(string device)
        {
            var controls = ControlsManager.GetControlsForDevice(device);
            var actionMap = mapProvider.GetInputActionMap(mapName);

            var actions = actionMap.actions;

            foreach (var action in actions)
            {
                var isFirstControl = true;
                string controlsText = "";

                for (int i = 0; i < action.bindings.Count; i++)
                {
                    if (action.bindings[i].isPartOfComposite)
                        continue;

                    //Get all kind of info from action that will be used for matching the correct sprite with the correct binding
                    string bindingString = action.GetBindingDisplayString(i, out string deviceLayoutName, out string controlPath);
                    if (!isFirstControl)
                        controlsText += " or ";

                    isFirstControl = false;
                    controlsText += bindingString;
                }
               Debug.Log(controlsText);
            }

            actionMap.Enable();
        }
    }
}
