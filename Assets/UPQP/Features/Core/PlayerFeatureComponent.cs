using Aokoro.Entities.Player;
using Aokoro.UI.ControlsDiplaySystem;
using NaughtyAttributes;
using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UPQP.Player;

namespace UPQP.Features
{

    public abstract class PlayerFeatureComponent<T> : FeatureComponent<T>, IPlayerFeature, CD_InputActionsProvider, IPlayerInputAssetProvider where T : Feature
    {
        public event Action OnResfreshNeeded;

        [SerializeField] public string featureName;
        [SerializeField] private InputActionAsset actions;
        [ReadOnly] public UPQP_Player player;
        private PlayerControls playerControls;

        public InputActionAsset ActionAsset { get => actions; set => actions = value; }


        protected override void Awake()
        {
            base.Awake();
            playerControls = GetComponentInParent<PlayerControls>();
        }

        protected override void Start()
        {
            base.Start();
        }

        private void OnEnable()
        {
            playerControls.OnControlChanges += TriggerRefresh;
        }

        private void OnDisable()
        {
            playerControls.OnControlChanges -= TriggerRefresh;
        }
        private void TriggerRefresh() => OnResfreshNeeded?.Invoke();

        public abstract void ExecuteFeature();
        public abstract void EndFeature();

        public abstract void BindToNewActions(InputActionMap[] maps);



        #region Interfaces

        InputAction[]  CD_InputActionsProvider.GetInputActions() => ActionAsset.actionMaps[0].actions.ToArray();

        string CD_InputActionsProvider.GetCurrentDeviceName() => player.playerInput.currentControlScheme;

        Feature IPlayerFeature.Feature => _Feature;

        string IPlayerFeature.MapName => featureName;

        UPQP_Player IPlayerFeature.Player { get => player; set => player = value; }

        #endregion
    }
}