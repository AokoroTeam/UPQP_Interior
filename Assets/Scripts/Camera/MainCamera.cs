using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Aokoro.Tools;
using System;

namespace UPQP.CameraManagement
{

    [RequireComponent(typeof(CinemachineBrain))]
    public class MainCamera : Singleton<MainCamera>
    {
        public CinemachineBrain Brain { get; private set; }

        protected override void Awake()
        {
            Brain = GetComponent<CinemachineBrain>();
        }

        protected override void OnExistingInstanceFound(MainCamera existingInstance)
        {
            Destroy(gameObject);
        }
    }
}
