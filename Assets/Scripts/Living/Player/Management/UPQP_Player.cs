using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.InputSystem;
using UPQP.Features;

namespace UPQP.Player
{
    public class UPQP_Player : PlayerManager
    {
        private PlayerFeature[] features;

        private PlayerFeature currentFeature;


        private Dictionary<PlayerFeature, InputActionMap> mapIds;
        private Dictionary<PlayerFeature, InputAction> startActions;

        protected override void Awake()
        {
            base.Awake();

            features = GetComponentsInChildren<PlayerFeature>();

            int length = Mathf.Min(features.Length, 10);
            for (int i = 0; i < length; i++)
            {
                PlayerFeature feature = features[i];
                InputActionAsset actions = playerInput.actions;

                InputAction startAction = actions.FindActionMap(playerInput.defaultActionMap).AddAction(
                    $"Start {feature.name}",
                    InputActionType.Button,
                    $"<keyboard>{(i == 9 ? 0 : i + 1)}");

                startAction.Enable();
                startAction.performed += ExecuteFeature;

                InputActionMap map = feature.GetActionMap();
                playerInput.actions.AddActionMap(map);
                mapIds.Add(feature, map);

            }

        }

        private void ExecuteFeature(InputAction.CallbackContext context)
        {
            if (currentFeature != null)
                return;
        }

        private void EndFeature(InputAction.CallbackContext context)
        {

        }
    }
}