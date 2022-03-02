using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using System.IO;
using UnityEngine.InputSystem;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Aokoro.UIManagement.ControlDisplay
{
    public static class ControlsDiplayManager
    {
        public static string CurrentDevice;
        public static event Action<string> OnDeviceChanges;

        private static CD_DeviceControlsList _controlsList;
        private static CD_DeviceControlsList ControlsList
        {
            get
            {
                if (_controlsList == null)
                {
                    _controlsList = Resources.Load<CD_DeviceControlsList>("DeviceControlsList");
#if UNITY_EDITOR
                    if (_controlsList == null)
                    {
                        _controlsList = ScriptableObject.CreateInstance<CD_DeviceControlsList>();
                        AssetDatabase.CreateAsset(_controlsList, Path.Combine("Resources", "DeviceControllist"));
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

        public static CD_DeviceControls GetControlsForDevice(string device)
        {
            CD_DeviceControls[] controls = ControlsList.controls;

            for (int i = 0; i < controls.Length; i++)
            {
                if (InputSystem.IsFirstLayoutBasedOnSecond(controls[i].device, device))
                {
                    return controls[i];
                }
            }
            Debug.Log($"Unkown device {device}, returning default");
            return controls[0];
        }

        public static CD_Command[] ExtractCommands(InputAction[] actions, CD_DeviceControls controls)
        {
            int length = actions.Length;
            CD_Command[] commands = new CD_Command[actions.Length];


            for (int i = 0; i < length; i++)
            {
                InputAction action = actions[i];
                CD_Command command = new CD_Command(action.name);

                for (int j = 0; j < action.bindings.Count; j++)
                {
                    InputBinding binding = action.bindings[j];

                    if (binding.isPartOfComposite)
                        continue;

                    string[] bindings = action.GetBindingDisplayString(j).Split('+', StringSplitOptions.RemoveEmptyEntries);
                    GameObject[] representations = new GameObject[bindings.Length];

                    if (ConvertBindings(controls, bindings, representations))
                        command.Add(bindings, representations);
                }

                commands[i] = command;
            }

            return commands;
        }

        private static bool ConvertBindings(CD_DeviceControls controls, string[] bindings, GameObject[] prefabs)
        {
            
            for (int k = 0; k < bindings.Length; k++)
            {
                //Get all kind of info from action that will be used for matching the correct sprite with the correct binding
                string bindingString = bindings[k];
                if (controls.GetMatchingControl(ref bindingString, out CD_ControlRepresentation data))
                {
                    bindings[k] = bindingString;
                    prefabs[k] = data.representation;
                }
                else
                    return false;
            }

            return true;
        }
    }
}
