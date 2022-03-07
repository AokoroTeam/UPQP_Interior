using Aokoro.Entities.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;
using UPQP.Features;

namespace UPQP.Features.SliceView
{

    public class SliceView_Player : PlayerFeature
    {
        private CinemachineOrbitalTransposer vCam;


        public override void StartFeature()
        {
            SliceView_Manager.Instance.OnFeatureStarts();
            player.Freezed.Subscribe(this, 20, true);
        }

        public override void EndFeature()
        {
            SliceView_Manager.Instance.OnFeatureStarts();
            player.Freezed.Unsubscribe(this);
        }

        private void Start()
        {
            vCam = SliceView_Manager.Instance.virtualCamera.GetCinemachineComponent<CinemachineOrbitalTransposer>();
        }
        public override void BindToNewActions(InputActionMap[] maps) 
        {
            var map = maps[0];
            map.FindAction("Rotate").performed += OnRotate_performed;
            map.FindAction("Exit").performed += OnExit_performed;
        }

        private void OnExit_performed(InputAction.CallbackContext ctx) => player.EndFeature(this);

        private void OnRotate_performed(InputAction.CallbackContext ctx) => vCam.m_XAxis.m_InputAxisValue = ctx.ReadValue<float>();
    }
}