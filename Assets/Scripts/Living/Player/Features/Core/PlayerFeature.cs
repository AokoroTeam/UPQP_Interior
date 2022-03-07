using Aokoro.Entities.Player;
using Aokoro.UIManagement.ControlsDiplaySystem;
using UnityEngine;
using UnityEngine.InputSystem;
using UPQP.Player;

namespace UPQP.Features
{

    public abstract class PlayerFeature : CD_InputActionsProvider, IPlayerInputAssetProvider
    {
        [SerializeField] public string featureName;
        [SerializeField] private InputActionAsset actions;
        internal UPQP_Player player;
        public InputActionAsset ActionAsset { get => actions; set => actions = value; }

        public abstract void StartFeature();
        public abstract void EndFeature();

        public override InputAction[] GetInputActions() => ActionAsset.actionMaps[0].actions.ToArray();

        public abstract void BindToNewActions(InputActionMap[] maps);
    }
}