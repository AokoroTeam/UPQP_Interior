using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.InputSystem;
using Aokoro.Tools;
using static Cinemachine.AxisState;
using UPQP.Player.Movement;

namespace UPQP.Player.CameraManagement
{
    public class PlayerCamController : CinemachineInputProvider, ILivingComponent<PlayerManager>
    {
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

        public virtual void Initiate(PlayerManager manager)
        {

            lookAction = Manager.playerInput.actions.FindActionMap("DefaultGameplay").FindAction("Look");
            XYAxis.Set(lookAction);

            lookAction.Enable();
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
