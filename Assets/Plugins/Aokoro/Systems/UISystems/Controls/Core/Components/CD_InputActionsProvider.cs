using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using UnityEngine.InputSystem;

namespace Aokoro.UIManagement.ControlsDiplaySystem
{
    public abstract class CD_InputActionsProvider : MonoBehaviour
    {
        public abstract InputAction[] GetActions(string mapName);
    }
}
