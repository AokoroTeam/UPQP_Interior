using UnityEngine;

namespace Aokoro.UI.ControlsDiplaySystem
{
    [CreateAssetMenu(menuName = "Aokoro/UI/Inputs/DeviceControlsList")]
    internal class CD_Data : ScriptableObject
    {
        [SerializeField]
        internal GameObject or;
        [SerializeField]
        internal GameObject and;
        [SerializeField]
        internal CD_DeviceControls[] controls;
    }
}
