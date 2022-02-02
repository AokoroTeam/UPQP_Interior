using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UPQP.Movement;

namespace UPQP.Animations
{
    public class PlayerAnimatorEventReceiver : MonoBehaviour
    {
        private PlayerMovement playerMovement;

        private void Awake()
        {
            playerMovement = GetComponentInParent<PlayerMovement>();
        }
        public void Foot_R(AnimationEvent animationEvent)
        {

        }

        public void Foot_L(AnimationEvent animationEvent)
        {

        }
    }
}
