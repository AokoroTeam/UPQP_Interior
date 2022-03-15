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
        public Feature Feature => _Feature;
        public string MapName => mapName;

        [SerializeField] private string mapName;
        [SerializeField] private InputActionAsset actions;

        public UPQP_Player Player { get; set; }
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

        public abstract void OnFeatureEnables();
        public abstract void OnFeatureDisables();

        public abstract void BindToNewActions(InputActionAsset actions);



        #region Interfaces

        InputAction[]  CD_InputActionsProvider.GetInputActions() => ActionAsset.actionMaps[0].actions.ToArray();

        string CD_InputActionsProvider.GetCurrentDeviceName() => Player.playerInput.currentControlScheme;

       
        

        #endregion
    }
}