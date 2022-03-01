using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Editor;

namespace Aokoro.UIManagement.ControlDisplay
{
    [CreateAssetMenu(menuName = "Aokoro/UI/Inputs/DeviceControlsList")]
    public class CD_DeviceControlsList : ScriptableObject
    {
        public GameObject or;
        public GameObject and;
        public CD_DeviceControls[] controls;
    }
}
