
using Aokoro.Entities;
using Aokoro.Entities.Player;
using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

namespace UPQP.Player.CameraManagement
{
    public class PlayerCamController : CinemachineInputProvider, IEntityComponent<PlayerManager>
    {
        string IEntityComponent.ComponentName => "PlayerCamController";
        public PlayerManager Manager { get; set; }

        private InputAction lookAction;

        [Header("Cam parameters")]
        [SerializeField, Range(.01f, 4)]
        private float verticalSpeed;
        [SerializeField, Range(.01f, 4)]
        private float horizontalSpeed;

        public virtual void Initiate(PlayerManager manager)
        {
            lookAction = manager.playerInput.actions.FindActionMap("DefaultGameplay").FindAction("Look");
            lookAction.Enable();
            XYAxis.Set(lookAction);
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
