using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UPQP.Features;

namespace UPQP.Features
{

    public abstract class PlayerFeature : MonoBehaviour
    {
        [SerializeField] string featureName;
        [Space]
        [SerializeField] private InputAction[] actions;

        public InputActionMap GetActionMap()
        {
            InputActionMap map = new InputActionMap(featureName);
            for (int i = 0; i < actions.Length; i++)
            {
                InputAction inputAction = actions[i];
                map.AddAction(inputAction.name, inputAction.type, inputAction.GetBindingDisplayString(), inputAction.interactions, inputAction.processors);
            }

            return map;
        }

        public abstract void EnterFeature(PlayerManager player);
        public abstract void ExitFeature(PlayerManager player);
    }
}