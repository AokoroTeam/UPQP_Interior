using UnityEngine;

namespace Aokoro.UIManagement.ControlsDiplaySystem
{
    [CreateAssetMenu(menuName = "Aokoro/UI/Inputs/DeviceControlsList")]
    internal class CD_Data : ScriptableObject
    {
        internal GameObject or;
        internal GameObject and;
        internal CD_DeviceControls[] controls;
    }
}
