using UnityEngine;
using System;
using System.IO;
using UnityEngine.InputSystem;
using System.Collections.Generic;

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


                UnityEngine.InputSystem.Utilities.ReadOnlyArray<InputBinding> bindings = action.bindings;
                int bindingCount = bindings.Count;

                for (int j = 0; j < bindingCount; j++)
                {
                    InputBinding binding = bindings[j];

                    if (binding.isPartOfComposite)
                        continue;

                    string displayString = action.GetBindingDisplayString(j, out string device, out string controlPath, InputBinding.DisplayStringOptions.DontIncludeInteractions);

                    if (binding.isComposite)
                    {
                        string compositeType = binding.GetNameOfComposite();
                        List<string> compositePaths = new List<string>();
                        int compositeIndex = 1;
                        bool insideComposite = true;

                        while (insideComposite)
                        {
                            int Index() => j + compositeIndex;

                            string compositePath = GetDisplayString(action, Index());
                            compositePaths.Add(compositePath);

                            compositeIndex++;
                            int newIndex = Index();
                            insideComposite = newIndex < bindingCount && bindings[newIndex].isPartOfComposite;
                        }

                        //Modifiers
                        if (compositeType is "OneModifier" or "TwoModifier")
                        {
                            MatchedInput[] inputs = new MatchedInput[compositePaths.Count];
                            if (ConvertBindingsToMatchedInputs(controls, compositePaths, inputs))
                                command.Addcombination(inputs);
                        }
                        //Axis
                        else
                        {
                            string pathData = string.Join('/', compositePaths);
                            AddBindingToCommand(controls, ref command, compositeType, pathData);
                        }
                    }
                    else
                        AddBindingToCommand(controls, ref command, controlPath, controlPath);
                }

                commands[i] = command;
            }

            return commands;

            static void AddBindingToCommand(CD_DeviceControls controls, ref CD_Command command, string matchingString, string pathData)
            {
                CD_Input data = controls.GetMatchingInput(matchingString);
                if (data.HasValue)
                    command.Addcombination(new MatchedInput(data, pathData));
                else
                    Debug.LogError($"Cannot find binding {matchingString}");
            }
        }

        private static string GetDisplayString(InputAction action, int index)
        {
            string displayString = action.GetBindingDisplayString(
                index,
                out string device,
                out string controlPath,
                InputBinding.DisplayStringOptions.DontIncludeInteractions
            );
            return string.IsNullOrWhiteSpace(controlPath) ? displayString : controlPath;
        }
        private static bool ConvertBindingsToMatchedInputs(CD_DeviceControls controls, IEnumerable<string> bindings, MatchedInput[] inputs)
        {
            int i = 0;
            foreach (string bindingString in bindings)
            {
                //Get all kind of info from action that will be used for matching the correct sprite with the correct binding
                CD_Input data = controls.GetMatchingInput(bindingString);
                if (data.HasValue)
                    inputs[i] = new MatchedInput(data, bindingString);
                else
                    return false;

                i++;
            }

            return true;
        }

        public static CD_InputAction[] ConvertInputSystemActions(InputAction[] actions, CD_ActionsFilters settings)
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
