using EasyCharacterMovement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UPQP.Player.Movement
{
    public class PlayerCharacter : Character, ILivingComponent<PlayerManager>
    {
        public PlayerManager Manager { get; set; }

        public bool HasUpdate => false;

        public bool HasFixedUpdate => false;

        public bool HasLateUpdate => false;

        public override bool CanJump() => false;


        public void Initiate(PlayerManager manager)
        {

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

        public void UpdateComponent()
        {

        }

        public void FixedUpdateComponent()
        {

        }

        public void LateUpdateComponent()
        {

        }
    }
}