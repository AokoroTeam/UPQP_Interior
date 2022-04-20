using Aokoro.Entities.Player;
using Cinemachine;
using UnityEngine;

namespace UPQP.Player.CameraManagement
{

    public class FirstPersonCameraController : PlayerCamController
    {
        CinemachineVirtualCamera Vcam
        {
            get
            {
                if (_vcam == null)
                    _vcam = GetComponent<CinemachineVirtualCamera>();
                return _vcam;
            }
        }
        CinemachinePOV Pov
        {
            get
            {
                if (_pov == null)
                    _pov = Vcam.GetCinemachineComponent<CinemachinePOV>();
                return _pov;
            }
        }
        CinemachineVirtualCamera _vcam;
        CinemachinePOV _pov;

        public override void Initiate(PlayerManager manager)
        {
            base.Initiate(manager);
            transform.SetParent(null);


        }

        public void OnCameraLive(ICinemachineCamera from, ICinemachineCamera to)
        {
            Recenter();
        }
        private void Start()
        {
            Recenter();
        }

        private void Recenter()
        {
            Quaternion rot = Quaternion.LookRotation(Vcam.LookAt.transform.forward, Vector3.up);
            Pov.ForceCameraPosition(Vcam.transform.position, rot);
            //Pov.transform.rotation = rot;
            //Pov.m_HorizontalAxis.Value = Pov.m_VerticalAxis.Value = 0;
        }
    }
}