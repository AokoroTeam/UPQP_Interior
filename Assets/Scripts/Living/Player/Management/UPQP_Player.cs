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
        [BoxGroup("Features")]
        public Transform FeaturesRoot;
        [BoxGroup("Features"), ReadOnly, SerializeField]
        private Feature[] features;
        [BoxGroup("Features"), ReadOnly, Space]
        public InputActionMap executeFeatures;


        private IPlayerFeature[] playerFeatures;


        protected override void Awake()
        {
            if(LevelManager.Instance.LevelInitiationPhase != LevelManager.playerPhase)
                Destroy(gameObject);
            else
                base.Awake();
        }
        public override void OnAwake()
        {
            features = LevelManager.Instance.CreateLevelFeatures();
            SetupPlayerFeatures();
            base.OnAwake();
        }

        private void SetupPlayerFeatures()
        {
            playerFeatures = GetComponentsInChildren<IPlayerFeature>();

            int length = Mathf.Min(playerFeatures.Length, 10);

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
                startAction.performed += ctx => ExecuteFeatureCallback(ctx, playerFeature);
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
            ChangeActionMap(playerFeature.MapName);
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