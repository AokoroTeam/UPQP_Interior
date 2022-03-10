using Aokoro.Entities.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;
using UPQP.Features;

namespace UPQP.Features.SliceView
{

    public class SliceView_Player : PlayerFeatureComponent<SliceView>
    {
        private CinemachineOrbitalTransposer vCam;

        protected override void Initiate()
        {
        }

        protected override void Start()
        {
            vCam = _Feature.Manager.virtualCamera.GetCinemachineComponent<CinemachineOrbitalTransposer>();
            base.Start();
        }


        public override void ExecuteFeature()
        {
            _Feature.Manager.OnFeatureStarts();
            player.Freezed.Subscribe(this, 20, true);
        }

        public override void EndFeature()
        {
            _Feature.Manager.OnFeatureEnds();
            player.Freezed.Unsubscribe(this);
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