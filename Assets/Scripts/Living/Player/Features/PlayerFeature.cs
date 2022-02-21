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
        [SerializeField] private InputActionMap actionMap;

        public InputActionMap GetActionMap()
        {
            InputActionMap map = new InputActionMap(featureName);
            for (int i = 0; i < actionMap.actions.Count; i++)
            {
                InputAction inputAction = actionMap.actions[i];
                map.AddAction(inputAction.name, inputAction.type, inputAction.GetBindingDisplayString(), inputAction.interactions, inputAction.processors);
            }

            return map;
        }

        public abstract void EnterFeature(PlayerManager player);
        public abstract void ExitFeature(PlayerManager player);
    }
}