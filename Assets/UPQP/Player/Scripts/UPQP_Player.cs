using System.Collections;
using System.Collections.Generic;
using Aokoro.Entities.Player;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.InputSystem;
using UPQP.Features;
using UPQP.Managers;
using Aokoro.UI.ControlsDiplaySystem;
using System;

namespace UPQP.Player
{
    public class UPQP_Player : PlayerManager
    {
        //Holds the data necessary for the UI
        public class UPQP_FeaturesActionProvider : ICD_InputActionsProvider
        {
            readonly UPQP_Player player;
            public event Action OnActionsNeedRefresh;


            public UPQP_FeaturesActionProvider(UPQP_Player player)
            {
                this.player = player;
                player.GetComponent<PlayerControls>().OnControlChanges += OnActionsNeedRefresh;
            }


            public string GetControlScheme() => player.playerInput.currentControlScheme;
            public InputAction[] GetInputActions() => player.executeFeatures.actions.ToArray();
            public InputDevice[] GetDevices() => player.playerInput.devices.ToArray();
        }

        [BoxGroup("Features"), SerializeField]
        private GameObject featuresControls;

        [SerializeField, BoxGroup("Features")]
        private Transform featuresRoot;
        public Transform FeaturesRoot => featuresRoot;

        [BoxGroup("Features"), ReadOnly, Space]
        public InputActionMap executeFeatures;


        private CD_Displayer featuresUI;

        public override void OnAwake()
        {
            SetupPlayerFeatures(LevelManager.Instance.CreateLevelFeatures());
            base.OnAwake();
        }

        protected override void SetupCursorForPlayer()
        {
            GameManager.Instance.cursorLockMode.Subscribe(this, Aokoro.PriorityTags.Small, CursorLockMode.Locked);
            GameManager.Instance.cursorVisibility.Subscribe(this, Aokoro.PriorityTags.Small, false);
        }

        private void SetupPlayerFeatures(Feature[] features)
        {
            //Find features that need to be started by the player
            var playerFeatures = GetComponentsInChildren<IPlayerFeature>();
            for (int i = 0; i < playerFeatures.Length; i++)
                playerFeatures[i].Player = this;

            if (features.Length == 0)
                return;

            //Cant be more than 10 because there are only 10 avaiable keys
            ///TODO Add 10 more binded to maj + number
            int length = Mathf.Min(features.Length, 10);

            //The map that will hold all actions to executes player features
            executeFeatures = new InputActionMap("executeFeatures");
            executeFeatures.Disable();

            //Filters to find this actions
            CD_ActionSettings[] actionSettings = new CD_ActionSettings[length];

            for (int i = 0; i < length; i++)
            {
                IPlayerFeature playerFeature = playerFeatures[i];

                ///Binds to player
                playerFeature.Player = this;

                string displayName = $"Start {playerFeature.MapName}";
                int digit = (i == 9 ? 0 : i + 1);

                ///Creates an action to start executing the feature binded to 1, then 2, then 3, etc....
                InputAction startAction = executeFeatures.AddAction(displayName, InputActionType.Button, $"<Keyboard>/{digit}", groups: "Keyboard&Mouse"); ;
                startAction.AddBinding($"<Keyboard>/numpad{digit}", groups: "Keyboard&Mouse");
                ///Links it to the correct callback
                startAction.performed += ctx => ExecuteFeatureCallback(ctx, playerFeature.Feature);

                //Indicates to the UI where to find this actions and how to name them
                actionSettings[i] = new CD_ActionSettings(displayName, playerFeature.Feature.FeatureName, 1);
            }

            executeFeatures.Enable();

            //Controls UI creation
            Aokoro.UI.GameUIManager mainUI = Aokoro.UI.GameUIManager.MainUI;
            //Acces to the main UI
            GameObject parent = mainUI.GetWindow(mainUI.DefaultWindow).windowObject;
            featuresUI = Instantiate(featuresControls, parent.transform).GetComponent<CD_Displayer>();

            //Setup the UI
            featuresUI.actionSettings = new CD_Settings(actionSettings);
            featuresUI.AssignActionProvider(new UPQP_FeaturesActionProvider(this), true);
        }

        protected override void Start()
        {
            base.Start();
            featuresUI?.Show();
        }
        private void ExecuteFeatureCallback(InputAction.CallbackContext context, Feature feature)
        {
            if (context.performed)
                ExecuteFeature(feature);
        }
        public void ExecuteFeature(Feature feature)
        {
            feature.EnableFeature();
            feature.IsActive = true;
            executeFeatures.Disable();
        }

        public void EndFeature(Feature feature)
        {
            feature.DisableFeature();
            feature.IsActive = false;
            executeFeatures.Enable();
        }


    }
}