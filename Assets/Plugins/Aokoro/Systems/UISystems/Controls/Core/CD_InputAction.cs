using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Aokoro.UI.ControlsDiplaySystem
{
    public readonly struct CD_InputAction
    {
        public readonly string DisplayName;
        public readonly InputAction action;

        public CD_InputAction(string displayName, InputAction action)
        {
            DisplayName = displayName;
            this.action = action;
        }

        public static implicit operator InputAction(CD_InputAction _InputActions) => _InputActions.action;
    }
}
