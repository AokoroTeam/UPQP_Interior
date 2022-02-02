using Aokoro.Tools;

using NaughtyAttributes;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

namespace UPQP.Movement
{
    public class PlayerMovement : MonoBehaviour, ILivingComponent<PlayerManager>
    {
        #region Interface
        public bool HasUpdate => true;

        public bool HasFixedUpdate => true;

        public bool HasLateUpdate => false;

        
        public PlayerManager Manager { get; set; }
        #endregion

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