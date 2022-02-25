using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using UnityEngine.InputSystem;

namespace Aokoro.UIManagement.Controls
{
    public abstract class ControlMapProvider : MonoBehaviour
    {
        public abstract InputActionMap GetInputActionMap(string mapName);
    }
}
