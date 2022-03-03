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
        string mapName;
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

            var actions = actionProvider.GetActions(mapName);

            CD_Command[] commands = ControlsDiplaySystem.ExtractCommands(actions, controls);
            DisplayCommands(commands);
        }

        private void DisplayCommands(CD_Command[] commands)
        {
            for (int i = 0; i < commands.Length; i++)
            {
                CD_DisplayCommand displayer = GameObject.Instantiate(CommandLayout).GetComponent<CD_DisplayCommand>();
                CD_Command command = commands[i];
                displayer.Fill(command);
            }
        }
    }
}
