using Aokoro.Entities;
using EasyCharacterMovement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Aokoro.Entities.Player;
using UnityEngine.InputSystem;
using Aokoro.UI.ControlsDiplaySystem;
using System;
using NaughtyAttributes;

namespace UPQP.Player.Movement
{
    [AddComponentMenu("UPQP/Player/Movement/PlayerCharacter")]
    public class PlayerCharacter : Character, IEntityComponent<PlayerManager>, IPlayerInputAssetProvider, CD_InputActionsProvider
    {
        [ReadOnly]
        public PlayerMovementUI UI;
        public PlayerManager Manager { get; set; }
        public InputActionAsset ActionAsset { get => inputActions; set => inputActions = value; }

        public event Action OnResfreshNeeded;

        private PlayerControls playerControls;
        
        public void BindToNewActions(InputActionMap[] maps)
        {

        }
        protected override void OnAwake()
        {
            base.OnAwake();
            playerControls = GetComponent<PlayerControls>();
        }

        protected override void OnStart()
        {
            base.OnStart();
            ShowUI();
        }
        public void ShowUI() => UI?.Show();
        public void HideUI() => UI?.Hide();

        protected override void OnOnEnable()
        {
            base.OnOnEnable();
            playerControls.OnControlChanges += TriggerRefresh;
        }

        protected override void OnOnDisable()
        {
            base.OnOnDisable();
            playerControls.OnControlChanges -= TriggerRefresh;
        }

        public void Initiate(PlayerManager manager)
        {
            inputActions = manager.playerInput.actions;
            camera = Camera.main;
        }
        protected override void Animate()
        {
            if (animator)
            {
                Vector3 currentVelocity = GetVelocity();
                float speed = GetSpeed();

                if (GetMovementInput().sqrMagnitude > .1f)
                {
                    var normVelocity = transform.InverseTransformDirection(currentVelocity).normalized;
                    Debug.DrawRay(base.GetPosition(), currentVelocity.normalized * 15);
                    animator.SetFloat("Norm_Forward_Speed", normVelocity.z, .1f, Time.deltaTime);
                    animator.SetFloat("Norm_Right_Speed", normVelocity.x, .1f, Time.deltaTime);
                    //animator.SetFloat("Angular_Speed", AngularVelocity.y);
                }

                animator.SetBool("IsMoving", speed > .05f);
                animator.SetBool("IsRunning", IsSprinting());
                animator.SetFloat("Speed", speed);
            }
        }


        #region CD_InputActionsProvider
        private void TriggerRefresh() => OnResfreshNeeded?.Invoke();
        public string GetCurrentDeviceName() => Manager.playerInput.currentControlScheme;

        public InputAction[] GetInputActions() => ActionAsset.actionMaps[0].actions.ToArray();

        #endregion
    }
}