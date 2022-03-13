using UnityEngine;
using System;
using System.IO;
using UnityEngine.InputSystem;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Aokoro.UI.ControlsDiplaySystem
{
    public static class ControlsDiplaySystem
    {
        
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

        public static CD_Command[] ExtractCommands(CD_InputAction[] actions, CD_DeviceControls controls)
        {
            int length = actions.Length;
            CD_Command[] commands = new CD_Command[actions.Length];


            for (int i = 0; i < length; i++)
            {
                CD_InputAction cd_action = actions[i];
                InputAction action = cd_action.action;

                CD_Command command = new CD_Command(cd_action.DisplayName);
                for (int j = 0; j < action.bindings.Count; j++)
                {
                    InputBinding binding = action.bindings[j];

                    if (binding.isPartOfComposite)
                        continue;

                    string[] bindings = action.GetBindingDisplayString(j, InputBinding.DisplayStringOptions.DontIncludeInteractions).Split('+', StringSplitOptions.RemoveEmptyEntries);
                    MatchedInput[] inputs = new MatchedInput[bindings.Length];

                    if (ConvertBindings(controls, bindings, inputs))
                        command.Addcombination(inputs);
                }

                commands[i] = command;
            }

            return commands;
        }

        private static bool ConvertBindings(CD_DeviceControls controls, string[] bindings, MatchedInput[] inputs)
        {

            for (int k = 0; k < bindings.Length; k++)
            {
                //Get all kind of info from action that will be used for matching the correct sprite with the correct binding
                string bindingString = bindings[k];
                CD_Input data = controls.GetMatchingInput(ref bindingString);
                if (data.HasValue)
                    inputs[k] = new MatchedInput(data, bindingString);

                else
                    return false;
            }

            return true;
        }

        public static CD_InputAction[] ConvertInputSystemActions(InputAction[] actions, CD_InputActionConvertionSettings settings)
        {
            if (!settings.HasValue)
                return ConvertInputSystemActions(actions);
            else
                return settings.ConvertInputSystemActions(actions);

        }
        public static CD_InputAction[] ConvertInputSystemActions(InputAction[] actions)
        {
            CD_InputAction[] cD_InputActions = new CD_InputAction[actions.Length];
            for (int i = 0; i < actions.Length; i++)
            {
                InputAction action = actions[i];
                cD_InputActions[i] = new CD_InputAction(action.name, action);
            }

            return cD_InputActions;
        }
    }
}
