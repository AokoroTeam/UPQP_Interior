using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Editor;

namespace Aokoro.UIManagement.Controls
{
    [CreateAssetMenu(menuName = "Aokoro/UI/Inputs/DeviceControlsList")]
    public class DeviceControlsList : ScriptableObject
    {
        public DeviceControls[] controls;
    }
}
