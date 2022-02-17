using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace UPQP.CameraManagement
{

    public class FirstPersonCameraController : PlayerCamController
    {
        public override void Initiate(PlayerManager manager)
        {
            base.Initiate(manager);
            transform.SetParent(null);
            Debug.Log("a");
        }
    }
}