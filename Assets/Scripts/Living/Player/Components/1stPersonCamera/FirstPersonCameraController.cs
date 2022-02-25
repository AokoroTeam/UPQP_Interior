using Aokoro.Entities.Player;

namespace UPQP.Player.CameraManagement
{

    public class FirstPersonCameraController : PlayerCamController
    {
        public override void Initiate(PlayerManager manager)
        {
            base.Initiate(manager);
            transform.SetParent(null);
        }
    }
}