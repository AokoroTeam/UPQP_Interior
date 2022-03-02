using Aokoro.Entities.Player;
using Aokoro.Entities;

using NaughtyAttributes;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

namespace UPQP.Player.Movement
{
    public class PlayerMovement : MonoBehaviour, IFixedUpdateLivingComponent<PlayerManager>, IUpdateLivingComponent<PlayerManager>
    {
        public PlayerManager Manager { get; set; }

        [Header("Audio")]
        [SerializeField] AudioSource footR;
        [SerializeField] AudioSource footL;
        [SerializeField] AudioClip[] concreteFootstepSounds;

        public void FixedUpdateComponent()
        {
        }

        public void Initiate(PlayerManager manager)
        {
        }

        public void LateUpdateComponent()
        {
        }

        public void UpdateComponent()
        {

        }

        public void Foot_R()
        {

        }

        public void Foot_L()
        {

        }
    }
}