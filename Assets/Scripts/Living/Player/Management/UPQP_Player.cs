using System.Collections;
using System.Collections.Generic;
using Aokoro.Entities.Player;
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

        private Dictionary<PlayerFeature, InputAction> startActions;
        [ReadOnly, Space]
        public InputActionMap executeFeatures;

        protected override void Awake()
        {
            base.Awake();
            MergeInputsfromFeatures();
        }



        private void MergeInputsfromFeatures()
        {
            startActions = new Dictionary<PlayerFeature, InputAction>();
            features = GetComponentsInChildren<PlayerFeature>();

            int length = Mathf.Min(features.Length, 10);

            executeFeatures = new InputActionMap("executeFeatures");
            executeFeatures.Disable();

            for (int i = 0; i < length; i++)
            {
                PlayerFeature feature = features[i];
                feature.player = this;

                //Creates an action to start executing the feature
                InputAction startAction = executeFeatures.AddAction(
                    $"Start {feature.featureName}",
                    InputActionType.Button,
                    $"<Keyboard>/{(i == 9 ? 0 : i + 1)}");

                //Links it to the correct callback
                startAction.performed += ctx => ExecuteFeatureCallback(ctx, feature);
                startActions.Add(feature, startAction);
            }

            executeFeatures.Enable();
        }

        private void ExecuteFeatureCallback(InputAction.CallbackContext context, PlayerFeature feature)
        {
            if (context.performed)
                ExecuteFeature(feature);
        }
        public void ExecuteFeature(PlayerFeature feature)
        {
            feature.StartFeature();
            ChangeActionMap(feature.featureName);
            executeFeatures.Disable();
        }

        public void EndFeature(PlayerFeature feature)
        {
            feature.EndFeature();

            ChangeActionMap(DefaultActionMap);
            executeFeatures.Enable();
        }
    }
}