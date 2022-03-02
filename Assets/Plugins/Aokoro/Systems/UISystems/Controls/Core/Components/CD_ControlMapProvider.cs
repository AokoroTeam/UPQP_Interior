using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using UnityEngine.InputSystem;

namespace Aokoro.UIManagement.ControlDisplay
{
    public abstract class CD_ControlMapProvider : MonoBehaviour
    {
        public abstract InputActionMap GetInputActionMap(string mapName);
    }
}
