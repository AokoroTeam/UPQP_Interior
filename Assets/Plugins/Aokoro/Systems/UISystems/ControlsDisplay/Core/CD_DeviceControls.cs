using NaughtyAttributes;
using UnityEngine;

namespace Aokoro.UI.ControlsDiplaySystem
{
    [CreateAssetMenu(menuName = "Aokoro/UI/Inputs/DeviceControls")]
    public class CD_DeviceControls : ScriptableObject
    {

        public string ControlScheme => controlScheme;

        [SerializeField]
        private string controlScheme;

        private CD_InputDisplay defaultControl;
        [SerializeField]
        private CD_InputDisplay[] SpecialControls;

        internal CD_InputRepresentation GetInputRepresentationFromControl(CD_InputControl control)
        {
            CD_InputDisplay display = FindDisplayForControl(control);
            return display.HasValue ? new CD_InputRepresentation(display, control) : null;
        }

        internal int GetInputRepresentationsFromControls(CD_InputControl[] controls, CD_InputRepresentation[] output)
        {
            int size = 0;
            for (int i = 0; i < controls.Length; i++)
            {
                CD_InputControl control = controls[i];
                CD_InputRepresentation representation = GetInputRepresentationFromControl(control);
                if (representation != null)
                {
                    output[size] = representation;
                    size++;
                }
            }

            return size;
        }

        private CD_InputDisplay FindDisplayForControl(CD_InputControl control)
        {
            for (int i = 0; i < SpecialControls.Length; i++)
            {
                CD_InputDisplay controlData = SpecialControls[i];

                if (!defaultControl.HasValue && controlData.isDefault)
                    defaultControl = controlData;

                if (controlData.MatchesControl(control))
                    return controlData;

            }

            return defaultControl;
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            for (int i = 0; i < SpecialControls.Length; i++)
                SpecialControls[i].Validate();
        }
#endif
    }
}
