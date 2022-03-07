using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using UnityEngine.InputSystem;

namespace Aokoro.UIManagement.ControlsDiplaySystem
{
    public class CD_DisplayerManager : MonoBehaviour
    {
        [SerializeField]
        private CD_InputActionsProvider actionProvider;
        [SerializeField]
        private CD_InputActionConvertionSettings actionSettings;
        [SerializeField]
        GameObject CommandLayout;

        [SerializeField]
        Transform root;
        private List<CD_DisplayCommand> displays;

        private void OnEnable()
        {
            ControlsDiplaySystem.OnDeviceChanges += OnDeviceChanges;
        }
        private void OnDisable()
        {
            ControlsDiplaySystem.OnDeviceChanges -= OnDeviceChanges;
        }

        private void OnDeviceChanges(string device)
        {
            CD_DeviceControls controls = ControlsDiplaySystem.GetControlsForDevice(device);
            CD_InputAction[] actions = ControlsDiplaySystem.ConvertInputSystemActions(actionProvider.GetInputActions(), actionSettings);
            CD_Command[] commands = ControlsDiplaySystem.ExtractCommands(actions, controls);

            DisplayCommands(commands);
        }

        private void DisplayCommands(CD_Command[] commands)
        {
            for (int i = 0; i < commands.Length; i++)
            {
                CD_DisplayCommand displayer = GameObject.Instantiate(CommandLayout, root).GetComponent<CD_DisplayCommand>();
                CD_Command command = commands[i];
                displayer.Fill(command);
            }
        }
    }
}
