using System.Collections;
using System.Collections.Generic;
using Aokoro.Entities.Player;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.InputSystem;
using UPQP.Features;
using UPQP.Managers;

namespace UPQP.Player
{
    public class UPQP_Player : PlayerManager
    {
        [BoxGroup("UI")]
        [SerializeField]
        private string FirstWindowToShow;

        [BoxGroup("Features")]
        public Transform FeaturesRoot;
        [BoxGroup("Features"), ReadOnly, Space]
        public InputActionMap executeFeatures;

        protected override void Awake()
        {
            if(LevelManager.Instance.LevelInitiationPhase != LevelManager.playerPhase)
                Destroy(gameObject);
            else
                base.Awake();
        }
        public override void OnAwake()
        {
            SetupPlayerFeatures(LevelManager.Instance.CreateLevelFeatures());
            base.OnAwake();
        }

        private void SetupPlayerFeatures(Feature[] features)
        {
            var playerFeatures = GetComponentsInChildren<IPlayerFeature>();
            for (int i = 0; i < playerFeatures.Length; i++)
                playerFeatures[i].Player = this;


            int length = Mathf.Min(features.Length, 10);

            executeFeatures = new InputActionMap("executeFeatures");
            executeFeatures.Disable();

            for (int i = 0; i < length; i++)
            {
                IPlayerFeature playerFeature = playerFeatures[i];

                ///Binds to player
                playerFeature.Player = this;

                ///Creates an action to start executing the feature
                InputAction startAction = executeFeatures.AddAction($"Start {playerFeature.MapName}", InputActionType.Button, $"<Keyboard>/{(i == 9 ? 0 : i + 1)}");

                ///Links it to the correct callback
                startAction.performed += ctx => ExecuteFeatureCallback(ctx, playerFeature.Feature);
            }

            executeFeatures.Enable();
        }

        private void ExecuteFeatureCallback(InputAction.CallbackContext context, Feature feature)
        {
            if (context.performed)
                ExecuteFeature(feature);
        }
        public void ExecuteFeature(Feature feature)
        {
            feature.EnableFeature();
            executeFeatures.Disable();
        }

        public void EndFeature(Feature feature)
        {
            feature.DisableFeature();
            executeFeatures.Enable();
        }
    }
}