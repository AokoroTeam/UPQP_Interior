using Aokoro.Tools.Cinematiques;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace UPQP
{
    public class PlayerManager : LivingBehaviour
    {
        public event Action<string, string> OnMapChange;
        public event Action OnRespawn;

        [HideInInspector]
        public PlayerInput playerInput;
        [HideInInspector]
        public Animator anim;
        [HideInInspector]
        public Rigidbody rb;

        [HideInInspector]
        public TimelineActor timelineActor;

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
            anim = GetComponent<Animator>();
            rb = GetComponent<Rigidbody>();
            timelineActor = GetComponent<TimelineActor>();

            Initiate<PlayerManager>();

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }


        public void Respawn(Vector3 position, Quaternion rotation)
        {
            rb.position = position;
            rb.rotation = rotation;
            OnRespawn?.Invoke();
        }

        public void ChangeActionMap(string targetMap)
        {
            if (playerInput.actions.FindActionMap(targetMap) != null)
            {
                string currentMap = playerInput.currentActionMap.name;
                playerInput.SwitchCurrentActionMap(targetMap);

                OnMapChange?.Invoke(currentMap, targetMap);
            }
        }
    }
}
