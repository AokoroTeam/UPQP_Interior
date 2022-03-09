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
        [BoxGroup("Features")]
        public Transform FeaturesRoot;
        [BoxGroup("Features"), ReadOnly, Space]
        public InputActionMap executeFeatures;


        private IPlayerFeature[] features;
        private Dictionary<Feature, InputAction> startActions;

        protected override void Awake()
        {
            base.Awake();
        }

        public void WhenFeaturesAreInitialized()
        {
            MergeInputsfromFeatures();
        }

        private void MergeInputsfromFeatures()
        {
            startActions = new Dictionary<Feature, InputAction>();
            features = GetComponentsInChildren<IPlayerFeature>();

            int length = Mathf.Min(features.Length, 10);

            executeFeatures = new InputActionMap("executeFeatures");
            executeFeatures.Disable();

            for (int i = 0; i < length; i++)
            {
                Feature feature = features[i].Feature;
                IPlayerFeature playerFeature = features[i];

                //Binds to player
                playerFeature.Player = this;

                //Creates an action to start executing the feature
                InputAction startAction = executeFeatures.AddAction(
                    $"Start {playerFeature.FeatureName}",
                    InputActionType.Button,
                    $"<Keyboard>/{(i == 9 ? 0 : i + 1)}");

                //Links it to the correct callback
                startAction.performed += ctx => ExecuteFeatureCallback(ctx, playerFeature);
                startActions.Add(feature, startAction);
            }

            executeFeatures.Enable();
        }

        private void ExecuteFeatureCallback(InputAction.CallbackContext context, IPlayerFeature feature)
        {
            if (context.performed)
                ExecuteFeature(feature);
        }
        public void ExecuteFeature(IPlayerFeature playerFeature)
        {
            playerFeature.ExecuteFeature();
            ChangeActionMap(playerFeature.FeatureName);
            executeFeatures.Disable();
        }

        public void EndFeature(IPlayerFeature playerFeature)
        {
            playerFeature.EndFeature();

            ChangeActionMap(DefaultActionMap);
            executeFeatures.Enable();
        }
    }
}