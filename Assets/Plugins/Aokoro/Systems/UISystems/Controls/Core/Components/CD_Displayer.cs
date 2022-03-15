using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using UnityEngine.InputSystem;

namespace Aokoro.UI.ControlsDiplaySystem
{
    public class CD_Displayer : MonoBehaviour
    {
        [SerializeField]
        private CD_InputActionConvertionSettings actionSettings;
        [SerializeField]
        GameObject CommandLayout;

        [SerializeField]
        Transform root;

        private List<CD_DisplayCommand> displays = new List<CD_DisplayCommand>();
        public CD_DeviceControls CurrentControl => ControlsDiplaySystem.GetControlsForDevice(actionProvider.GetCurrentDeviceName());

        private CD_InputActionsProvider actionProvider;


        private void OnEnable()
        {
            if(actionProvider != null)
                actionProvider.OnResfreshNeeded += Refresh;
        }

        private void OnDisable()
        {
            if (actionProvider != null)
                actionProvider.OnResfreshNeeded -= Refresh;
        }


        public void AssignActionProvider(CD_InputActionsProvider value, bool triggerRefresh = true)
        {
            if (actionProvider != value)
            {
                if (actionProvider != null)
                {
                    actionProvider.OnResfreshNeeded -= Refresh;
                    if (triggerRefresh)
                        Refresh();
                }

                actionProvider = value;
                actionProvider.OnResfreshNeeded += Refresh;
            }
        }

        public void Show()
        {
            root.gameObject.SetActive(true);
            Refresh();
        }

        public void Hide()
        {
            root.gameObject.SetActive(false);
        }

        public void Refresh()
        {
            foreach (var display in displays)
                Destroy(display.gameObject);
            displays.Clear();

            CD_InputAction[] actions = ControlsDiplaySystem.ConvertInputSystemActions(actionProvider.GetInputActions(), actionSettings);
            CD_Command[] commands = ControlsDiplaySystem.ExtractCommands(actions, CurrentControl);

            for (int i = 0; i < commands.Length; i++)
            {
                CD_DisplayCommand displayer = GameObject.Instantiate(CommandLayout, root).GetComponent<CD_DisplayCommand>();
                displays.Add(displayer);
                displayer.Fill(commands[i]);
            }

            //Canvas.ForceUpdateCanvases();
        }
    }
}
