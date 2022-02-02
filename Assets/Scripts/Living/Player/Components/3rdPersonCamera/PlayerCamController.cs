using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.InputSystem;
using Aokoro.Tools;
using static Cinemachine.AxisState;
using UPQP.Movement;

namespace UPQP.CameraManagement
{
    public class PlayerCamController : CinemachineInputProvider, ILivingComponent<PlayerManager>
    {
        private CinemachineFreeLook freeLook;

        public PlayerManager Manager { get; set; }

        public bool HasUpdate => true;

        public bool HasFixedUpdate => false;

        public bool HasLateUpdate => false;

        private InputAction lookAction;

        [Header("Cam parameters")]
        [SerializeField, Range(.01f, 4)]
        private float verticalSpeed;
        [SerializeField, Range(.01f, 4)]
        private float horizontalSpeed;
        [SerializeField, Range(0, 1)]
        private float speed;

        [Header("Heading")]
        [SerializeField] private float headingStrenght;

        public ComplexeProperty<Vector3> Heading = new ComplexeProperty<Vector3>();

        private PlayerCharacter playerCharacter;

        public void Initiate(PlayerManager manager)
        {
            manager.GetLivingComponent(out playerCharacter);

            freeLook = GetComponentInChildren<CinemachineFreeLook>();
            lookAction = Manager.playerInput.actions.FindActionMap("DefaultGameplay").FindAction("Look");
            lookAction.Enable();
        }

        public void UpdateComponent()
        {
            if(playerCharacter)
            {
                bool autoRecenter = playerCharacter.IsOnGround() && playerCharacter.GetVelocity().sqrMagnitude > .2f;

                freeLook.m_RecenterToTargetHeading.m_enabled = autoRecenter;
                freeLook.m_YAxisRecentering.m_enabled = autoRecenter;

            }
        }

        public void FixedUpdateComponent()
        {

        }

        public void LateUpdateComponent()
        {


        }

        public void DisableInputs()
        {
            lookAction.Disable();
        }

        public void EnableInputs()
        {
            lookAction.Enable();
        }


        public override float GetAxisValue(int axis)
        {
            if (enabled)
            {
                var action = ResolveForPlayer(axis, axis == 2 ? ZAxis : XYAxis);
                if (action != null)
                {
                    switch (axis)
                    {
                        case 0: return action.ReadValue<Vector2>().x;
                        case 1: return action.ReadValue<Vector2>().y;
                        case 2: return action.ReadValue<float>();
                    }
                }
            }

            return 0;
        }
    }
}
