using Aokoro.Entities;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using NaughtyAttributes;

namespace Aokoro.Entities.Player
{
    public class PlayerManager : Entity
    {
        public event Action<string, string> OnMapChange;
        public event Action OnRespawn;

        [HideInInspector]
        public PlayerInput playerInput;
        public string DefaultActionMap;

        [HideInInspector]
        public Animator anim;
        [HideInInspector]
        public Rigidbody rb;
        [Space]
        [ReadOnly]
        public InputActionMap currentMap;

        public static PlayerManager LocalPlayer
        {
            get
            {
                if (localPlayer == null)
                {
                    GameObject player = GameObject.FindGameObjectWithTag("Player");
                    if (player != null)
                        localPlayer = player.GetComponent<PlayerManager>();

                }

                return localPlayer;
            }
        }

        public AudioListener AudioListener
        {
            get
            {
                if (audioListener == null)
                    audioListener = Camera.main.GetComponent<AudioListener>();

                return audioListener;
            }
        }

        private AudioListener audioListener;

        private static PlayerManager localPlayer;

        protected override void Awake()
        {
            if (localPlayer != this && localPlayer != null)
            {
                Destroy(gameObject);
                return;
            }
            else
                localPlayer = this;

            playerInput = GetComponent<PlayerInput>();
            playerInput.actions = GenerateInputActionAsset();
            SetupActionAsset(playerInput.actions);

            anim = GetComponent<Animator>();
            rb = GetComponent<Rigidbody>();

            Initiate<PlayerManager>();

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        private void Start()
        {
            ChangeActionMap(DefaultActionMap);
        }
        private void SetupActionAsset(InputActionAsset actions)
        {
            actions.Enable();
        }
        protected virtual InputActionAsset GenerateInputActionAsset()
        {
            var asset = ScriptableObject.CreateInstance<InputActionAsset>();
            IPlayerInputAssetProvider[] inputProviders = GetComponentsInChildren<IPlayerInputAssetProvider>();

            for (int i = 0; i < inputProviders.Length; i++)
            {
                IPlayerInputAssetProvider inputProvider = inputProviders[i];
                InputActionAsset subAsset = inputProvider.ActionAsset;

                //ControlSchemes
                foreach (var scheme in subAsset.controlSchemes)
                {
                    if (!asset.FindControlScheme(scheme.name).HasValue)
                        asset.AddControlScheme(scheme);
                }

                foreach (InputActionMap map in subAsset.actionMaps)
                    asset.AddActionMap(map.Clone());
            }
            return asset;
        }

        public void Respawn(Vector3 position, Quaternion rotation)
        {
            rb.position = position;
            rb.rotation = rotation;
            OnRespawn?.Invoke();
        }

        public void ChangeActionMap() => ChangeActionMap(DefaultActionMap);
        public void ChangeActionMap(string targetMap)
        {
            if (playerInput.actions.FindActionMap(targetMap) != null)
            {
                InputActionMap currentActionMap = playerInput.currentActionMap;
                if (currentActionMap != null)
                {
                    string currentMap = currentActionMap.name;
                    OnMapChange?.Invoke(currentMap, targetMap);
                }

                playerInput.SwitchCurrentActionMap(targetMap);

                currentMap = playerInput.currentActionMap;
            }
            else
                Debug.Log($"Map {targetMap} not found");
        }
    }
}
