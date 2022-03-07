using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Aokoro.UIManagement.ControlsDiplaySystem
{
    [System.Serializable]
    public struct CD_InputActionConvertionSettings
    {
        [SerializeField]
        private CD_InputActionSelector[] selectActions;

        internal bool HasValue => selectActions != null;

        public CD_InputAction[] ConvertInputSystemActions(InputAction[] actions)
        {
            List<CD_InputAction> filteredActions = new List<CD_InputAction>();

            for (int i = 0; i < actions.Length; i++)
            {
                InputAction inputAction = actions[i];

                for (int j = 0; j < selectActions.Length; j++)
                {
                    CD_InputActionSelector selector = selectActions[j];
                    if(selector.displayName == inputAction.name)
                        filteredActions.Add(new CD_InputAction(selector.outputName, inputAction));
                }
            }

            return filteredActions.ToArray();
        }

        [System.Serializable]
        private struct CD_InputActionSelector
        {
            public string displayName;
            public string outputName;
        }
    }
}