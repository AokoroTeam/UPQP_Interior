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
            mapIds = new Dictionary<PlayerFeature, InputActionMap>();
            startActions = new Dictionary<PlayerFeature, InputAction>();
            features = GetComponentsInChildren<PlayerFeature>();

            int length = Mathf.Min(features.Length, 10);

            InputActionAsset actions = playerInput.actions;
            InputActionMap inputActionMap = actions.FindActionMap(playerInput.defaultActionMap);
            inputActionMap.Disable();

            for (int i = 0; i < length; i++)
            {
                PlayerFeature feature = features[i];

                //Creates an action to start executing the feature
                InputAction startAction = inputActionMap.AddAction(
                    $"Start {feature.name}",
                    InputActionType.Button,
                    $"<Keyboard>/{(i == 9 ? 0 : i + 1)}");

                //Links it to the correct callback
                startAction.Enable();
                startAction.performed += ctx => ExecuteFeatureCallback(ctx, feature);

                InputActionMap map = feature.GetActionMap();
                playerInput.actions.AddActionMap(map);

                startActions.Add(feature, startAction);
                mapIds.Add(feature, map);
            }

            inputActionMap.Enable();
        }

        private void ExecuteFeatureCallback(InputAction.CallbackContext context, PlayerFeature feature)
        {
            if (context.performed)
                ExecuteFeature(feature);
        }
        public void ExecuteFeature(PlayerFeature feature)
        {
            foreach (var actionsPair in startActions)
            {
                if (feature == actionsPair.Key)
                    feature.EnterFeature(this);

                actionsPair.Value.Disable();
            }
        }

        private void EndFeature(PlayerFeature feature)
        {
            foreach (var actionsPair in startActions)
                actionsPair.Value.Enable();
        }
    }
}