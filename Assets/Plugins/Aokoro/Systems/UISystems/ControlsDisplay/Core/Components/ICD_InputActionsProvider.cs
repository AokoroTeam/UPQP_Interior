using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using UnityEngine.InputSystem;

namespace Aokoro.UI.ControlsDiplaySystem
{
    public interface ICD_InputActionsProvider
    {
        public InputAction[] GetInputActions();
        public string GetCurrentDeviceName();
        public event Action OnActionsNeedRefresh;
    }
}
