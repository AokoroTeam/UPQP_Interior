using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Aokoro.Entities.Player;
using Aokoro.Entities;
using System;

namespace Aokoro.UI.ControlsDiplaySystem
{
    public class PlayerControls : MonoBehaviour, ILateUpdateLivingComponent<PlayerManager>
    {
        string IEntityComponent.ComponentName => "PlayerControls";

        public event Action OnControlChanges;
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
            Debug.Log($"Awake" + (Keyboard.current == null ? "Keyboards not found" : $"Keyboard found layout is {Keyboard.current.keyboardLayout}"));
        }
        private void Update()
        {
            Debug.Log($"Update " + (Keyboard.current == null ? "Keyboards not found" : $"Keyboard found layout is  {Keyboard.current.keyboardLayout}"));
        }
        private void Start()
        {
            playerInput.SwitchCurrentControlScheme(InputSystem.devices.ToArray());
            lastMap = playerInput.currentActionMap;

            Debug.Log($"Start " + (Keyboard.current == null ? "Keyboards not found" : $"Keyboard found layout is {Keyboard.current.keyboardLayout}"));
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
            Debug.Log("Player controls have changed");
            OnControlChanges?.Invoke();
        }

        public void OnLateUpdate()
        {
            if (lastMap != playerInput.currentActionMap)
            {
                lastMap = playerInput.currentActionMap;
                OnControlsChanges(playerInput);
            }
        }
    }
}