using Aokoro.Entities.Player;
using Aokoro.UI.ControlsDiplaySystem;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.InputSystem;
using UPQP.Player;

namespace UPQP.Features
{

    public abstract class PlayerFeatureComponent<T> : FeatureComponent<T>, IPlayerFeature, CD_InputActionsProvider, IPlayerInputAssetProvider where T : Feature
    {
        [SerializeField] public string featureName;
        [SerializeField] private InputActionAsset actions;
        [ReadOnly] public UPQP_Player player;

        public InputActionAsset ActionAsset { get => actions; set => actions = value; }
        public abstract void ExecuteFeature();
        public abstract void EndFeature();

        public abstract void BindToNewActions(InputActionMap[] maps);


        #region Interfaces

        InputAction[]  CD_InputActionsProvider.GetInputActions() => ActionAsset.actionMaps[0].actions.ToArray();

        string CD_InputActionsProvider.GetCurrentDeviceName() => player.playerInput.currentControlScheme;

        Feature IPlayerFeature.Feature => _Feature;

        string IPlayerFeature.FeatureName => featureName;

        UPQP_Player IPlayerFeature.Player { get => player; set => player = value; }

        #endregion
    }
}