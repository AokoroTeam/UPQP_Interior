using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Aokoro.Entities;
using UPQP;
using System;

namespace Aokoro.UIManagement.Controls
{
    public class PlayerControls : ControlMapProvider, ILateUpdateLivingComponent<UPQP.PlayerManager>
    {
        private PlayerInput playerInput;
        public PlayerManager Manager { get; set; }

        private InputActionMap lastMap;

        public void Initiate(PlayerManager manager)
        {
            playerInput = manager.playerInput;
        }
        private void Awake()
        {
            playerInput = GetComponent<PlayerInput>();
        }
        private void Start()
        {
            ControlsManager.TriggerControlChanges(playerInput.defaultControlScheme);
        }
        private void OnEnable()
        {
            playerInput.onControlsChanged += OnControlsChanges;
        }

        private void OnDisable()
        {

            playerInput.onControlsChanged -= OnControlsChanges;
        }
        private void OnControlsChanges(PlayerInput playerInput)
        {
            ControlsManager.TriggerControlChanges(playerInput.currentControlScheme);
        }

        public void LateUpdateComponent()
        {
            if (lastMap != playerInput.currentActionMap)
            {
                lastMap = playerInput.currentActionMap;
                ControlsManager.TriggerControlChanges(playerInput.currentControlScheme);
            }
        }

        public override InputActionMap GetInputActionMap(string mapName) => playerInput.actions.FindActionMap(mapName, false);
    }
}