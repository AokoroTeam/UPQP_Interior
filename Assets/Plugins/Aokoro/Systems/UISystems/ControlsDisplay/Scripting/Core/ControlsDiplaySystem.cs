using UnityEngine;
using System;
using System.IO;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using UnityEngine.InputSystem.Utilities;

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

        public static CD_DeviceControls GetControlsForControlScheme(string controlScheme)
        {
            CD_DeviceControls[] controls = Data.controls;

            for (int i = 0; i < controls.Length; i++)
            {
                if (controls[i].ControlScheme == controlScheme)
                    return controls[i];
            }
            Debug.Log($"Unkown scheme : {controlScheme}. Returning default");
            return controls[0];
        }


        public static CD_Command[] ExtractCommands(CD_InputAction[] actions, CD_DeviceControls controls, InputDevice[] devices)
        {
            int length = actions.Length;
            CD_Command[] commands = new CD_Command[actions.Length];

            for (int i = 0; i < length; i++)
            {
                //System representation of actions
                CD_InputAction cd_action = actions[i];
                //Action to work with
                InputAction action = cd_action.action;
                //Create the command that will represent the action and the controls associated to it
                CD_Command command = new CD_Command(cd_action.DisplayName);

                //Going through all bindings inside of the action
                var bindings = action.bindings;
                int bindingCount = bindings.Count;
                for (int j = 0; j < bindingCount; j++)
                {

                    InputBinding binding = bindings[j];
                    //Only the final path is intresting
                    string effectivePath = binding.effectivePath;

                    //If so, skip because it is already used later on
                    if (binding.isPartOfComposite)
                        continue;

                    //If the binding is itself a combination of multiple bindings
                    if (binding.isComposite)
                    {
                        CD_InputControl composite = ExtractCompositeControls(devices, bindings, j);

                        //Modifiers
                        if (composite.compositeType is "OneModifier" or "TwoModifier")
                        {
                            CD_InputRepresentation[] representations = new CD_InputRepresentation[composite.Lenght];
                            int representationLenght = controls.GetInputRepresentationsFromControls(composite.Split(), representations);
                            command.Addcombination(representationLenght, representations);
                        }
                        //Axis etc...
                        else
                        {
                            CD_InputRepresentation representation = controls.GetInputRepresentationFromControl(composite);
                            command.Addcombination(representation);
                        }
                    }

                    else if (TryGetControlPathsFromBinding(devices, effectivePath, out string controlPath, out string displayName))
                    {
                        CD_InputRepresentation representation = controls.GetInputRepresentationFromControl(new CD_InputControl(controlPath, displayName));
                        command.Addcombination(representation);
                    }
                }

                commands[i] = command;
            }

            return commands;
        }

        private static CD_InputControl ExtractCompositeControls(InputDevice[] devices, ReadOnlyArray<InputBinding> bindings, int index)
        {

            CD_InputControl composite = new CD_InputControl(bindings[index].GetNameOfComposite());

            //Composites parts are after the Composite
            while (true)
            {
                index++;
                if (index >= bindings.Count)
                    break;

                var compositeBinding = bindings[index];

                //Out of the composite
                if (!compositeBinding.isPartOfComposite)
                    break;

                if (TryGetControlPathsFromBinding(devices, compositeBinding.effectivePath, out string compositeControlPath, out string compositeDisplayName))
                    composite.AddControl(compositeControlPath, compositeDisplayName);
            }
            return composite;
        }

        private static bool TryGetControlPathsFromBinding(InputDevice[] devices, string bindingPath, out string controlPath, out string displayName)
        {
            displayName = string.Empty;
            controlPath = string.Empty;

            foreach (var device in devices)
            {
                var control = InputControlPath.TryFindControl(device, bindingPath);
                if (control != null)
                {
                    displayName = InputControlPath.ToHumanReadableString(control.path, 
                        out string deviceLayoutName, 
                        out controlPath, 
                        InputControlPath.HumanReadableStringOptions.OmitDevice,
                        device);

                    /*Debug.Log($"Display name : {displayName} | ControlPath : {controlPath} | Device Layout : {deviceLayoutName}");
                    Debug.Log($"{control.path} | {control.name} | {control.variants}  | {control.shortDisplayName} ");*/
                    return true;
                }
            }

            return false;
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
