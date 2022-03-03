using UnityEngine;
using System;
using System.IO;
using UnityEngine.InputSystem;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Aokoro.UIManagement.ControlsDiplaySystem
{
    public static class ControlsDiplaySystem
    {
        internal static string CurrentDevice;
        internal static event Action<string> OnDeviceChanges;

        private static CD_Data _data;
        internal static CD_Data Data
        {
            get
            {
                if (_data == null)
                {
                    _data = Resources.Load<CD_Data>("DeviceControlsList");
#if UNITY_EDITOR
                    if (_data == null)
                    {
                        _data = ScriptableObject.CreateInstance<CD_Data>();
                        AssetDatabase.CreateAsset(_data, Path.Combine("Resources", "DeviceControllist"));
                    }
#endif
                }

                return _data;
            }
        }

        public static void TriggerControlChanges(string newDevice)
        {
            OnDeviceChanges?.Invoke(newDevice);
            CurrentDevice = newDevice;
        }

        public static CD_DeviceControls GetControlsForDevice(string device)
        {
            CD_DeviceControls[] controls = Data.controls;

            for (int i = 0; i < controls.Length; i++)
            {
                if (InputSystem.IsFirstLayoutBasedOnSecond(controls[i].Device, device))
                    return controls[i];
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
                    CD_Input[] inputs = new CD_Input[bindings.Length];

                    if (ConvertBindings(controls, bindings, inputs))
                        command.Addcombination(inputs);
                }

                commands[i] = command;
            }

            return commands;
        }

        private static bool ConvertBindings(CD_DeviceControls controls, string[] bindings, CD_Input[] inputs)
        {

            for (int k = 0; k < bindings.Length; k++)
            {
                //Get all kind of info from action that will be used for matching the correct sprite with the correct binding
                string bindingString = bindings[k];
                CD_Input data = controls.GetMatchingControl(ref bindingString);
                if (data.HasValue)
                {
                    bindings[k] = bindingString;
                    inputs[k] = data;
                }
                else
                    return false;
            }

            return true;
        }
    }
}
