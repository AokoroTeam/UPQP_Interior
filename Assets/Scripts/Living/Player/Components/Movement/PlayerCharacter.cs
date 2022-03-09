using Aokoro.Entities;
using EasyCharacterMovement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Aokoro.Entities.Player;
using UnityEngine.InputSystem;

namespace UPQP.Player.Movement
{
    public class PlayerCharacter : Character, IEntityComponent<PlayerManager>, IPlayerInputAssetProvider
    {
        public PlayerManager Manager { get; set; }
        public InputActionAsset ActionAsset { get => inputActions; set => inputActions = value; }

        public void BindToNewActions(InputActionMap[] maps)
        {

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
    }
}