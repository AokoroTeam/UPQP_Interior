using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Aokoro.UI.ControlsDiplaySystem
{
    [System.Serializable]
    public struct CD_ActionsFilters
    {
        [SerializeField]
        private CD_ActionFilterData[] selectActions;

        internal bool HasValue => selectActions != null;

        public CD_ActionsFilters(params CD_ActionFilterData[] selectors)
        {
            selectActions = selectors;
        }
        public CD_InputAction[] ConvertInputSystemActions(IEnumerable<InputAction> actions)
        {
            List<CD_InputAction> filteredActions = new List<CD_InputAction>();
            List<InputAction> actionsList = new List<InputAction>(actions);

            for (int i = 0; i < selectActions.Length; i++)
            {
                var filter = selectActions[i];
                int indexOf = actionsList.FindIndex(ctx => ctx.name == filter.displayName);
                if (indexOf != -1)
                {
                    filteredActions.Add(new CD_InputAction(filter.outputName, actionsList[indexOf]));
                    actionsList.RemoveAt(indexOf);
                }
            }

            return filteredActions.ToArray();
        }

        [System.Serializable]
        public struct CD_ActionFilterData
        {
            public string displayName;
            public string outputName;

            public CD_ActionFilterData(string displayName, string outputName)
            {
                this.displayName = displayName;
                this.outputName = outputName;
            }
        }
    }
}